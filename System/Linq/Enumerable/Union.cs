namespace System.Linq
{
    using System.Collections.Generic;

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

        /// <summary>Produces the set union of two sequences according to a specified key selector function.</summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
        /// <param name="first">An <see cref="IEnumerable{T}" /> whose distinct elements form the first set for the union.</param>
        /// <param name="second">An <see cref="IEnumerable{T}" /> whose distinct elements form the second set for the union.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains the elements from both input sequences, excluding duplicates.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
        /// <remarks>
        /// <para>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its `GetEnumerator` method directly or by using `foreach` in Visual C# or `For Each` in Visual Basic.</para>
        /// <para>The default equality comparer, <see cref="EqualityComparer{T}.Default" />, is used to compare values.</para>
        /// <para>When the object returned by this method is enumerated, <see cref="O:Enumerable.UnionBy" /> enumerates <paramref name="first" /> and <paramref name="second" /> in that order and yields each element that has not already been yielded.</para>
        /// </remarks>
        public static IEnumerable<TSource> UnionBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector) => UnionBy(first, second, keySelector, null);

        /// <summary>Produces the set union of two sequences according to a specified key selector function.</summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
        /// <param name="first">An <see cref="IEnumerable{T}" /> whose distinct elements form the first set for the union.</param>
        /// <param name="second">An <see cref="IEnumerable{T}" /> whose distinct elements form the second set for the union.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}" /> to compare values.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains the elements from both input sequences, excluding duplicates.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
        /// <remarks>
        /// <para>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its `GetEnumerator` method directly or by using `foreach` in Visual C# or `For Each` in Visual Basic.</para>
        /// <para>If <paramref name="comparer" /> is <see langword="null" />, the default equality comparer, <see cref="EqualityComparer{T}.Default" />, is used to compare values.</para>
        /// <para>When the object returned by this method is enumerated, <see cref="O:Enumerable.UnionBy" /> enumerates <paramref name="first" /> and <paramref name="second" /> in that order and yields each element that has not already been yielded.</para>
        /// </remarks>
        
        public static IEnumerable<TSource> UnionBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return first.Concat(second).DistinctBy(keySelector, comparer);

            /*if (first is null)
            {
                throw new ArgumentException("first");
            }
            if (second is null)
            {
                throw new ArgumentException("second");
            }
            if (keySelector is null)
            {
                throw new ArgumentException("keySelector");
            }
            
            var set = new HashSet<TKey>(comparer);

            foreach (TSource element in first)
            {
                if (set.Add(keySelector(element)))
                {
                    yield return element;
                }
            }

            foreach (TSource element in second)
            {
                if (set.Add(keySelector(element)))
                {
                    yield return element;
                }
            }*/
        }
    }
}
