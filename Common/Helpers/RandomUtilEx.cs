namespace Gamefreak130.Common.Helpers
{
    using Sims3.Gameplay.Core;
    using Sims3.Gameplay.Interfaces;
    using System;

    public static class RandomUtilEx
    {
        public static T CoinFlipSelect<T>(T obj1, T obj2) => RandomUtil.CoinFlip() ? obj1 : obj2;

        public static double GetDouble(double maxValue, Random randomGen) => GetDouble(0.0, maxValue, randomGen);

        public static double GetDouble(double minValue, double maxValue, Random randomGen) 
            => minValue + ((maxValue - minValue) * randomGen.NextDouble());

        public static float GetFloat(float maxValue, Random randomGen) => GetFloat(0f, maxValue, randomGen);

        public static float GetFloat(float minValue, float maxValue, Random randomGen) 
            => minValue + (float)((maxValue - minValue) * randomGen.NextDouble());

        public static IWeightable GetWeightedRandomObjectFromList(IWeightable[] randomList, Random randomGen)
        {
            if (randomList.Length == 0)
            {
                throw new ArgumentException("The list is empty.");
            }
            if (randomList.Length == 1)
            {
                return randomList[0];
            }
            float totalWeight = 0f;
            foreach (IWeightable weightable in randomList)
            {
                totalWeight += weightable.Weight;
            }
            float @float = GetFloat(totalWeight, randomGen);
            float num = 0f;
            foreach (IWeightable weightable in randomList)
            {
                num += weightable.Weight;
                if (@float <= num)
                {
                    return weightable;
                }
            }
            return randomList[0];
        }

        public static ResultType GetWeightedRandomObjectFromList<ResultType>(float[] chances, ResultType[] results, Random randomGen) 
            => GetWeightedRandomObjectFromList(chances, results, randomGen, out _);

        public static ResultType GetWeightedRandomObjectFromList<ResultType>(float[] chances, ResultType[] results, Random randomGen, out float chosenWeight)
        {
            int length = chances.Length;
            if (length != results.Length)
            {
                throw new ArgumentException("The lists are unequal in length.");
            }
            if (length == 0)
            {
                throw new ArgumentException("The list is empty.");
            }
            if (length == 1)
            {
                chosenWeight = 0f;
                return results[0];
            }
            float totalWeight = 0f;
            for (int i = 0; i < length; i++)
            {
                totalWeight += chances[i];
            }
            float @float = GetFloat(totalWeight, randomGen);
            float num = 0f;
            for (int j = 0; j < length; j++)
            {
                num += chances[j];
                if (@float <= num)
                {
                    chosenWeight = chances[j];
                    return results[j];
                }
            }
            chosenWeight = chances[0];
            return results[0];
        }

        public static bool RandomChance(float chance, Random randomGen) => GetFloat(0f, 100f, randomGen) < chance;

        public static bool RandomChance01(float chance, Random randomGen) => GetFloat(0f, 1f, randomGen) < chance;
    }
}
