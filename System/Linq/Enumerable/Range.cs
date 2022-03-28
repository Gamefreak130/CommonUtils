namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Generates a sequence of integral numbers within a specified range.
        /// </summary>
        /// <param name="start">The value of the first integer in the sequence.</param>
        /// <param name="count">The number of sequential integers to generate.</param>

        public static IEnumerable<int> Range(int start, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, null);

            var end = (long)start + count;
            if (end - 1 >= int.MaxValue)
                throw new ArgumentOutOfRangeException("count", count, null);

            return RangeYield(start, end);
        }

        private static IEnumerable<int> RangeYield(int start, long end)
        {
            for (var i = start; i < end; i++)
                yield return i;
        }
    }
}
