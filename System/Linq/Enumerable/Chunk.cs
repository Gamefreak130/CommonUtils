﻿namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
        /// </summary>
        /// <remarks>
        /// Every chunk except the last will be of size <paramref name="size"/>.
        /// The last chunk will contain the remaining elements and may be of a smaller size.
        /// </remarks>
        /// <param name="source">
        /// An <see cref="IEnumerable{T}"/> whose elements to chunk.
        /// </param>
        /// <param name="size">
        /// Maximum size of each chunk.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that contains the elements the input sequence split into chunks of size <paramref name="size"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="size"/> is below 1.
        /// </exception>
        public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> source, int size)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException("size", size, null);
            }

            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    TSource[] chunk = new TSource[size];
                    chunk[0] = e.Current;

                    int i = 1;
                    for (; i < chunk.Length && e.MoveNext(); i++)
                    {
                        chunk[i] = e.Current;
                    }

                    if (i == chunk.Length)
                    {
                        yield return chunk;
                    }
                    else
                    {
                        Array.Resize(ref chunk, i);
                        yield return chunk;
                        yield break;
                    }
                }
            }
        }
    }
}
