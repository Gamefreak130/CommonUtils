namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.OrderBy(keySelector, /* comparer */ null);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a 
        /// specified comparer.
        /// </summary>

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, /* descending */ false);
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.OrderByDescending(keySelector, /* comparer */ null);
        }

        /// <summary>
        ///  Sorts the elements of a sequence in descending order by using a 
        /// specified comparer. 
        /// </summary>

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (source == null)
                throw new ArgumentNullException("keySelector");

            return new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, /* descending */ true);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in 
        /// ascending order according to a key.
        /// </summary>

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.ThenBy(keySelector, /* comparer */ null);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in 
        /// ascending order by using a specified comparer.
        /// </summary>

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.CreateOrderedEnumerable(keySelector, comparer, /* descending */ false);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in 
        /// descending order, according to a key.
        /// </summary>

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.ThenByDescending(keySelector, /* comparer */ null);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in 
        /// descending order by using a specified comparer.
        /// </summary>

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.CreateOrderedEnumerable(keySelector, comparer, /* descending */ true);
        }
    }
}
