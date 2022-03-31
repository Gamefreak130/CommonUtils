namespace Gamefreak130.Common.Tasks
{
    using Gamefreak130.Common.Helpers;
    using Sims3.SimIFace;

    public static partial class TaskEx
    {
        private class DelayTask : AwaitableTask
        {
            private StopWatch mTimer;

            private readonly uint mDelay;

            private readonly StopWatch.TickStyles mTickStyles;

            public DelayTask(uint delay, StopWatch.TickStyles tickStyles) : base(false)
            {
                mDelay = delay;
                mTickStyles = tickStyles;
                Start();
            }

            protected override void Dispose(bool fromSimulator)
            {
                mTimer?.Dispose();
                mTimer = null;
                base.Dispose(fromSimulator);
            }

            protected override void Perform()
            {
                mTimer = StopWatchEx.StartNew(mTickStyles);
                while (mTimer?.GetElapsedTime() < mDelay)
                {
                    Yield(true);
                }
            }
        }

        public static AwaitableTask Delay(uint delay, StopWatch.TickStyles tickStyles = StopWatch.TickStyles.Milliseconds)
        {
            DelayTask task = new(delay, tickStyles);
            return task;
        }
    }
}
