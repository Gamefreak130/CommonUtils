namespace System.Linq
{
    using System.Collections.Generic;

    // CONSIDER IntersectBy, ExceptBy
    public static partial class Enumerable
    {
        /// <summary>
        /// Base implementation for Intersect and Except operators.
        /// </summary>

        private static IEnumerable<TSource> IntersectExceptImpl<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer,
            bool flag)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            var keys = new List<Key<TSource>>();
            var flags = new Dictionary<Key<TSource>, bool>(new KeyComparer<TSource>(comparer));

            foreach (var item in from item in first
                                 select new Key<TSource>(item) into item
                                 where !flags.ContainsKey(item)
                                 select item)
            {
                flags.Add(item, !flag);
                keys.Add(item);
            }

            foreach (var item in from item in second
                                 select new Key<TSource>(item) into item
                                 where flags.ContainsKey(item)
                                 select item)
            {
                flags[item] = flag;
            }

            //
            // As per docs, "the marked elements are yielded in the order in 
            // which they were collected.
            //

            return from item in keys where flags[item] select item.Value;
        }

        /// <summary>
        /// Produces the set intersection of two sequences by using the 
        /// default equality comparer to compare values.
        /// </summary>

        public static IEnumerable<TSource> Intersect<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return first.Intersect(second, /* comparer */ null);
        }

        /// <summary>
        /// Produces the set intersection of two sequences by using the 
        /// specified <see cref="IEqualityComparer{T}" /> to compare values.
        /// </summary>

        public static IEnumerable<TSource> Intersect<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            return IntersectExceptImpl(first, second, comparer, /* flag */ true);
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the 
        /// default equality comparer to compare values.
        /// </summary>

        public static IEnumerable<TSource> Except<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return first.Except(second, /* comparer */ null);
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the 
        /// specified <see cref="IEqualityComparer{T}" /> to compare values.
        /// </summary>

        public static IEnumerable<TSource> Except<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            return IntersectExceptImpl(first, second, comparer, /* flag */ false);
        }
    }
}
