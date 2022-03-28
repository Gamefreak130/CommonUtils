namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Determines whether two sequences are equal by comparing the 
        /// elements by using the default equality comparer for their type.
        /// </summary>

        public static bool SequenceEqual<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return first.SequenceEqual(second, /* comparer */ null);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing their 
        /// elements by using a specified <see cref="IEqualityComparer{T}" />.
        /// </summary>

        public static bool SequenceEqual<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException("frist");
            if (second == null)
                throw new ArgumentNullException("second");

            comparer = comparer ?? EqualityComparer<TSource>.Default;

            using (IEnumerator<TSource> lhs = first.GetEnumerator(),
                                        rhs = second.GetEnumerator())
            {
                do
                {
                    if (!lhs.MoveNext())
                        return !rhs.MoveNext();

                    if (!rhs.MoveNext())
                        return false;
                }
                while (comparer.Equals(lhs.Current, rhs.Current));
            }

            return false;
        }
    }
}
