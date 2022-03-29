namespace System.Linq
{
    using System.Collections.Generic;

    // TEST
    public static partial class Enumerable
    {
        /// <summary>
        /// Applies a specified function to the corresponding elements of two sequences, producing a sequence of the results.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="resultSelector">A function that specifies how to merge the elements from the two sequences.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains merged elements of two input sequences.</returns>
        
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first is null)
            {
                throw new ArgumentNullException("first");
            }

            if (second is null)
            {
                throw new ArgumentNullException("second");
            }

            if (resultSelector is null)
            {
                throw new ArgumentNullException("resultSelector");
            }

            return ZipYield(first, second, resultSelector);
        }

        /// <summary>
        /// Produces a sequence of tuples with elements from the two specified sequences.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <returns>A sequence of tuples with elements taken from the first and second sequences, in that order.</returns>

        public static IEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
        {
            if (first is null)
            {
                throw new ArgumentNullException("first");
            }

            if (second is null)
            {
                throw new ArgumentNullException("second");
            }

            return ZipYield(first, second, (e1, e2) => (e1, e2));
        }

        /// <summary>
        /// Produces a sequence of tuples with elements from the three specified sequences.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements of the third input sequence.</typeparam>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="third">The third sequence to merge.</param>
        /// <returns>A sequence of tuples with elements taken from the first, second, and third sequences, in that order.</returns>
        
        public static IEnumerable<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third)
        {
            if (first is null)
            {
                throw new ArgumentNullException("first");
            }

            if (second is null)
            {
                throw new ArgumentNullException("second");
            }

            if (third is null)
            {
                throw new ArgumentNullException("third");
            }

            return ZipYield(first, second, third);
        }

        private static IEnumerable<TResult> ZipYield<TFirst, TSecond, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            using (IEnumerator<TFirst> e1 = first.GetEnumerator())
            using (IEnumerator<TSecond> e2 = second.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    yield return resultSelector(e1.Current, e2.Current);
                }
            }
        }

        private static IEnumerable<(TFirst First, TSecond Second, TThird Third)> ZipYield<TFirst, TSecond, TThird>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third)
        {
            using (IEnumerator<TFirst> e1 = first.GetEnumerator())
            using (IEnumerator<TSecond> e2 = second.GetEnumerator())
            using (IEnumerator<TThird> e3 = third.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                {
                    yield return (e1.Current, e2.Current, e3.Current);
                }
            }
        }
    }
}