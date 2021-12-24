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

    public abstract class AwaitableTask : IDisposable
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

        private class ContinuationActionTask<TTask> : ActionTask, IContinuationTask where TTask : AwaitableTask
        {
            protected AwaitableTask mAntecedent;

            public ContinuationActionTask(Action<TTask> action, bool runSynchronously) : this(runSynchronously)
                => mAction = () => {
                    TTask antecedent = mAntecedent as TTask;
                    mAntecedent = null;
                    action(antecedent); 
                };

            protected ContinuationActionTask(bool runSynchronously) : base(runSynchronously)
                => Status = AwaitableTaskStatus.WaitingForContinuation;

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                mAntecedent = antecedent;
                StartInternal();
            }
        }

        private class ContinuationActionTask<TTask, TParam> : ContinuationActionTask<TTask> where TTask : AwaitableTask
        {
            public ContinuationActionTask(Action<TTask, TParam> action, TParam param, bool runSynchronously) : base(runSynchronously)
                => mAction = () => {
                    TTask antecedent = mAntecedent as TTask;
                    mAntecedent = null;
                    action(antecedent, param);
                };
        }

        private class ContinuationFuncTask<TTask, TResult> : FunctionTask<TResult>, IContinuationTask where TTask : AwaitableTask
        {
            protected AwaitableTask mAntecedent;

            public ContinuationFuncTask(Func<TTask, TResult> action, bool runSynchronously) : this(runSynchronously) 
                => mFunc = () => {
                    TTask antecedent = mAntecedent as TTask;
                    mAntecedent = null;
                    return action(antecedent); 
                };

            protected ContinuationFuncTask(bool runSynchronously) : base(runSynchronously) 
                => Status = AwaitableTaskStatus.WaitingForContinuation;

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                mAntecedent = antecedent;
                StartInternal();
            }
        }

        private class ContinuationFuncTask<TTask, TParam, TResult> : ContinuationFuncTask<TTask, TResult> where TTask : AwaitableTask
        {
            public ContinuationFuncTask(Func<TTask, TParam, TResult> action, TParam param, bool runSynchronously) : base(runSynchronously)
                => mFunc = () => {
                    TTask antecedent = mAntecedent as TTask;
                    mAntecedent = null;
                    return action(antecedent, param);
                };
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

        private class UnwrapPromise<TResult> : AwaitableTask<TResult>, IContinuationTask
        {
            private enum UnwrapPromiseState : byte
            {
                WaitingOnOuterTask, // StartAsContinuation() means "process completed outer task"
                WaitingOnInnerTask, // StartAsContinuation() means "process completed inner task"
                Done,               // StartAsContinuation() means "something went wrong and we are hosed!"
            }

            private UnwrapPromiseState mState;

            private AwaitableTask mFinishingTask;

            public UnwrapPromise(AwaitableTask outerTask) : base(true)
            {
                Status = AwaitableTaskStatus.WaitingForContinuation;

                if (outerTask.IsCompleted)
                {
                    ProcessCompletedOuterTask(outerTask);
                }
                else
                {
                    outerTask.AddContinuation(this);
                }
            }

            private void ProcessCompletedOuterTask(AwaitableTask outerTask)
            {
                mState = UnwrapPromiseState.WaitingOnInnerTask;
                if (outerTask.IsFaulted || outerTask.IsCanceled)
                {
                    TrySetFromTask(outerTask);
                }
                else
                {
                    AwaitableTask innerTask = outerTask switch
                    {
                        AwaitableTask<AwaitableTask> taskOfTask                    => taskOfTask.Await(),
                        AwaitableTask<AwaitableTask<TResult>> taskOfTaskOfTResult  => taskOfTaskOfTResult.Await(),
                        _                                                          => throw new Exception("Outer task must be of type AwaitableTask<AwaitableTask> or AwaitableTask<AwaitableTask<TResult>>")
                    };

                    ProcessInnerTask(innerTask);
                }
            }

            private void ProcessInnerTask(AwaitableTask innerTask)
            {
                if (innerTask == null)
                {
                    Cancel();
                    mState = UnwrapPromiseState.Done;
                }
                else if (innerTask.IsCompleted)
                {
                    TrySetFromTask(innerTask);
                    mState = UnwrapPromiseState.Done;
                }
                else
                {
                    innerTask.AddContinuation(this);
                }
            }

            private void TrySetFromTask(AwaitableTask task)
            {
                mFinishingTask = task;
                StartInternal();
            }

            protected override TResult GetResult()
            {
                AwaitableTask finishingTask = mFinishingTask;
                mFinishingTask = null;
                switch (finishingTask.Status)
                {
                    case AwaitableTaskStatus.Canceled:
                        throw new ResetException();

                    case AwaitableTaskStatus.Faulted:
                        mInnerExceptions = new(finishingTask.Exception.InnerExceptions);
                        return default;

                    case AwaitableTaskStatus.RanToCompletion:
                        return finishingTask is AwaitableTask<TResult> taskWithResult ? taskWithResult.Await() : default;

                    default:
                        throw new Exception("UnwrapPromise in illegal state");
                }
            }

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                switch (mState)
                {
                    case UnwrapPromiseState.WaitingOnOuterTask:
                        ProcessCompletedOuterTask(antecedent);
                        break;
                    case UnwrapPromiseState.WaitingOnInnerTask:
                        TrySetFromTask(antecedent);
                        mState = UnwrapPromiseState.Done;
                        break;
                    default:
                        throw new Exception("UnwrapPromise in illegal state");
                }
            }
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

        public void Cancel()
        {
            CancelInternal();
            Dispose();
        }

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
            if (!IsCompleted)
            {
                CancelInternal();
            }
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

        public AwaitableTask ContinueWith(Action<AwaitableTask> action, bool runSynchronously = false)
            => ContinueWithInternal(this, action, runSynchronously);

        public AwaitableTask ContinueWith<TParam>(Action<AwaitableTask, TParam> action, TParam param, bool runSynchronously = false)
            => ContinueWithInternal(this, action, param, runSynchronously);

        public AwaitableTask<TResult> ContinueWith<TResult>(Func<AwaitableTask, TResult> func, bool runSynchronously = false)
            => ContinueWithInternal(this, func, runSynchronously);

        public AwaitableTask<TResult> ContinueWith<TParam, TResult>(Func<AwaitableTask, TParam, TResult> func, TParam param, bool runSynchronously = false)
            => ContinueWithInternal(this, func, param, runSynchronously);

        private protected static AwaitableTask ContinueWithInternal<TTask>(TTask antecedent, Action<TTask> action, bool runSynchronously = false)
            where TTask : AwaitableTask
        {
            ContinuationActionTask<TTask> task = new(action, runSynchronously);
            antecedent.AddContinuation(task);
            return task;
        }

        private protected static AwaitableTask ContinueWithInternal<TTask, TParam>(TTask antecedent, Action<TTask, TParam> action, TParam param, bool runSynchronously = false) 
            where TTask : AwaitableTask
        {
            ContinuationActionTask<TTask, TParam> task = new(action, param, runSynchronously);
            antecedent.AddContinuation(task);
            return task;
        }

        private protected static AwaitableTask<TResult> ContinueWithInternal<TTask, TResult>(TTask antecedent, Func<TTask, TResult> func, bool runSynchronously = false) 
            where TTask : AwaitableTask
        {
            ContinuationFuncTask<TTask, TResult> task = new(func, runSynchronously);
            antecedent.AddContinuation(task);
            return task;
        }

        private protected static AwaitableTask<TResult> ContinueWithInternal<TTask, TParam, TResult>(TTask antecedent, Func<TTask, TParam, TResult> func, TParam param, bool runSynchronously = false) 
            where TTask : AwaitableTask
        {
            ContinuationFuncTask<TTask, TParam, TResult> task = new(func, param, runSynchronously);
            antecedent.AddContinuation(task);
            return task;
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

        internal static AwaitableTask ContinueWhenAll(AwaitableTask[] tasks, Action<AwaitableTask[]> action, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        internal static AwaitableTask ContinueWhenAll<TParam>(AwaitableTask[] tasks, Action<AwaitableTask[], TParam> action, TParam param, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        internal static AwaitableTask ContinueWhenAll<TAntecedentResult>(AwaitableTask<TAntecedentResult>[] tasks, Action<AwaitableTask<TAntecedentResult>[]> action, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        internal static AwaitableTask ContinueWhenAll<TAntecedentResult, TParam>(AwaitableTask<TAntecedentResult>[] tasks, Action<AwaitableTask<TAntecedentResult>[], TParam> action, TParam param, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        internal static AwaitableTask<TResult> ContinueWhenAll<TResult>(AwaitableTask[] tasks, Func<AwaitableTask[], TResult> action, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        internal static AwaitableTask<TResult> ContinueWhenAll<TParam, TResult>(AwaitableTask[] tasks, Func<AwaitableTask[], TParam, TResult> action, TParam param, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        internal static AwaitableTask<TResult> ContinueWhenAll<TAntecedentResult, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>[], TResult> action, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        internal static AwaitableTask<TResult> ContinueWhenAll<TAntecedentResult, TParam, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>[], TParam, TResult> action, TParam param, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

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

        internal static AwaitableTask Unwrap(AwaitableTask<AwaitableTask> task)
            => task.IsCompletedSuccessfully
            ? task.Await() ?? TaskEx.FromCanceled()
            : new UnwrapPromise<object>(task);

        internal static AwaitableTask<TResult> Unwrap<TResult>(AwaitableTask<AwaitableTask<TResult>> task)
            => task.IsCompletedSuccessfully
            ? task.Await() ?? TaskEx.FromCanceled<TResult>()
            : new UnwrapPromise<TResult>(task);
    }

    public abstract class AwaitableTask<TResult> : AwaitableTask
    {
        private TResult mResult;

        protected AwaitableTask(bool runSynchronously) : base(runSynchronously)
        {
        }

        protected AwaitableTask(TResult result) : base(true)
            => mResult = result;

        protected sealed override void Perform() => mResult = GetResult();

        protected abstract TResult GetResult();

        public AwaitableTask ContinueWith(Action<AwaitableTask<TResult>> action, bool runSynchronously = false) 
            => ContinueWithInternal(this, action, runSynchronously);

        public AwaitableTask ContinueWith<TParam>(Action<AwaitableTask<TResult>, TParam> action, TParam param, bool runSynchronously = false) 
            => ContinueWithInternal(this, action, param, runSynchronously);

        public AwaitableTask<TFinalResult> ContinueWith<TFinalResult>(Func<AwaitableTask<TResult>, TFinalResult> func, bool runSynchronously = false) 
            => ContinueWithInternal(this, func, runSynchronously);

        public AwaitableTask<TFinalResult> ContinueWith<TParam, TFinalResult>(Func<AwaitableTask<TResult>, TParam, TFinalResult> func, TParam param, bool runSynchronously = false) 
            => ContinueWithInternal(this, func, param, runSynchronously);

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
