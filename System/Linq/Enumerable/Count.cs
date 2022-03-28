namespace System.Linq
{
    using System.Collections;
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>

        public static int Count<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var collection = source as ICollection;
            return collection != null
                 ? collection.Count
                 : source.Aggregate(0, (count, item) => checked(count + 1));
        }

        /// <summary>
        /// Returns a number that represents how many elements in the 
        /// specified sequence satisfy a condition.
        /// </summary>

        public static int Count<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return Count(source.Where(predicate));
        }

        /// <summary>
        /// Returns an <see cref="Int64"/> that represents the total number 
        /// of elements in a sequence.
        /// </summary>

        public static long LongCount<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var array = source as Array;
            return array != null
                 ? array.LongLength
                 : source.Aggregate(0L, (count, item) => count + 1);
        }

        /// <summary>
        /// Returns an <see cref="Int64"/> that represents how many elements 
        /// in a sequence satisfy a condition.
        /// </summary>

        public static long LongCount<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return LongCount(source.Where(predicate));
        }
    }
}
