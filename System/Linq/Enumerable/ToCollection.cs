namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Creates a <see cref="List{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>

        public static List<TSource> ToList<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new List<TSource>(source);
        }

        /// <summary>
        /// Creates an array from an <see cref="IEnumerable{T}"/>.
        /// </summary>

        public static TSource[] ToArray<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.ToList().ToArray();
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to a specified key 
        /// selector function.
        /// </summary>

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.ToDictionary(keySelector, /* comparer */ null);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to a specified key 
        /// selector function and key comparer.
        /// </summary>

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            return source.ToDictionary(keySelector, e => e, comparer);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to specified key 
        /// selector and element selector functions.
        /// </summary>

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            return source.ToDictionary(keySelector, elementSelector, /* comparer */ null);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to a specified key 
        /// selector function, a comparer, and an element selector function.
        /// </summary>

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            var dict = new Dictionary<TKey, TElement>(comparer);

            foreach (var item in source)
            {
                //
                // ToDictionary is meant to throw ArgumentNullException if
                // keySelector produces a key that is null and 
                // Argument exception if keySelector produces duplicate keys 
                // for two elements. Incidentally, the doucmentation for
                // IDictionary<TKey, TValue>.Add says that the Add method
                // throws the same exceptions under the same circumstances
                // so we don't need to do any additional checking or work
                // here and let the Add implementation do all the heavy
                // lifting.
                //

                dict.Add(keySelector(item), elementSelector(item));
            }

            return dict;
        }
    }
}
