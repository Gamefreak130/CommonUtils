namespace System.Linq
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of objects that have a common key.
    /// </summary>

    public interface IGrouping<out TKey, TElement> : IEnumerable<TElement>
    {
        /// <summary>
        /// Gets the key of the <see cref="IGrouping{TKey,TElement}" />.
        /// </summary>

        TKey Key { get; }
    }
}
