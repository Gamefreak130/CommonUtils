namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="System.Int32" /> values.
        /// </summary>

        public static int Sum(
            this IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int sum = 0;
            foreach (var num in source)
                sum = checked(sum + num);

            return sum;
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="System.Int32" /> 
        /// values that are obtained by invoking a transform function on 
        /// each element of the input sequence.
        /// </summary>

        public static int Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="System.Int32" /> values.
        /// </summary>

        public static int? Sum(
            this IEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int sum = 0;
            foreach (var num in source)
                sum = checked(sum + (num ?? 0));

            return sum;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="System.Int32" /> 
        /// values that are obtained by invoking a transform function on 
        /// each element of the input sequence.
        /// </summary>

        public static int? Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int?> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="System.Int64" /> values.
        /// </summary>

        public static long Sum(
            this IEnumerable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            long sum = 0;
            foreach (var num in source)
                sum = checked(sum + num);

            return sum;
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="System.Int64" /> 
        /// values that are obtained by invoking a transform function on 
        /// each element of the input sequence.
        /// </summary>

        public static long Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="System.Int64" /> values.
        /// </summary>

        public static long? Sum(
            this IEnumerable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            long sum = 0;
            foreach (var num in source)
                sum = checked(sum + (num ?? 0));

            return sum;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="System.Int64" /> 
        /// values that are obtained by invoking a transform function on 
        /// each element of the input sequence.
        /// </summary>

        public static long? Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long?> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="System.Single" /> values.
        /// </summary>

        public static float Sum(
            this IEnumerable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            float sum = 0;
            foreach (var num in source)
                sum = checked(sum + num);

            return sum;
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="System.Single" /> 
        /// values that are obtained by invoking a transform function on 
        /// each element of the input sequence.
        /// </summary>

        public static float Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="System.Single" /> values.
        /// </summary>

        public static float? Sum(
            this IEnumerable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            float sum = 0;
            foreach (var num in source)
                sum = checked(sum + (num ?? 0));

            return sum;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="System.Single" /> 
        /// values that are obtained by invoking a transform function on 
        /// each element of the input sequence.
        /// </summary>

        public static float? Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float?> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="System.Double" /> values.
        /// </summary>

        public static double Sum(
            this IEnumerable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            double sum = 0;
            foreach (var num in source)
                sum = checked(sum + num);

            return sum;
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="System.Double" /> 
        /// values that are obtained by invoking a transform function on 
        /// each element of the input sequence.
        /// </summary>

        public static double Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="System.Double" /> values.
        /// </summary>

        public static double? Sum(
            this IEnumerable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            double sum = 0;
            foreach (var num in source)
                sum = checked(sum + (num ?? 0));

            return sum;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="System.Double" /> 
        /// values that are obtained by invoking a transform function on 
        /// each element of the input sequence.
        /// </summary>

        public static double? Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double?> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="System.Decimal" /> values.
        /// </summary>

        public static decimal Sum(
            this IEnumerable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            decimal sum = 0;
            foreach (var num in source)
                sum = checked(sum + num);

            return sum;
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="System.Decimal" /> 
        /// values that are obtained by invoking a transform function on 
        /// each element of the input sequence.
        /// </summary>

        public static decimal Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="System.Decimal" /> values.
        /// </summary>

        public static decimal? Sum(
            this IEnumerable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            decimal sum = 0;
            foreach (var num in source)
                sum = checked(sum + (num ?? 0));

            return sum;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="System.Decimal" /> 
        /// values that are obtained by invoking a transform function on 
        /// each element of the input sequence.
        /// </summary>

        public static decimal? Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal?> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}
