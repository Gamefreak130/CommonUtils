namespace Gamefreak130.Common.Tasks
{
    using Sims3.SimIFace;
    using System;

    public class RepeatingFunctionTask : CommonTask
    {
        private StopWatch mTimer;

        private readonly int mDelay;

        private readonly StopWatch.TickStyles mTickStyles;

        private readonly Func<bool> mFunction;

        public RepeatingFunctionTask(Func<bool> function, int delay = 500, StopWatch.TickStyles tickStyles = StopWatch.TickStyles.Milliseconds)
        {
            mFunction = function;
            mDelay = delay;
            mTickStyles = tickStyles;
        }

        public override void Dispose()
        {
            mTimer?.Dispose();
            mTimer = null;
            base.Dispose();
        }

        protected override void Run()
        {
            mTimer = StopWatch.Create(mTickStyles);
            mTimer.Start();
            do
            {
                mTimer.Restart();
                while (mTimer?.GetElapsedTime() < mDelay)
                {
                    if (Simulator.CheckYieldingContext(false))
                    {
                        Simulator.Sleep(0u);
                    }
                }
                if (!mFunction())
                {
                    break;
                }
                if (Simulator.CheckYieldingContext(false))
                {
                    Simulator.Sleep(0u);
                }
            }
            while (mTimer is not null);
        }
    }
}
