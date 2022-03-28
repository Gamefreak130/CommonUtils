namespace System.Linq
{
    using System.Collections.Generic;

    // TODO Take w/ C# 8 range, TakeLast
    public static partial class Enumerable
    {
        /// <summary>
        /// Returns elements from a sequence as long as a specified condition is true.
        /// </summary>

        public static IEnumerable<TSource> TakeWhile<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.TakeWhile((item, i) => predicate(item));
        }

        /// <summary>
        /// Returns elements from a sequence as long as a specified condition is true.
        /// The element's index is used in the logic of the predicate function.
        /// </summary>

        public static IEnumerable<TSource> TakeWhile<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return TakeWhileYield(source, predicate);
        }

        private static IEnumerable<TSource> TakeWhileYield<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            var i = 0;
            foreach (var item in source)
                if (predicate(item, i++))
                    yield return item;
                else
                    break;
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start 
        /// of a sequence.
        /// </summary>

        public static IEnumerable<TSource> Take<TSource>(
            this IEnumerable<TSource> source,
            int count)
        {
            return source.TakeWhile((item, i) => i < count);
        }
    }
}
