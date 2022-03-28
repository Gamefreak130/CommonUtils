namespace System.Linq
{
    using System.Collections;
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable"/> to the 
        /// specified type.
        /// </summary>

        public static IEnumerable<TResult> Cast<TResult>(
            this IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return CastYield<TResult>(source);
        }

        private static IEnumerable<TResult> CastYield<TResult>(
            IEnumerable source)
        {
            foreach (var item in source)
                yield return (TResult)item;
        }

        /// <summary>
        /// Filters the elements of an <see cref="IEnumerable"/> based on a specified type.
        /// </summary>

        public static IEnumerable<TResult> OfType<TResult>(
            this IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OfTypeYield<TResult>(source);
        }

        private static IEnumerable<TResult> OfTypeYield<TResult>(
            IEnumerable source)
        {
            foreach (var item in source)
                if (item is TResult)
                    yield return (TResult)item;
        }
    }
}
