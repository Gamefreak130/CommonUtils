namespace System.Linq
{
    /// <remarks>
    /// This type is not intended to be used directly from user code.
    /// It may be removed or changed in a future version without notice.
    /// </remarks>

    internal struct Key<T>
    {
        private readonly T value;
        public Key(T val) : this() { value = val; }
        public T Value => value;
    }
}
