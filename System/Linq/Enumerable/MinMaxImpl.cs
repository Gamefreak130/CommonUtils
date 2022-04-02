namespace System.Linq
{
    using System.Collections.Generic;

    // TEST MinMaxByImpl
    public static partial class Enumerable
    {
        /// <summary>
        /// Base implementation for Min/Max operator.
        /// </summary>

        private static TSource MinMaxImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, bool> lesser)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            //Debug.Assert(lesser != null);

            if (typeof(TSource).IsClass) // ReSharper disable CompareNonConstrainedGenericWithNull                
                source = source.Where(e => e != null).DefaultIfEmpty(); // ReSharper restore CompareNonConstrainedGenericWithNull

            return source.Aggregate((a, item) => lesser(a, item) ? a : item);
        }

        /// <summary>
        /// Base implementation for Min/Max operator for nullable types.
        /// </summary>

        private static TSource? MinMaxImpl<TSource>(
            this IEnumerable<TSource?> source,
            TSource? seed, Func<TSource?, TSource?, bool> lesser) where TSource : struct
        {
            if (source == null)
                throw new ArgumentNullException("source");
            //Debug.Assert(lesser != null);

            return source.Aggregate(seed, (a, item) => lesser(a, item) ? a : item);
            //  == MinMaxImpl(Repeat<TSource?>(null, 1).Concat(source), lesser);
        }

        /// <summary>
        /// Base implementation for MinBy/MaxBy operator.
        /// </summary>

        private static TSource MinMaxByImpl<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, TKey, bool> lesser)
        {
            if (source == null)
            {
                throw new ArgumentException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentException("keySelector");
            }

            using IEnumerator<TSource> e = source.GetEnumerator();

            if (!e.MoveNext())
            {
                if (default(TSource) is null)
                {
                    return default;
                }
                else
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
            }

            TSource value = e.Current;
            TKey key = keySelector(value);

            if (default(TKey) is null)
            {
                if (key == null)
                {
                    TSource firstValue = value;

                    do
                    {
                        if (!e.MoveNext())
                        {
                            // All keys are null, surface the first element.
                            return firstValue;
                        }

                        value = e.Current;
                        key = keySelector(value);
                    }
                    while (key == null);
                }

                while (e.MoveNext())
                {
                    TSource nextValue = e.Current;
                    TKey nextKey = keySelector(nextValue);
                    if (nextKey != null && lesser(nextKey, key))
                    {
                        key = nextKey;
                        value = nextValue;
                    }
                }
            }
            else
            {
                while (e.MoveNext())
                {
                    TSource nextValue = e.Current;
                    TKey nextKey = keySelector(nextValue);
                    if (lesser(nextKey, key))
                    {
                        key = nextKey;
                        value = nextValue;
                    }
                }
            }

            return value;
        }
    }
}
