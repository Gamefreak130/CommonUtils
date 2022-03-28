namespace System.Linq
{
    using System.Collections.Generic;

    // CONSIDER MaxBy, MinBy
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

        /// <summary>
        /// Returns the maximum value in a generic sequence.
        /// </summary>

        public static TSource Max<TSource>(
            this IEnumerable<TSource> source)
        {
            var comparer = Comparer<TSource>.Default;
            return source.MinMaxImpl((x, y) => comparer.Compare(x, y) > 0);
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic 
        /// sequence and returns the maximum resulting value.
        /// </summary>

        public static TResult Max<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// Returns the maximum value in a sequence of nullable 
        /// <see cref="System.Int32" /> values.
        /// </summary>

        public static int? Max(
            this IEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return MinMaxImpl(source.Where(x => x != null),
                null, (max, x) => x == null || (max != null && x.Value < max.Value));
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and 
        /// returns the maximum nullable <see cref="System.Int32" /> value.
        /// </summary>

        public static int? Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int?> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// Returns the maximum value in a sequence of nullable 
        /// <see cref="System.Int64" /> values.
        /// </summary>

        public static long? Max(
            this IEnumerable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return MinMaxImpl(source.Where(x => x != null),
                null, (max, x) => x == null || (max != null && x.Value < max.Value));
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and 
        /// returns the maximum nullable <see cref="System.Int64" /> value.
        /// </summary>

        public static long? Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long?> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// Returns the maximum value in a sequence of nullable 
        /// <see cref="System.Single" /> values.
        /// </summary>

        public static float? Max(
            this IEnumerable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return MinMaxImpl(source.Where(x => x != null),
                null, (max, x) => x == null || (max != null && x.Value < max.Value));
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and 
        /// returns the maximum nullable <see cref="System.Single" /> value.
        /// </summary>

        public static float? Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float?> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// Returns the maximum value in a sequence of nullable 
        /// <see cref="System.Double" /> values.
        /// </summary>

        public static double? Max(
            this IEnumerable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return MinMaxImpl(source.Where(x => x != null),
                null, (max, x) => x == null || (max != null && x.Value < max.Value));
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and 
        /// returns the maximum nullable <see cref="System.Double" /> value.
        /// </summary>

        public static double? Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double?> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// Returns the maximum value in a sequence of nullable 
        /// <see cref="System.Decimal" /> values.
        /// </summary>

        public static decimal? Max(
            this IEnumerable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return MinMaxImpl(source.Where(x => x != null),
                null, (max, x) => x == null || (max != null && x.Value < max.Value));
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and 
        /// returns the maximum nullable <see cref="System.Decimal" /> value.
        /// </summary>

        public static decimal? Max<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal?> selector)
        {
            return source.Select(selector).Max();
        }
    }
}
