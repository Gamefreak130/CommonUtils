namespace System.Linq
{
    // TODO Prepend, Append, Chunk, Zip
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for 
    /// querying objects that implement <see cref="IEnumerable{T}" />.
    /// </summary>

    public static partial class Enumerable
    {
        private sealed class Grouping<K, V> : List<V>, IGrouping<K, V>
        {
            internal Grouping(K key)
            {
                Key = key;
            }

            public K Key { get; private set; }
        }

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
        /// Returns an empty <see cref="IEnumerable{T}"/> that has the 
        /// specified type argument.
        /// </summary>

        public static IEnumerable<TResult> Empty<TResult>()
        {
            return Sequence<TResult>.Empty;
        }

        public static IEnumerable<TResult> AsEnumerable<TResult>(this IEnumerable<TResult> source)
        {
            return source;
        }
    }
}
