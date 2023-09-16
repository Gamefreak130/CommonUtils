namespace Gamefreak130.Common.Tasks
{
    using System;
    using System.Collections.Generic;

    // TEST this lmao
    public class TaskCompletionSource
    {
        private class NopTask : AwaitableTask
        {
            public NopTask() : base(true)
                => Status = AwaitableTaskStatus.Running;

            public void SetCanceled() => Status = AwaitableTaskStatus.Canceled;

            public void SetException(Exception ex) => AddException(ex);

            public void SetException(IEnumerable<Exception> exceptions)
            {
                foreach (Exception ex in exceptions)
                {
                    AddException(ex);
                }
            }

            public void SetResult() => Status = AwaitableTaskStatus.RanToCompletion;

            protected override void Perform() => throw new NotSupportedException();
        }

        internal const string kAlreadyCompletedMessage = "Task is already completed";

        private readonly NopTask mTask = new();

        public AwaitableTask Task => mTask;

        public void SetCanceled()
        {
            if (!TrySetCanceled())
            {
                throw new InvalidOperationException(kAlreadyCompletedMessage);
            }
        }

        public bool TrySetCanceled()
        {
            if (mTask.IsCompleted)
            {
                return false;
            }
            mTask.SetCanceled();
            return true;
        }

        public void SetException(Exception ex)
        {
            if (!TrySetException(ex))
            {
                throw new InvalidOperationException(kAlreadyCompletedMessage);
            }
        }

        public bool TrySetException(Exception ex)
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }
            if (mTask.IsCompleted)
            {
                return false;
            }
            mTask.SetException(ex);
            return true;
        }

        public void SetException(IEnumerable<Exception> exceptions)
        {
            if (!TrySetException(exceptions))
            {
                throw new InvalidOperationException(kAlreadyCompletedMessage);
            }
        }

        public bool TrySetException(IEnumerable<Exception> exceptions)
        {
            if (exceptions is null)
            {
                throw new ArgumentNullException(nameof(exceptions));
            }
            if (mTask.IsCompleted)
            {
                return false;
            }
            mTask.SetException(exceptions);
            return true;
        }

        public void SetResult()
        {
            if (!TrySetResult())
            {
                throw new InvalidOperationException(kAlreadyCompletedMessage);
            }
        }

        public bool TrySetResult()
        {
            if (mTask.IsCompleted)
            {
                return false;
            }
            mTask.SetResult();
            return true;
        }
    }

    public class TaskCompletionSource<TResult>
    {
        private class NopTask : AwaitableTask<TResult>
        {
            public NopTask() : base(true)
                => Status = AwaitableTaskStatus.Running;

            public void SetCanceled() => Status = AwaitableTaskStatus.Canceled;

            public void SetException(Exception ex) => AddException(ex);

            public void SetException(IEnumerable<Exception> exceptions)
            {
                foreach (Exception ex in exceptions)
                {
                    AddException(ex);
                }
            }

            new public bool TrySetResult(TResult result)
            {
                bool flag = base.TrySetResult(result);
                Status = AwaitableTaskStatus.RanToCompletion;
                return flag;
            }

            protected override TResult GetResult() => throw new NotSupportedException();
        }

        private readonly NopTask mTask = new();

        public AwaitableTask<TResult> Task => mTask;

        public void SetCanceled()
        {
            if (!TrySetCanceled())
            {
                throw new InvalidOperationException(TaskCompletionSource.kAlreadyCompletedMessage);
            }
        }

        public bool TrySetCanceled()
        {
            if (mTask.IsCompleted)
            {
                return false;
            }
            mTask.SetCanceled();
            return true;
        }

        public void SetException(Exception ex)
        {
            if (!TrySetException(ex))
            {
                throw new InvalidOperationException(TaskCompletionSource.kAlreadyCompletedMessage);
            }
        }

        public bool TrySetException(Exception ex)
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }
            if (mTask.IsCompleted)
            {
                return false;
            }
            mTask.SetException(ex);
            return true;
        }

        public void SetException(IEnumerable<Exception> exceptions)
        {
            if (!TrySetException(exceptions))
            {
                throw new InvalidOperationException(TaskCompletionSource.kAlreadyCompletedMessage);
            }
        }

        public bool TrySetException(IEnumerable<Exception> exceptions)
        {
            if (exceptions is null)
            {
                throw new ArgumentNullException(nameof(exceptions));
            }
            if (mTask.IsCompleted)
            {
                return false;
            }
            mTask.SetException(exceptions);
            return true;
        }

        public void SetResult(TResult result)
        {
            if (!TrySetResult(result))
            {
                throw new InvalidOperationException(TaskCompletionSource.kAlreadyCompletedMessage);
            }
        }

        public bool TrySetResult(TResult result) => mTask.TrySetResult(result);
    }
}
