namespace Gamefreak130.Common.Tasks
{
    using Sims3.SimIFace;

    public static partial class TaskEx
    {
        public static void Yield(bool shouldThrowException = false)
            => Yield(0, shouldThrowException);

        public static void Yield(uint delayTicks, bool shouldThrowException = false)
        {
            if (Simulator.CheckYieldingContext(shouldThrowException))
            {
                Simulator.Sleep(delayTicks);
            }
        }
    }
}
