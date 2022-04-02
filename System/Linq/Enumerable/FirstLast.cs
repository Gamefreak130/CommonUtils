namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Base implementation of First operator.
        /// </summary>

        private static TSource FirstImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource> empty)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            //Debug.Assert(empty != null);

            var list = source as IList<TSource>;    // optimized case for lists
            if (list != null)
                return list.Count > 0 ? list[0] : empty();

            using (var e = source.GetEnumerator())  // fallback for enumeration
                return e.MoveNext() ? e.Current : empty();
        }

        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>

        public static TSource First<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.FirstImpl(Futures<TSource>.Undefined);
        }

        /// <summary>
        /// Returns the first element in a sequence that satisfies a specified condition.
        /// </summary>

        public static TSource First<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return First(source.Where(predicate));
        }

        /// <summary>
        /// Returns the first element of a sequence, or a default value if 
        /// the sequence contains no elements.
        /// </summary>

        public static TSource FirstOrDefault<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.FirstImpl(Futures<TSource>.Default);
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a 
        /// condition or a default value if no such element is found.
        /// </summary>

        public static TSource FirstOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return FirstOrDefault(source.Where(predicate));
        }

        /// <summary>
        /// Returns the first element of a sequence, or a default value if 
        /// the sequence contains no elements.
        /// </summary>

        public static TSource FirstOrDefault<TSource>(
            this IEnumerable<TSource> source, 
            TSource defaultValue)
        {
            return source.FirstImpl(() => defaultValue);
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a 
        /// condition or a default value if no such element is found.
        /// </summary>

        public static TSource FirstOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate, 
            TSource defaultValue)
        {
            return FirstOrDefault(source.Where(predicate), defaultValue);
        }

        /// <summary>
        /// Base implementation of Last operator.
        /// </summary>

        private static TSource LastImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource> empty)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var list = source as IList<TSource>;    // optimized case for lists
            if (list != null)
                return list.Count > 0 ? list[list.Count - 1] : empty();

            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                    return empty();

                var last = e.Current;
                while (e.MoveNext())
                    last = e.Current;

                return last;
            }
        }

        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        public static TSource Last<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.LastImpl(Futures<TSource>.Undefined);
        }

        /// <summary>
        /// Returns the last element of a sequence that satisfies a 
        /// specified condition.
        /// </summary>

        public static TSource Last<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return Last(source.Where(predicate));
        }

        /// <summary>
        /// Returns the last element of a sequence, or a default value if 
        /// the sequence contains no elements.
        /// </summary>

        public static TSource LastOrDefault<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.LastImpl(Futures<TSource>.Default);
        }

        /// <summary>
        /// Returns the last element of a sequence that satisfies a 
        /// condition or a default value if no such element is found.
        /// </summary>

        public static TSource LastOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return LastOrDefault(source.Where(predicate));
        }

        /// <summary>
        /// Returns the last element of a sequence, or a default value if 
        /// the sequence contains no elements.
        /// </summary>

        public static TSource LastOrDefault<TSource>(
            this IEnumerable<TSource> source, 
            TSource defaultValue)
        {
            return source.LastImpl(() => defaultValue);
        }

        /// <summary>
        /// Returns the last element of a sequence that satisfies a 
        /// condition or a default value if no such element is found.
        /// </summary>

        public static TSource LastOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate, 
            TSource defaultValue)
        {
            return LastOrDefault(source.Where(predicate), defaultValue);
        }
    }
}
