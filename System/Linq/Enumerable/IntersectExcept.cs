namespace System.Linq
{
    using System.Collections.Generic;

    // TEST
    public static partial class Enumerable
    {
        /// <summary>
        /// Base implementation for Intersect and Except operators.
        /// </summary>

        /*private static IEnumerable<TSource> IntersectExceptImpl<TSource>(
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
        }*/

        /// <summary>
        /// Base implementation for Intersect and Except operators.
        /// </summary>

        private static IEnumerable<TSource> IntersectExceptImpl<TSource, TKey>(
            this IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer,
            bool flag)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (keySelector == null)
                throw new ArgumentException("keySelector");

            var keys = new List<KeyValuePair<Key<TKey>, TSource>>();
            var flags = new Dictionary<Key<TKey>, bool>(new KeyComparer<TKey>(comparer));

            foreach (var item in from item in first
                                 select new KeyValuePair<Key<TKey>, TSource>(new Key<TKey>(keySelector(item)), item) into item
                                 where !flags.ContainsKey(item.Key)
                                 select item)
            {
                flags.Add(item.Key, !flag);
                keys.Add(item);
            }

            foreach (var item in from item in second
                                 select new Key<TKey>(item) into item
                                 where flags.ContainsKey(item)
                                 select item)
            {
                flags[item] = flag;
            }

            //
            // As per docs, "the marked elements are yielded in the order in 
            // which they were collected.
            //

            return from item in keys where flags[item.Key] select item.Value;
        }

        /// <summary>
        /// Produces the set intersection of two sequences by using the 
        /// default equality comparer to compare values.
        /// </summary>

        public static IEnumerable<TSource> Intersect<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return first.Intersect(second, null);
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
            return IntersectExceptImpl(first, second, x => x, comparer, true);
        }

        /// <summary>Produces the set intersection of two sequences according to a specified key selector function.</summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
        /// <param name="first">An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in <paramref name="second" /> will be returned.</param>
        /// <param name="second">An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in the first sequence will be returned.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
        /// <remarks>
        /// <para>The intersection of two sets A and B is defined as the set that contains all the elements of A that also appear in B, but no other elements.</para>
        /// <para>When the object returned by this method is enumerated, `Intersect` yields distinct elements occurring in both sequences in the order in which they appear in <paramref name="first" />.</para>
        /// <para>The default equality comparer, <see cref="EqualityComparer{T}.Default" />, is used to compare values.</para>
        /// </remarks>
        public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector) => IntersectBy(first, second, keySelector, null);

        /// <summary>Produces the set intersection of two sequences according to a specified key selector function.</summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
        /// <param name="first">An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in <paramref name="second" /> will be returned.</param>
        /// <param name="second">An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in the first sequence will be returned.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{TKey}" /> to compare keys.</param>
        /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
        /// <remarks>
        /// <para>The intersection of two sets A and B is defined as the set that contains all the elements of A that also appear in B, but no other elements.</para>
        /// <para>When the object returned by this method is enumerated, `Intersect` yields distinct elements occurring in both sequences in the order in which they appear in <paramref name="first" />.</para>
        /// <para>If <paramref name="comparer" /> is <see langword="null" />, the default equality comparer, <see cref="EqualityComparer{T}.Default" />, is used to compare values.</para>
        /// </remarks>
        public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return IntersectExceptImpl(first, second, keySelector, comparer, true);
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the 
        /// default equality comparer to compare values.
        /// </summary>

        public static IEnumerable<TSource> Except<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return first.Except(second, null);
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
            return IntersectExceptImpl(first, second, x => x, comparer, false);
        }

        /// <summary>
        /// Produces the set difference of two sequences according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
        /// <param name="first">An <see cref="IEnumerable{TSource}" /> whose keys that are not also in <paramref name="second"/> will be returned.</param>
        /// <param name="second">An <see cref="IEnumerable{TKey}" /> whose keys that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
        public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector) => ExceptBy(first, second, keySelector, null);

        /// <summary>
        /// Produces the set difference of two sequences according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
        /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
        /// <param name="first">An <see cref="IEnumerable{TSource}" /> whose keys that are not also in <paramref name="second"/> will be returned.</param>
        /// <param name="second">An <see cref="IEnumerable{TKey}" /> whose keys that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}" /> to compare values.</param>
        /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
        public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return IntersectExceptImpl(first, second, keySelector, comparer, false);
        }
    }
}
