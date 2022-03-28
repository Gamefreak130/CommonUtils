namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Correlates the elements of two sequences based on matching keys. 
        /// The default equality comparer is used to compare keys.
        /// </summary>

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, /* comparer */ null);
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys. 
        /// The default equality comparer is used to compare keys. A 
        /// specified <see cref="IEqualityComparer{T}" /> is used to compare keys.
        /// </summary>

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException("outer");
            if (inner == null)
                throw new ArgumentNullException("inner");
            if (outerKeySelector == null)
                throw new ArgumentNullException("outerKeySelector");
            if (innerKeySelector == null)
                throw new ArgumentNullException("innerKeySelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            var lookup = inner.ToLookup(innerKeySelector, comparer);

            return
                from o in outer
                from i in lookup[outerKeySelector(o)]
                select resultSelector(o, i);
        }
    }
}
