namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Returns the elements of the specified sequence or the type 
        /// parameter's default value in a singleton collection if the 
        /// sequence is empty.
        /// </summary>

        public static IEnumerable<TSource> DefaultIfEmpty<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.DefaultIfEmpty(default(TSource));
        }

        /// <summary>
        /// Returns the elements of the specified sequence or the specified 
        /// value in a singleton collection if the sequence is empty.
        /// </summary>

        public static IEnumerable<TSource> DefaultIfEmpty<TSource>(
            this IEnumerable<TSource> source,
            TSource defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return DefaultIfEmptyYield(source, defaultValue);
        }

        private static IEnumerable<TSource> DefaultIfEmptyYield<TSource>(
            IEnumerable<TSource> source,
            TSource defaultValue)
        {
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                    yield return defaultValue;
                else
                    do
                    { yield return e.Current; } while (e.MoveNext());
            }
        }
    }
}
