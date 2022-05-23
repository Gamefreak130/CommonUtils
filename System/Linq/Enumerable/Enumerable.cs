namespace System.Linq
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for 
    /// querying objects that implement <see cref="IEnumerable{T}" />.
    /// </summary>

    public static partial class Enumerable
    {
        private static class Futures<T>
        {
            public static readonly Func<T> Default = () => default(T);
            public static readonly Func<T> Undefined = () => { throw new InvalidOperationException(); };
        }

        private static class Sequence<T>
        {
            public static readonly IEnumerable<T> Empty = new T[0];
        }

        /// <summary>
        /// Makes an enumerator seen as enumerable once more.
        /// </summary>
        /// <remarks>
        /// The supplied enumerator must have been started. The first element
        /// returned is the element the enumerator was on when passed in.
        /// DO NOT use this method if the caller must be a generator. It is
        /// mostly safe among aggregate operations.
        /// </remarks>

        public static IEnumerable<T> Renumerable<T>(this IEnumerator<T> e)
        {
            //Debug.Assert(e != null);

            do
            { yield return e.Current; } while (e.MoveNext());
        }

        /// <summary>
        /// Makes an enumerator seen as enumerable once more.
        /// </summary>
        /// <remarks>
        /// The supplied enumerator must have been started. The first element
        /// returned is the element the enumerator was on when passed in.
        /// DO NOT use this method if the caller must be a generator. It is
        /// mostly safe among aggregate operations.
        /// </remarks>

        public static IEnumerable<T> Renumerable<T>(this IEnumerator e)
        {
            do
            { yield return (T)e.Current; } while (e.MoveNext());
        }

        /// <summary>
        /// Returns an empty <see cref="IEnumerable{T}"/> that has the 
        /// specified type argument.
        /// </summary>

        public static IEnumerable<TResult> Empty<TResult>()
        {
            return Sequence<TResult>.Empty;
        }

        /// <summary>
        /// Returns the input typed as <see cref="IEnumerable{T}"/>.
        /// </summary>

        public static IEnumerable<TResult> AsEnumerable<TResult>(this IEnumerable<TResult> source)
        {
            return source;
        }
    }
}
