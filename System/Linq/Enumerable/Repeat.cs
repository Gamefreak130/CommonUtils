namespace System.Linq
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Generates a sequence that contains one repeated value.
        /// </summary>

        public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, null);

            return RepeatYield(element, count);
        }

        private static IEnumerable<TResult> RepeatYield<TResult>(TResult element, int count)
        {
            for (var i = 0; i < count; i++)
                yield return element;
        }
    }
}
