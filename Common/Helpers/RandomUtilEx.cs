namespace Gamefreak130.Common.Helpers
{
    using Sims3.Gameplay.Core;

    public static class RandomUtilEx
    {
        public static T CoinFlipSelect<T>(T obj1, T obj2) => RandomUtil.CoinFlip() ? obj1 : obj2;
    }
}
