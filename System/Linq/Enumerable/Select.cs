namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>

        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select((item, i) => selector(item));
        }

        /// <summary>
        /// Projects each element of a sequence into a new form by 
        /// incorporating the element's index.
        /// </summary>

        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return SelectYield(source, selector);
        }

        private static IEnumerable<TResult> SelectYield<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, int, TResult> selector)
        {
            var i = 0;
            foreach (var item in source)
                yield return selector(item, i++);
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}" /> 
        /// and flattens the resulting sequences into one sequence.
        /// </summary>

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.SelectMany((item, i) => selector(item));
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}" />, 
        /// and flattens the resulting sequences into one sequence. The 
        /// index of each source element is used in the projected form of 
        /// that element.
        /// </summary>

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, IEnumerable<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.SelectMany(selector, (item, subitem) => subitem);
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}" />, 
        /// flattens the resulting sequences into one sequence, and invokes 
        /// a result selector function on each element therein.
        /// </summary>

        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            if (collectionSelector == null)
                throw new ArgumentNullException("collectionSelector");

            return source.SelectMany((item, i) => collectionSelector(item), resultSelector);
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}" />, 
        /// flattens the resulting sequences into one sequence, and invokes 
        /// a result selector function on each element therein. The index of 
        /// each source element is used in the intermediate projected form 
        /// of that element.
        /// </summary>

        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (collectionSelector == null)
                throw new ArgumentNullException("collectionSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return SelectManyYield(source, collectionSelector, resultSelector);
        }

        private static IEnumerable<TResult> SelectManyYield<TSource, TCollection, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            var i = 0;
            foreach (var item in source)
                foreach (var subitem in collectionSelector(item, i++))
                    yield return resultSelector(item, subitem);
        }
    }
}
