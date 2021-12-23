namespace System
{
    using System.Collections;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Helper so we can call some tuple methods recursively without knowing the underlying types.
    /// </summary>
    interface IValueTupleInternal : ITuple
    {
        int GetHashCode(IEqualityComparer comparer);
        string ToStringEnd();
    }
}
