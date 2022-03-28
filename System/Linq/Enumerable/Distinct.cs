namespace System.Linq
{
    using System.Collections.Generic;

    // CONSIDER DistinctBy
    public static partial class Enumerable
    {
        /// <summary>
        /// Returns distinct elements from a sequence by using the default 
        /// equality comparer to compare values.
        /// </summary>

        public static IEnumerable<TSource> Distinct<TSource>(
            this IEnumerable<TSource> source)
        {
            return Distinct(source, /* comparer */ null);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified 
        /// <see cref="IEqualityComparer{T}"/> to compare values.
        /// </summary>

        public static IEnumerable<TSource> Distinct<TSource>(
            this IEnumerable<TSource> source,
            IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return DistinctYield(source, comparer);
        }

        private static IEnumerable<TSource> DistinctYield<TSource>(
            IEnumerable<TSource> source,
            IEqualityComparer<TSource> comparer)
        {
            var set = new Dictionary<TSource, object>(comparer);
            var gotNull = false;

            foreach (var item in source)
            {
                if (item == null)
                {
                    if (gotNull)
                        continue;
                    gotNull = true;
                }
                else
                {
                    if (set.ContainsKey(item))
                        continue;
                    set.Add(item, null);
                }

                yield return item;
            }
        }
    }
}
