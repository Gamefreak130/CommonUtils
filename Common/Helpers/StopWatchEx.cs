namespace Gamefreak130.Common.Helpers
{
    using Sims3.SimIFace;

    public static class StopWatchEx
    {
        public static StopWatch StartNew(StopWatch.TickStyles tickStyles)
        {
            StopWatch stopWatch = StopWatch.Create(tickStyles);
            stopWatch.Start();
            return stopWatch;
        }
    }
}
