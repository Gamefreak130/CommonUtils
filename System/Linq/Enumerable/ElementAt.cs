namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Returns the element at a specified index in a sequence.
        /// </summary>

        public static TSource ElementAt<TSource>(
            this IEnumerable<TSource> source,
            int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, null);

            var list = source as IList<TSource>;
            if (list != null)
                return list[index];

            try
            {
                return source.SkipWhile((item, i) => i < index).First();
            }
            catch (InvalidOperationException) // if thrown by First
            {
                throw new ArgumentOutOfRangeException("index", index, null);
            }
        }

        /// <summary>
        /// Returns the element at a specified index in a sequence or a 
        /// default value if the index is out of range.
        /// </summary>

        public static TSource ElementAtOrDefault<TSource>(
            this IEnumerable<TSource> source,
            int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (index < 0)
                return default(TSource);

            var list = source as IList<TSource>;
            if (list != null)
                return index < list.Count ? list[index] : default(TSource);

            return source.SkipWhile((item, i) => i < index).FirstOrDefault();
        }
    }
}
