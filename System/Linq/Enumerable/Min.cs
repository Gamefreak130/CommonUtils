namespace System.Linq
{
    using System.Collections.Generic;

    // TEST
    public static partial class Enumerable
    {
        /// <summary>
        /// Returns the minimum value in a generic sequence.
        /// </summary>

        public static TSource Min<TSource>(
            this IEnumerable<TSource> source)
        {
            var comparer = Comparer<TSource>.Default;
            return source.MinMaxImpl((x, y) => comparer.Compare(x, y) < 0);
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic 
        /// sequence and returns the minimum resulting value.
        /// </summary>

        public static TResult Min<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// Returns the minimum value in a sequence of nullable 
        /// <see cref="System.Int32" /> values.
        /// </summary>

        public static int? Min(
            this IEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return MinMaxImpl(source.Where(x => x != null), null, (min, x) => min < x);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and 
        /// returns the minimum nullable <see cref="System.Int32" /> value.
        /// </summary>

        public static int? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// Returns the minimum value in a sequence of nullable 
        /// <see cref="System.Int64" /> values.
        /// </summary>

        public static long? Min(
            this IEnumerable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return MinMaxImpl(source.Where(x => x != null), null, (min, x) => min < x);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and 
        /// returns the minimum nullable <see cref="System.Int64" /> value.
        /// </summary>

        public static long? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// Returns the minimum value in a sequence of nullable 
        /// <see cref="System.Single" /> values.
        /// </summary>

        public static float? Min(
            this IEnumerable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return MinMaxImpl(source.Where(x => x != null), null, (min, x) => min < x);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and 
        /// returns the minimum nullable <see cref="System.Single" /> value.
        /// </summary>

        public static float? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// Returns the minimum value in a sequence of nullable 
        /// <see cref="System.Double" /> values.
        /// </summary>

        public static double? Min(
            this IEnumerable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return MinMaxImpl(source.Where(x => x != null), null, (min, x) => min < x);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and 
        /// returns the minimum nullable <see cref="System.Double" /> value.
        /// </summary>

        public static double? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// Returns the minimum value in a sequence of nullable 
        /// <see cref="System.Decimal" /> values.
        /// </summary>

        public static decimal? Min(
            this IEnumerable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return MinMaxImpl(source.Where(x => x != null), null, (min, x) => min < x);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and 
        /// returns the minimum nullable <see cref="System.Decimal" /> value.
        /// </summary>

        public static decimal? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>Returns the minimum value in a generic sequence according to a specified key selector function.</summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TKey">The type of key to compare elements by.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>The value with the minimum key in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">No key extracted from <paramref name="source" /> implements the <see cref="IComparable" /> or <see cref="System.IComparable{TKey}" /> interface.</exception>
        /// <remarks>
        /// <para>If <typeparamref name="TKey" /> is a reference type and the source sequence is empty or contains only values that are <see langword="null" />, this method returns <see langword="null" />.</para>
        /// </remarks>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => MinBy(source, keySelector, comparer: null);

        /// <summary>Returns the minimum value in a generic sequence according to a specified key selector function.</summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TKey">The type of key to compare elements by.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparer">The <see cref="IComparer{TKey}" /> to compare keys.</param>
        /// <returns>The value with the minimum key in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">No key extracted from <paramref name="source" /> implements the <see cref="IComparable" /> or <see cref="IComparable{TKey}" /> interface.</exception>
        /// <remarks>
        /// <para>If <typeparamref name="TKey" /> is a reference type and the source sequence is empty or contains only values that are <see langword="null" />, this method returns <see langword="null" />.</para>
        /// </remarks>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentException("keySelector");
            }

            comparer ??= Comparer<TKey>.Default;

            return MinMaxByImpl(source, keySelector, (x, y) => comparer.Compare(x, y) < 0);

            /*using IEnumerator<TSource> e = source.GetEnumerator();

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
                    if (nextKey != null && comparer.Compare(nextKey, key) < 0)
                    {
                        key = nextKey;
                        value = nextValue;
                    }
                }
            }
            else
            {
                if (comparer == Comparer<TKey>.Default)
                {
                    while (e.MoveNext())
                    {
                        TSource nextValue = e.Current;
                        TKey nextKey = keySelector(nextValue);
                        if (Comparer<TKey>.Default.Compare(nextKey, key) < 0)
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
                        if (comparer.Compare(nextKey, key) < 0)
                        {
                            key = nextKey;
                            value = nextValue;
                        }
                    }
                }
            }

            return value;*/
        }
    }
}
