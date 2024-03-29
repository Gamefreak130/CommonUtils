﻿namespace System.Linq
{
    using System.Collections;
    using System.Collections.Generic;

    // TEST
    public static partial class Enumerable
    {
        /// <summary>Returns the element at a specified index in a sequence.</summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
        /// <param name="index">The index of the element to retrieve, which is either from the start or the end.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is outside the bounds of the <paramref name="source" /> sequence.</exception>
        /// <returns>The element at the specified position in the <paramref name="source" /> sequence.</returns>
        /// <remarks>
        /// <para>If the type of <paramref name="source" /> implements <see cref="IList{T}" />, that implementation is used to obtain the element at the specified index. Otherwise, this method obtains the specified element.</para>
        /// <para>This method throws an exception if <paramref name="index" /> is out of range. To instead return a default value when the specified index is out of range, use the <see cref="O:Enumerable.ElementAtOrDefault" /> method.</para>
        /// </remarks>
        public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, Index index)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (!index.IsFromEnd)
            {
                return source.ElementAt(index.Value);
            }

            if (source is ICollection)
            {
                return source.ElementAt(source.Count() - index.Value);
            }

            if (!TryGetElementFromEnd(source, index.Value, out TSource element))
            {
                throw new ArgumentOutOfRangeException("index", index, null);
            }

            return element;
        }

        /// <summary>Returns the element at a specified index in a sequence or a default value if the index is out of range.</summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
        /// <param name="index">The index of the element to retrieve, which is either from the start or the end.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
        /// <returns><see langword="default" /> if <paramref name="index" /> is outside the bounds of the <paramref name="source" /> sequence; otherwise, the element at the specified position in the <paramref name="source" /> sequence.</returns>
        /// <remarks>
        /// <para>If the type of <paramref name="source" /> implements <see cref="IList{T}" />, that implementation is used to obtain the element at the specified index. Otherwise, this method obtains the specified element.</para>
        /// <para>The default value for reference and nullable types is <see langword="null" />.</para>
        /// </remarks>
        public static TSource ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, Index index)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (!index.IsFromEnd)
            {
                return source.ElementAtOrDefault(index.Value);
            }

            if (source is ICollection)
            {
                return source.ElementAtOrDefault(source.Count() - index.Value);
            }

            TryGetElementFromEnd(source, index.Value, out TSource element);
            return element;
        }

        private static bool TryGetElementFromEnd<TSource>(IEnumerable<TSource> source, int indexFromEnd, out TSource element)
        {
            //Debug.Assert(source != null);

            if (indexFromEnd > 0)
            {
                using IEnumerator<TSource> e = source.GetEnumerator();
                if (e.MoveNext())
                {
                    Queue<TSource> queue = new();
                    queue.Enqueue(e.Current);
                    while (e.MoveNext())
                    {
                        if (queue.Count == indexFromEnd)
                        {
                            queue.Dequeue();
                        }

                        queue.Enqueue(e.Current);
                    }

                    if (queue.Count == indexFromEnd)
                    {
                        element = queue.Dequeue();
                        return true;
                    }
                }
            }

            element = default;
            return false;
        }
    }
}
