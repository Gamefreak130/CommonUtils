namespace Gamefreak130.Common.Tasks
{
    using System;

    public static partial class TaskEx
    {
        public static AwaitableTask CompletedTask
        {
            get
            {
                TaskCompletionSource source = new();
                source.SetResult();
                return source.Task;
            }
        }

        public static AwaitableTask<TResult> FromResult<TResult>(TResult result)
        {
            TaskCompletionSource<TResult> source = new();
            source.SetResult(result);
            return source.Task;
        }

        public static AwaitableTask FromCanceled()
        {
            TaskCompletionSource source = new();
            source.SetCanceled();
            return source.Task;
        }

        public static AwaitableTask<TResult> FromCanceled<TResult>()
        {
            TaskCompletionSource<TResult> source = new();
            source.SetCanceled();
            return source.Task;
        }

        public static AwaitableTask FromException(Exception exception)
        {
            TaskCompletionSource source = new();
            source.SetException(exception);
            return source.Task;
        }

        public static AwaitableTask<TResult> FromException<TResult>(Exception exception)
        {
            TaskCompletionSource<TResult> source = new();
            source.SetException(exception);
            return source.Task;
        }
    }
}
