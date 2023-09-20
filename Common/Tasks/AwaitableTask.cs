namespace Gamefreak130.Common.Tasks
{
    using Gamefreak130.Common.Helpers;
    using Gamefreak130.Common.Loggers;
    using Sims3.SimIFace;
    using System;
    using System.Collections.Generic;
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

        private SimulatorWrapper mWrapper;

        private Queue<IContinuationTask> mContinuations;

        private Queue<ulong> mAwaitingTaskIds;

        private List<Exception> mInnerExceptions;

        private bool mExceptionObserved;

        private readonly bool mSynchronous;

        private AwaitableTaskStatus mStatus;

        public AwaitableTaskStatus Status 
        {
            get => mStatus;
            protected set
            {
                mStatus = value;
                if (IsCompleted)
                {
                    while (mAwaitingTaskIds?.Count > 0)
                    {
                        ulong taskId = mAwaitingTaskIds.Dequeue();
                        if (taskId != ObjectGuid.kInvalidObjectGuidValue)
                        {
                            Simulator.Wake(new(taskId));
                        }
                    }
                }
            }
        }

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

                return canceledException is not null ? new(canceledException) : null;
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
            catch (OperationCanceledException)
            {
                Status = AwaitableTaskStatus.Canceled;
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
            if (!IsCompleted)
            {
                mAwaitingTaskIds ??= new(1);
                mAwaitingTaskIds.Enqueue(Simulator.CurrentTask.Value);
                TaskEx.Yield(uint.MaxValue, true);
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
    }

    public abstract partial class AwaitableTask<TResult> : AwaitableTask
    {
        private TResult mResult;

        protected AwaitableTask(bool runSynchronously) : base(runSynchronously)
        {
        }

        protected sealed override void Perform() => TrySetResult(GetResult());

        protected abstract TResult GetResult();

        protected bool TrySetResult(TResult result)
        {
            if (IsCompleted)
            {
                return false;
            }
            mResult = result;
            return true;
        }

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
