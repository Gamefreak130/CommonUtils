namespace Gamefreak130.Common.Tasks
{
    using Sims3.SimIFace;
    using System;

    public abstract partial class AwaitableTask : IDisposable
    {
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
                        AwaitableTask<AwaitableTask> taskOfTask => taskOfTask.Await(),
                        AwaitableTask<AwaitableTask<TResult>> taskOfTaskOfTResult => taskOfTaskOfTResult.Await(),
                        _ => throw new Exception("Outer task must be of type AwaitableTask<AwaitableTask> or AwaitableTask<AwaitableTask<TResult>>")
                    };

                    ProcessInnerTask(innerTask);
                }
            }

            private void ProcessInnerTask(AwaitableTask innerTask)
            {
                if (innerTask is null)
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

        internal static AwaitableTask Unwrap(AwaitableTask<AwaitableTask> task)
            => task.IsCompletedSuccessfully
            ? task.Await() ?? TaskEx.FromCanceled()
            : new UnwrapPromise<object>(task);

        internal static AwaitableTask<TResult> Unwrap<TResult>(AwaitableTask<AwaitableTask<TResult>> task)
            => task.IsCompletedSuccessfully
            ? task.Await() ?? TaskEx.FromCanceled<TResult>()
            : new UnwrapPromise<TResult>(task);
    }

    public static partial class TaskEx
    {
        public static AwaitableTask Unwrap(this AwaitableTask<AwaitableTask> task)
            => AwaitableTask.Unwrap(task);

        public static AwaitableTask<TResult> Unwrap<TResult>(this AwaitableTask<AwaitableTask<TResult>> task)
            => AwaitableTask.Unwrap(task);
    }
}
