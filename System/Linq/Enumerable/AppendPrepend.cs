namespace System.Linq
{
    using System.Collections.Generic;

    // TEST
    public static partial class Enumerable
    {
        /// <summary>
        /// Appends a value to the end of a sequence
        /// </summary>

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource element)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            foreach (var item in source)
                yield return item;

            yield return element;
        }

        /// <summary>
        /// Adds a value to the beginning of a sequence
        /// </summary>

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource element)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            yield return element;

            foreach (var item in source)
                yield return item;
        }
    }
}
