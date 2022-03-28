namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Determines whether all elements of a sequence satisfy a condition.
        /// </summary>

        public static bool All<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            foreach (var item in source)
                if (!predicate(item))
                    return false;

            return true;
        }

        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>

        public static bool Any<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            using (var e = source.GetEnumerator())
                return e.MoveNext();
        }

        /// <summary>
        /// Determines whether any element of a sequence satisfies a 
        /// condition.
        /// </summary>

        public static bool Any<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return source.Where(predicate).Any();
        }
    }
}
