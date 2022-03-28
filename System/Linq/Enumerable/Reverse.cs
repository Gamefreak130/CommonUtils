namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>

        public static IEnumerable<TSource> Reverse<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return ReverseYield(source);
        }

        private static IEnumerable<TSource> ReverseYield<TSource>(IEnumerable<TSource> source)
        {
            var stack = new Stack<TSource>();
            foreach (var item in source)
                stack.Push(item);

            foreach (var item in stack)
                yield return item;
        }
    }
}
