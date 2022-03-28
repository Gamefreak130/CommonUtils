namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Concatenates two sequences.
        /// </summary>

        public static IEnumerable<TSource> Concat<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return ConcatYield(first, second);
        }

        private static IEnumerable<TSource> ConcatYield<TSource>(
            IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            foreach (var item in first)
                yield return item;

            foreach (var item in second)
                yield return item;
        }
    }
}
