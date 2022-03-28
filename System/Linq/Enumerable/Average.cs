namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="System.Int32" /> values.
        /// </summary>

        public static double Average(
            this IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            long sum = 0;
            long count = 0;

            foreach (var num in source)
                checked
                {
                    sum += (int)num;
                    count++;
                }

            if (count == 0)
                throw new InvalidOperationException();

            return (double)sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="System.Int32" /> values 
        /// that are obtained by invoking a transform function on each 
        /// element of the input sequence.
        /// </summary>

        public static double Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="System.Int32" /> values.
        /// </summary>

        public static double? Average(
            this IEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            long sum = 0;
            long count = 0;

            foreach (var num in source.Where(n => n != null))
                checked
                {
                    sum += (int)num;
                    count++;
                }

            if (count == 0)
                return null;

            return (double?)sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="System.Int32" /> values 
        /// that are obtained by invoking a transform function on each 
        /// element of the input sequence.
        /// </summary>

        public static double? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int?> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="System.Int64" /> values.
        /// </summary>

        public static double Average(
            this IEnumerable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            long sum = 0;
            long count = 0;

            foreach (var num in source)
                checked
                {
                    sum += (long)num;
                    count++;
                }

            if (count == 0)
                throw new InvalidOperationException();

            return (double)sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="System.Int64" /> values 
        /// that are obtained by invoking a transform function on each 
        /// element of the input sequence.
        /// </summary>

        public static double Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="System.Int64" /> values.
        /// </summary>

        public static double? Average(
            this IEnumerable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            long sum = 0;
            long count = 0;

            foreach (var num in source.Where(n => n != null))
                checked
                {
                    sum += (long)num;
                    count++;
                }

            if (count == 0)
                return null;

            return (double?)sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="System.Int64" /> values 
        /// that are obtained by invoking a transform function on each 
        /// element of the input sequence.
        /// </summary>

        public static double? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, long?> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="System.Single" /> values.
        /// </summary>

        public static float Average(
            this IEnumerable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            float sum = 0;
            long count = 0;

            foreach (var num in source)
                checked
                {
                    sum += (float)num;
                    count++;
                }

            if (count == 0)
                throw new InvalidOperationException();

            return (float)sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="System.Single" /> values 
        /// that are obtained by invoking a transform function on each 
        /// element of the input sequence.
        /// </summary>

        public static float Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="System.Single" /> values.
        /// </summary>

        public static float? Average(
            this IEnumerable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            float sum = 0;
            long count = 0;

            foreach (var num in source.Where(n => n != null))
                checked
                {
                    sum += (float)num;
                    count++;
                }

            if (count == 0)
                return null;

            return (float?)sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="System.Single" /> values 
        /// that are obtained by invoking a transform function on each 
        /// element of the input sequence.
        /// </summary>

        public static float? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, float?> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="System.Double" /> values.
        /// </summary>

        public static double Average(
            this IEnumerable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            double sum = 0;
            long count = 0;

            foreach (var num in source)
                checked
                {
                    sum += (double)num;
                    count++;
                }

            if (count == 0)
                throw new InvalidOperationException();

            return (double)sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="System.Double" /> values 
        /// that are obtained by invoking a transform function on each 
        /// element of the input sequence.
        /// </summary>

        public static double Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="System.Double" /> values.
        /// </summary>

        public static double? Average(
            this IEnumerable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            double sum = 0;
            long count = 0;

            foreach (var num in source.Where(n => n != null))
                checked
                {
                    sum += (double)num;
                    count++;
                }

            if (count == 0)
                return null;

            return (double?)sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="System.Double" /> values 
        /// that are obtained by invoking a transform function on each 
        /// element of the input sequence.
        /// </summary>

        public static double? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, double?> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="System.Decimal" /> values.
        /// </summary>

        public static decimal Average(
            this IEnumerable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            decimal sum = 0;
            long count = 0;

            foreach (var num in source)
                checked
                {
                    sum += (decimal)num;
                    count++;
                }

            if (count == 0)
                throw new InvalidOperationException();

            return (decimal)sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="System.Decimal" /> values 
        /// that are obtained by invoking a transform function on each 
        /// element of the input sequence.
        /// </summary>

        public static decimal Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="System.Decimal" /> values.
        /// </summary>

        public static decimal? Average(
            this IEnumerable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            decimal sum = 0;
            long count = 0;

            foreach (var num in source.Where(n => n != null))
                checked
                {
                    sum += (decimal)num;
                    count++;
                }

            if (count == 0)
                return null;

            return (decimal?)sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="System.Decimal" /> values 
        /// that are obtained by invoking a transform function on each 
        /// element of the input sequence.
        /// </summary>

        public static decimal? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, decimal?> selector)
        {
            return source.Select(selector).Average();
        }
    }
}
