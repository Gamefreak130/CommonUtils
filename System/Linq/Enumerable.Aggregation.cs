namespace System.Linq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    static partial class Enumerable
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>

        public static int Count<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var collection = source as ICollection;
            return collection != null
                 ? collection.Count
                 : source.Aggregate(0, (count, item) => checked(count + 1));
        }

        /// <summary>
        /// Returns a number that represents how many elements in the 
        /// specified sequence satisfy a condition.
        /// </summary>

        public static int Count<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return Count(source.Where(predicate));
        }

        /// <summary>
        /// Returns an <see cref="Int64"/> that represents the total number 
        /// of elements in a sequence.
        /// </summary>

        public static long LongCount<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var array = source as Array;
            return array != null
                 ? array.LongLength
                 : source.Aggregate(0L, (count, item) => count + 1);
        }

        /// <summary>
        /// Returns an <see cref="Int64"/> that represents how many elements 
        /// in a sequence satisfy a condition.
        /// </summary>

        public static long LongCount<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return LongCount(source.Where(predicate));
        }

        /// <summary>
        /// Applies an accumulator function over a sequence.
        /// </summary>

        public static TSource Aggregate<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, TSource> func)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (func == null)
                throw new ArgumentNullException("func");

            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                    throw new InvalidOperationException();

                return e.Renumerable().Skip(1).Aggregate(e.Current, func);
            }
        }

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified 
        /// seed value is used as the initial accumulator value.
        /// </summary>

        public static TAccumulate Aggregate<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func)
        {
            return Aggregate(source, seed, func, r => r);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified 
        /// seed value is used as the initial accumulator value, and the 
        /// specified function is used to select the result value.
        /// </summary>

        public static TResult Aggregate<TSource, TAccumulate, TResult>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (func == null)
                throw new ArgumentNullException("func");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            var result = seed;

            foreach (var item in source)
                result = func(result, item);

            return resultSelector(result);
        }

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
