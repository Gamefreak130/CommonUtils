namespace System.Linq
{
    using System.Collections.Generic;

    /// <remarks>
    /// This type is not intended to be used directly from user code.
    /// It may be removed or changed in a future version without notice.
    /// </remarks>

    internal sealed class KeyComparer<T> : IEqualityComparer<Key<T>>
    {
        private readonly IEqualityComparer<T> _innerComparer;

        public KeyComparer(IEqualityComparer<T> innerComparer)
        {
            _innerComparer = innerComparer ?? EqualityComparer<T>.Default;
        }

        public bool Equals(Key<T> x, Key<T> y)
        {
            return _innerComparer.Equals(x.Value, y.Value);
        }

        public int GetHashCode(Key<T> obj)
        {
            return obj.Value == null ? 0 : _innerComparer.GetHashCode(obj.Value);
        }
    }
}
