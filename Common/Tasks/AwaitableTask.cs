namespace Gamefreak130.Common.Tasks
{
    using Gamefreak130.Common.Helpers;
    using Gamefreak130.Common.Loggers;
    using Sims3.SimIFace;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    public enum AwaitableTaskStatus : byte
    {
        Created,
        WaitingForContinuation,
        WaitingToRun,
        Running,
        RanToCompletion,
        Canceled,
        Faulted
    }

    public abstract partial class AwaitableTask : IDisposable
    {
        private const string kNoTasksMessage = "At least one task must be provided.";

        private interface IContinuationTask
        {
            AwaitableTaskStatus Status { get; }

            void StartAsContinuation(AwaitableTask antecedent);
        }

        private class SimulatorWrapper : CommonTask
        {
            private readonly AwaitableTask mTaskToRun;

            public SimulatorWrapper(AwaitableTask taskToRun)
                => mTaskToRun = taskToRun;

            public override void Dispose() => Dispose(true);

            public void Dispose(bool fromSimulator)
            {
                if (fromSimulator)
                {
                    mTaskToRun.Dispose();
                }
                base.Dispose();
            }

            public override void Simulate() => mTaskToRun.Simulate();

            public override string ToString() => mTaskToRun.ToString() + $", ObjectId: {ObjectId}";

            protected sealed override void Perform() => mTaskToRun.Perform();
        }

        private class WhenAnyPromise<TTask> : AwaitableTask<TTask>, IContinuationTask where TTask : AwaitableTask
        {
            protected AwaitableTask mAntecedent;

            public WhenAnyPromise() : base(true)
                => Status = AwaitableTaskStatus.WaitingForContinuation;

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                mAntecedent = antecedent;
                StartInternal();
            }

            protected override TTask GetResult()
            {
                TTask antecedent = mAntecedent as TTask;
                mAntecedent = null;
                return antecedent;
            }
        }

        private class WhenAllPromise<TResult> : AwaitableTask<TResult[]>, IContinuationTask
        {
            private int mNumRemaining;

            private AwaitableTask[] mAntecedentTasks;

            public WhenAllPromise(AwaitableTask[] tasks) : base(true)
            {
                Status = AwaitableTaskStatus.WaitingForContinuation;
                mAntecedentTasks = tasks;
                mNumRemaining = mAntecedentTasks.Length;
                foreach (AwaitableTask task in mAntecedentTasks)
                {
                    task.AddContinuation(this);
                }
            }

            protected override void Dispose(bool fromSimulator)
            {
                mAntecedentTasks = null;
                base.Dispose(fromSimulator);
            }

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                if (--mNumRemaining < 0)
                {
                    throw new Exception("Remaining task count should never go below 0.");
                }
                if (mNumRemaining == 0)
                {
                    StartInternal();
                }
            }

            protected override TResult[] GetResult()
            {
                TResult[] results = new TResult[mAntecedentTasks.Length];
                List<Exception> observedExceptions = null;
                bool shouldFault = false;
                bool shouldCancel = false;
                for (int i = 0; i < mAntecedentTasks.Length; i++)
                {
                    AwaitableTask task = mAntecedentTasks[i];
                    if (task.IsFaulted)
                    {
                        observedExceptions ??= new();
                        observedExceptions.AddRange(task.Exception.InnerExceptions);
                        shouldFault = true;
                    }
                    else if (task.IsCanceled)
                    {
                        ReadOnlyCollection<Exception> innerExceptions = task.Exception.InnerExceptions;
                        if (innerExceptions.Count > 1)
                        {
                            observedExceptions ??= new();
                            for (int j = 0; j < innerExceptions.Count - 1; i++)
                            {
                                observedExceptions.Add(innerExceptions[j]);
                            }
                        }
                        shouldCancel = true;
                    }
                    else
                    {
                        results[i] = task is AwaitableTask<TResult> taskWithResult ? taskWithResult.Await() : default;
                    }
                    mAntecedentTasks[i] = null;
                }

                if (observedExceptions?.Count > 0)
                {
                    mInnerExceptions = observedExceptions;
                }

                return shouldCancel && !shouldFault ? throw new ResetException() : results;
            }
        }

        private class ContinueWhenAllPromise<TTask> : AwaitableTask<TTask[]>, IContinuationTask where TTask : AwaitableTask
        {
            private int mNumRemaining;

            private TTask[] mAntecedentTasks;

            public ContinueWhenAllPromise(TTask[] tasks) : base(true)
            {
                Status = AwaitableTaskStatus.WaitingForContinuation;
                mAntecedentTasks = tasks;
                mNumRemaining = mAntecedentTasks.Length;
                foreach (AwaitableTask task in mAntecedentTasks)
                {
                    task.AddContinuation(this);
                }
            }

            protected override void Dispose(bool fromSimulator)
            {
                mAntecedentTasks = null;
                base.Dispose(fromSimulator);
            }

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                if (--mNumRemaining < 0)
                {
                    throw new Exception("Remaining task count should never go below 0.");
                }
                if (mNumRemaining == 0)
                {
                    StartInternal();
                }
            }

            protected override TTask[] GetResult() => mAntecedentTasks;
        }

        private SimulatorWrapper mWrapper;

        private Queue<IContinuationTask> mContinuations;

        private List<Exception> mInnerExceptions;

        private bool mExceptionObserved;

        private readonly bool mSynchronous;

        public AwaitableTaskStatus Status { get; protected set; }

        public AggregateException Exception 
        { 
            get
            {
                TaskCanceledException canceledException = null;
                if (IsCanceled)
                {
                    canceledException = new(this);
                }

                if (mInnerExceptions is not null)
                {
                    if (!mExceptionObserved)
                    {
                        mExceptionObserved = true;
                        GC.SuppressFinalize(this);
                    }

                    if (canceledException is null)
                    {
                        return new(mInnerExceptions);
                    }

                    Exception[] innerExceptions = new Exception[mInnerExceptions.Count + 1];
                    for (int i = 0; i < mInnerExceptions.Count; i++)
                    {
                        innerExceptions[i] = mInnerExceptions[i];
                    }
                    innerExceptions[mInnerExceptions.Count] = canceledException;
                    return new(innerExceptions);
                }

                return canceledException is not null ? (new(canceledException)) : null;
            }
        }

        public bool IsCompleted => Status is AwaitableTaskStatus.RanToCompletion or AwaitableTaskStatus.Canceled or AwaitableTaskStatus.Faulted;

        public bool IsCompletedSuccessfully => Status is AwaitableTaskStatus.RanToCompletion;

        public bool IsCanceled => Status is AwaitableTaskStatus.Canceled;

        public bool IsFaulted => Status is AwaitableTaskStatus.Faulted;

        // TODO Add documentation and better error/argument handling
        ~AwaitableTask()
        {
            try
            {
                if (!mExceptionObserved && mInnerExceptions is not null)
                {
                    ThrowIfExceptional(false);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.sInstance.Log(ex);
            }
        }

        protected AwaitableTask(bool runSynchronously)
            => mSynchronous = runSynchronously;

        private void Simulate()
        {
            try
            {
                Status = AwaitableTaskStatus.Running;
                Perform();
                Status = mInnerExceptions is not null ? AwaitableTaskStatus.Faulted : AwaitableTaskStatus.RanToCompletion;
            }
            catch (ResetException)
            {
                Status = AwaitableTaskStatus.Canceled;
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            finally
            {
                try
                {
                    RunContinuations();
                    Dispose();
                }
                catch (ResetException)
                {
                }
                catch (ExecutionEngineException)
                {
                }
                catch (Exception ex)
                {
                    ExceptionLogger.sInstance.Log(ex);
                }
            }
        }

        protected abstract void Perform();

        protected void AddException(Exception ex)
        {
            Status = AwaitableTaskStatus.Faulted;
            mInnerExceptions ??= new(1);
            mInnerExceptions.Add(ex);
        }

        public ObjectGuid Start() 
            => this is IContinuationTask
            ? throw new NotSupportedException("Start may not be called on a continuation task.")
            : Status is not (AwaitableTaskStatus.Created or AwaitableTaskStatus.WaitingForContinuation)
            ? throw new InvalidOperationException("A task can only be run once.")
            : StartInternal();

        protected ObjectGuid StartInternal()
        {
            if (mSynchronous)
            {
                Simulate();
                return ObjectGuid.InvalidObjectGuid;
            }
            else
            {
                Status = AwaitableTaskStatus.WaitingToRun;
                mWrapper = new(this);
                return mWrapper.Start();
            }
        }

        public void Cancel() => Dispose(false);

        private void CancelInternal()
        {
            if (!IsCompleted)
            {
                Status = AwaitableTaskStatus.Canceled;
                RunContinuations();
            }
        }

        public void Dispose() => Dispose(false);

        protected virtual void Dispose(bool fromSimulator)
        {
            CancelInternal();
            if (!fromSimulator)
            {
                mWrapper?.Dispose(false);
            }
            mWrapper = null;
        }

        public void Await()
        {
            if (Status is AwaitableTaskStatus.Created)
            {
                Start();
            }

            WaitForCompletion();
            ThrowIfExceptional(true);
        }

        public void Await(uint delay, StopWatch.TickStyles tickStyles = StopWatch.TickStyles.Milliseconds)
        {
            if (Status is AwaitableTaskStatus.Created)
            {
                Start();
            }

            WaitForCompletion(delay, tickStyles);
            ThrowIfExceptional(true);
        }

        private void WaitForCompletion()
        {
            while (!IsCompleted)
            {
                TaskEx.Yield(true);
            }
        }

        private void WaitForCompletion(uint delay, StopWatch.TickStyles tickStyles)
        {
            using (StopWatch timer = StopWatchEx.StartNew(tickStyles))
            {
                while (!IsCompleted)
                {
                    if (timer.GetElapsedTime() > delay)
                    {
                        throw new TimeoutException();
                    }
                    TaskEx.Yield(true);
                }
            }
        }

        private void ThrowIfExceptional(bool includeCancelExceptions)
        {
            if (IsFaulted || (IsCanceled && includeCancelExceptions))
            {
                throw Exception;
            }
        }

        private void AddContinuation(IContinuationTask continuationTask)
        {
            if (continuationTask.Status is AwaitableTaskStatus.WaitingForContinuation)
            {
                if (IsCompleted)
                {
                    continuationTask.StartAsContinuation(this);
                }
                else
                {
                    mContinuations ??= new(1);
                    mContinuations.Enqueue(continuationTask);
                }
            }
        }

        private void RunContinuations()
        {
            while (mContinuations?.Count > 0)
            {
                IContinuationTask nextContinuation = mContinuations.Dequeue();
                if (nextContinuation.Status is AwaitableTaskStatus.WaitingForContinuation)
                {
                    nextContinuation.StartAsContinuation(this);
                }
            }
        }

        public override string ToString() => BuildString(new()).ToString();

        private StringBuilder BuildString(StringBuilder builder)
        {
            builder.Append($"{base.ToString()}, Status: {Status}, Exceptions: ");

            AggregateException aggregateException = Exception;
            builder.Append("[");
            if (aggregateException is not null)
            {
                BuildExceptionString(builder, aggregateException);
            }
            builder.Append(" ]");

            builder.Append(", Pending Continuations: [");
            if (mContinuations is not null)
            {
                foreach (IContinuationTask task in mContinuations)
                {
                    builder.Append($" ({(task as AwaitableTask).BuildString(builder)})");
                }
            }
            builder.Append(" ]");
            return builder;
        }

        private void BuildExceptionString(StringBuilder builder, AggregateException aggregateException)
        {
            foreach (Exception ex in aggregateException.InnerExceptions)
            {
                if (ex is AggregateException innerAggregate)
                {
                    BuildExceptionString(builder, innerAggregate);
                }
                else
                {
                    builder.Append($" ({ex.GetType().Name})");
                }
            }
        }

        internal static AwaitableTask<AwaitableTask> WhenAny(params AwaitableTask[] tasks)
            => WhenAnyInternal(tasks);

        internal static AwaitableTask<AwaitableTask<TResult>> WhenAny<TResult>(params AwaitableTask<TResult>[] tasks)
            => WhenAnyInternal(tasks);

        private static AwaitableTask<TTask> WhenAnyInternal<TTask>(TTask[] tasks) where TTask : AwaitableTask
        {
            if (tasks.Length == 0)
            {
                throw new ArgumentException(kNoTasksMessage, nameof(tasks));
            }

            // We would just like to do this:
            //    return (Task<Task<TResult>>) WhenAny( (Task[]) tasks);
            // but classes are not covariant to enable casting Task<TResult> to Task<Task<TResult>>.

            // Check the first two tasks for completion so that we can avoid constructing an unnecessary task
            // In what is apparently the most common use case, according to Microsoft
            if (tasks[0].IsCompleted)
            {
                return TaskEx.FromResult(tasks[0]);
            }
            if (tasks.Length >= 2 && tasks[1].IsCompleted)
            {
                return TaskEx.FromResult(tasks[1]);
            }

            WhenAnyPromise<TTask> completionTask = new();
            foreach (AwaitableTask task in tasks)
            {
                task.AddContinuation(completionTask);
            }
            return completionTask;
        }

        internal static AwaitableTask WhenAll(params AwaitableTask[] tasks)
            => WhenAllInternal<object>(tasks);

        internal static AwaitableTask<TResult[]> WhenAll<TResult>(params AwaitableTask<TResult>[] tasks)
            => WhenAllInternal<TResult>(tasks);

        private static AwaitableTask<TResult[]> WhenAllInternal<TResult>(AwaitableTask[] tasks)
        {
            if (tasks.Length == 0)
            {
                throw new ArgumentException(kNoTasksMessage, nameof(tasks));
            }

            AwaitableTask[] tasksCopy = new AwaitableTask[tasks.Length];
            tasks.CopyTo(tasksCopy, 0);
            return new WhenAllPromise<TResult>(tasksCopy);
        }

        private static AwaitableTask<TTask[]> ContinueWhenAllInternal<TTask>(TTask[] tasks) where TTask : AwaitableTask
        {
            if (tasks.Length == 0)
            {
                throw new ArgumentException(kNoTasksMessage, nameof(tasks));
            }

            TTask[] tasksCopy = new TTask[tasks.Length];
            tasks.CopyTo(tasksCopy, 0);
            return new ContinueWhenAllPromise<TTask>(tasks);
        }
    }

    public abstract partial class AwaitableTask<TResult> : AwaitableTask
    {
        private TResult mResult;

        protected AwaitableTask(bool runSynchronously) : base(runSynchronously)
        {
        }

        protected AwaitableTask(TResult result) : base(true)
            => mResult = result;

        protected sealed override void Perform() => mResult = GetResult();

        protected abstract TResult GetResult();

        new public TResult Await()
        {
            base.Await();
            return mResult;
        }

        new public TResult Await(uint delay, StopWatch.TickStyles tickStyles = StopWatch.TickStyles.Milliseconds)
        {
            base.Await(delay, tickStyles);
            return mResult;
        }
    }
}
