namespace Gamefreak130.Common.Tasks
{
    using Sims3.SimIFace;
    using System;

    // TEST
    public class RepeatingFunctionTask : CommonTask
    {
        private readonly uint mDelay;

        private readonly StopWatch.TickStyles mTickStyles;

        private readonly Func<bool> mFunction;

        private AwaitableTask mDelayTask;

        public RepeatingFunctionTask(Func<bool> function, uint delay = 500, StopWatch.TickStyles tickStyles = StopWatch.TickStyles.Milliseconds)
        {
            mFunction = function;
            mDelay = delay;
            mTickStyles = tickStyles;
        }

        public override void Dispose()
        {
            mDelayTask.Cancel();
            base.Dispose();
        }

        protected override void Perform()
        {
            while (true)
            {
                mDelayTask = TaskEx.Delay(mDelay, mTickStyles);
                mDelayTask.Await();
                if (!mFunction())
                {
                    return;
                }
            }
        }

        public static RepeatingFunctionTask Run(Func<bool> function, uint delay = 500, StopWatch.TickStyles tickStyles = StopWatch.TickStyles.Milliseconds)
        {
            RepeatingFunctionTask task = new(function, delay, tickStyles);
            task.Start();
            return task;
        }
    }
}
