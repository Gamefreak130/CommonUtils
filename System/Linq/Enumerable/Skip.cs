namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Bypasses elements in a sequence as long as a specified condition 
        /// is true and then returns the remaining elements.
        /// </summary>

        public static IEnumerable<TSource> SkipWhile<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.SkipWhile((item, i) => predicate(item));
        }

        /// <summary>
        /// Bypasses elements in a sequence as long as a specified condition 
        /// is true and then returns the remaining elements. The element's 
        /// index is used in the logic of the predicate function.
        /// </summary>

        public static IEnumerable<TSource> SkipWhile<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return SkipWhileYield(source, predicate);
        }

        private static IEnumerable<TSource> SkipWhileYield<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            using (var e = source.GetEnumerator())
            {
                for (var i = 0; ; i++)
                {
                    if (!e.MoveNext())
                        yield break;

                    if (!predicate(e.Current, i))
                        break;
                }

                do
                { yield return e.Current; } while (e.MoveNext());
            }
        }

        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then 
        /// returns the remaining elements.
        /// </summary>

        public static IEnumerable<TSource> Skip<TSource>(
            this IEnumerable<TSource> source,
            int count)
        {
            return source is List<TSource> list 
                ? list.SkipYield(count) 
                : source.SkipWhile((item, i) => i < count);
        }

        private static IEnumerable<TSource> SkipYield<TSource>(
            this List<TSource> source,
            int count)
        {
            for (int i = count; i < source.Count; i++)
            {
                yield return source[i];
            }
        }
    }
}
