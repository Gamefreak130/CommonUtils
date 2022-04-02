namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Base implementation of Single operator.
        /// </summary>

        private static TSource SingleImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource> empty)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    var single = e.Current;
                    if (!e.MoveNext())
                        return single;

                    throw new InvalidOperationException();
                }

                return empty();
            }
        }

        /// <summary>
        /// Returns the only element of a sequence, and throws an exception 
        /// if there is not exactly one element in the sequence.
        /// </summary>

        public static TSource Single<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.SingleImpl(Futures<TSource>.Undefined);
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a 
        /// specified condition, and throws an exception if more than one 
        /// such element exists.
        /// </summary>

        public static TSource Single<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return Single(source.Where(predicate));
        }

        /// <summary>
        /// Returns the only element of a sequence, or a default value if 
        /// the sequence is empty; this method throws an exception if there 
        /// is more than one element in the sequence.
        /// </summary>

        public static TSource SingleOrDefault<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.SingleImpl(Futures<TSource>.Default);
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a 
        /// specified condition or a default value if no such element 
        /// exists; this method throws an exception if more than one element 
        /// satisfies the condition.
        /// </summary>

        public static TSource SingleOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return SingleOrDefault(source.Where(predicate));
        }

        /// <summary>
        /// Returns the only element of a sequence, or a specified default 
        /// value if the sequence is empty; this method throws an exception if 
        /// there is more than one element in the sequence.
        /// </summary>

        public static TSource SingleOrDefault<TSource>(
            this IEnumerable<TSource> source,
            TSource defaultValue)
        {
            return source.SingleImpl(() => defaultValue);
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a 
        /// specified condition, or a specified default value if no such 
        /// element exists; this method throws an exception if more 
        /// than one element satisfies the condition.
        /// </summary>

        public static TSource SingleOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            TSource defaultValue)
        {
            return SingleOrDefault(source.Where(predicate), defaultValue);
        }
    }
}
