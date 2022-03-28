namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Makes an enumerator seen as enumerable once more.
        /// </summary>
        /// <remarks>
        /// The supplied enumerator must have been started. The first element
        /// returned is the element the enumerator was on when passed in.
        /// DO NOT use this method if the caller must be a generator. It is
        /// mostly safe among aggregate operations.
        /// </remarks>

        private static IEnumerable<T> Renumerable<T>(this IEnumerator<T> e)
        {
            //Debug.Assert(e != null);

            do
            { yield return e.Current; } while (e.MoveNext());
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
    }
}
