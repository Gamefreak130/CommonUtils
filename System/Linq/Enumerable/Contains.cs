namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Determines whether a sequence contains a specified element by 
        /// using the default equality comparer.
        /// </summary>

        public static bool Contains<TSource>(
            this IEnumerable<TSource> source,
            TSource value)
        {
            return source.Contains(value, /* comparer */ null);
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element by 
        /// using a specified <see cref="IEqualityComparer{T}" />.
        /// </summary>

        public static bool Contains<TSource>(
            this IEnumerable<TSource> source,
            TSource value,
            IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (comparer == null)
            {
                var collection = source as ICollection<TSource>;
                if (collection != null)
                    return collection.Contains(value);
            }

            comparer = comparer ?? EqualityComparer<TSource>.Default;
            return source.Any(item => comparer.Equals(item, value));
        }
    }
}
