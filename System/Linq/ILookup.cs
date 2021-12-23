namespace System.Linq
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines an indexer, size property, and Boolean search method for 
    /// data structures that map keys to <see cref="IEnumerable{T}"/> 
    /// sequences of values.
    /// </summary>

    public partial interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>
    {
        bool Contains(TKey key);
        int Count { get; }
        IEnumerable<TElement> this[TKey key] { get; }
    }
}
