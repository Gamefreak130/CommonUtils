namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Groups the elements of a sequence according to a specified key 
        /// selector function.
        /// </summary>

        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return GroupBy(source, keySelector, /* comparer */ null);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key 
        /// selector function and compares the keys by using a specified 
        /// comparer.
        /// </summary>

        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            return GroupBy(source, keySelector, e => e, comparer);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key 
        /// selector function and projects the elements for each group by 
        /// using a specified function.
        /// </summary>

        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            return GroupBy(source, keySelector, elementSelector, /* comparer */ null);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key 
        /// selector function and creates a result value from each group and 
        /// its key.
        /// </summary>

        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
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

            return ToLookup(source, keySelector, elementSelector, comparer);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a key selector 
        /// function. The keys are compared by using a comparer and each 
        /// group's elements are projected by using a specified function.
        /// </summary>

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
        {
            return GroupBy(source, keySelector, resultSelector, /* comparer */ null);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key 
        /// selector function and creates a result value from each group and 
        /// its key. The elements of each group are projected by using a 
        /// specified function.
        /// </summary>

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, IEnumerable<TSource>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return ToLookup(source, keySelector, comparer).Select(g => resultSelector(g.Key, g));
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key 
        /// selector function and creates a result value from each group and 
        /// its key. The keys are compared by using a specified comparer.
        /// </summary>

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            return GroupBy(source, keySelector, elementSelector, resultSelector, /* comparer */ null);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key 
        /// selector function and creates a result value from each group and 
        /// its key. Key values are compared by using a specified comparer, 
        /// and the elements of each group are projected by using a 
        /// specified function.
        /// </summary>

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return ToLookup(source, keySelector, elementSelector, comparer)
                   .Select(g => resultSelector(g.Key, g));
        }

        /// <summary>
        /// Correlates the elements of two sequences based on equality of 
        /// keys and groups the results. The default equality comparer is 
        /// used to compare keys.
        /// </summary>

        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
        {
            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, /* comparer */ null);
        }

        /// <summary>
        /// Correlates the elements of two sequences based on equality of 
        /// keys and groups the results. The default equality comparer is 
        /// used to compare keys. A specified <see cref="IEqualityComparer{T}" /> 
        /// is used to compare keys.
        /// </summary>

        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector,
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
            return outer.Select(o => resultSelector(o, lookup[outerKeySelector(o)]));
        }
    }
}
