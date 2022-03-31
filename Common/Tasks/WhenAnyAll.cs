namespace Gamefreak130.Common.Tasks
{
    public static partial class TaskEx
    {
        public static AwaitableTask<AwaitableTask> WhenAny(params AwaitableTask[] tasks)
            => AwaitableTask.WhenAny(tasks);

        public static AwaitableTask<AwaitableTask<TResult>> WhenAny<TResult>(params AwaitableTask<TResult>[] tasks)
            => AwaitableTask.WhenAny(tasks);

        public static AwaitableTask WhenAll(params AwaitableTask[] tasks)
            => AwaitableTask.WhenAll(tasks);

        public static AwaitableTask<TResult[]> WhenAll<TResult>(params AwaitableTask<TResult>[] tasks)
            => AwaitableTask.WhenAll(tasks);
    }
}
