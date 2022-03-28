namespace System.Linq
{
    using System.Collections.Generic;

    // CONSIDER UnionBy
    public static partial class Enumerable
    {
        /// <summary>
        /// Produces the set union of two sequences by using the default 
        /// equality comparer.
        /// </summary>

        public static IEnumerable<TSource> Union<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return Union(first, second, /* comparer */ null);
        }

        /// <summary>
        /// Produces the set union of two sequences by using a specified 
        /// <see cref="IEqualityComparer{T}" />.
        /// </summary>

        public static IEnumerable<TSource> Union<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            return first.Concat(second).Distinct(comparer);
        }
    }
}
