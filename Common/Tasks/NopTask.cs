namespace Gamefreak130.Common.Tasks
{
    using System;

    public static partial class TaskEx
    {
        private class NopTask : AwaitableTask
        {
            public NopTask(bool complete) : base(true)
                => Status = complete ? AwaitableTaskStatus.RanToCompletion : AwaitableTaskStatus.Canceled;

            public NopTask(Exception exception) : base(true)
            {
                AddException(exception);
                Status = AwaitableTaskStatus.Faulted;
            }

            protected override void Perform() => throw new NotSupportedException();
        }

        private class NopTask<TResult> : AwaitableTask<TResult>
        {
            public NopTask() : base(true)
                => Status = AwaitableTaskStatus.Canceled;

            public NopTask(TResult result) : base(result)
                => Status = AwaitableTaskStatus.RanToCompletion;

            public NopTask(Exception exception) : base(true)
            {
                AddException(exception);
                Status = AwaitableTaskStatus.Faulted;
            }

            protected override TResult GetResult() => throw new NotSupportedException();
        }

        public static AwaitableTask CompletedTask => new NopTask(true);

        public static AwaitableTask<TResult> FromResult<TResult>(TResult result)
            => new NopTask<TResult>(result);

        public static AwaitableTask FromCanceled()
            => new NopTask(false);

        public static AwaitableTask<TResult> FromCanceled<TResult>()
            => new NopTask<TResult>();

        public static AwaitableTask FromException(Exception exception)
            => new NopTask(exception);

        public static AwaitableTask<TResult> FromException<TResult>(Exception exception)
            => new NopTask<TResult>(exception);
    }
}
