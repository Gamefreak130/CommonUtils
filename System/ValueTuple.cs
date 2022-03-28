namespace System
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The ValueTuple types (from arity 0 to 8) comprise the runtime implementation that underlies tuples in C# and struct tuples in F#.
    /// Aside from created via language syntax, they are most easily created via the ValueTuple.Create factory methods.
    /// The System.ValueTuple types differ from the System.Tuple types in that:
    /// - they are structs rather than classes,
    /// - they are mutable rather than readonly, and
    /// - their members (such as Item1, Item2, etc) are fields rather than properties.
    /// </summary>
    [Serializable]
    public partial struct ValueTuple : IEquatable<ValueTuple>,
		IComparable, IComparable<ValueTuple>, IValueTupleInternal
	{
		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple"/> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="ValueTuple"/>.</returns>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple;
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified value.</summary>
		/// <param name="other">An instance to compare to this instance.</param>
		/// <returns>true if <paramref name="other"/> has the same value as this instance; otherwise, false.</returns>
		public bool Equals(ValueTuple other)
		{
			return true;
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null)       return 1;
			if (other is ValueTuple) return 0;

			throw new ArgumentException();

		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other"/>.
		/// Returns less than zero if this instance is less than <paramref name="other"/>, zero if this
		/// instance is equal to <paramref name="other"/>, and greater than zero if this instance is greater
		/// than <paramref name="other"/>.
		/// </returns>
		public int CompareTo(ValueTuple other)
		{
			return 0;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return 0;
		}

		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return 0;
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="ValueTuple"/> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="ValueTuple"/> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>()</c>.
		/// </remarks>
		public override string ToString()
		{
			return "()";
		}

		string IValueTupleInternal.ToStringEnd()
		{
			return ")";
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 0;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index]
		{
			get { throw new IndexOutOfRangeException(); }
		}

		/// <summary>Creates a new struct 0-tuple.</summary>
		/// <returns>A 0-tuple.</returns>
		public static ValueTuple Create() =>
			new ValueTuple();

        /// <summary>Creates a new struct 1-tuple, or singleton.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <returns>A 1-tuple (singleton) whose value is (item1).</returns>
		public static ValueTuple<T1> Create<T1>(T1 item1) =>
            new ValueTuple<T1>(item1);

        /// <summary>Creates a new struct 2-tuple, or pair.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <param name="item2">The value of the second component of the tuple.</param>
		/// <returns>A 2-tuple (pair) whose value is (item1, item2).</returns>
		public static ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) =>
            new ValueTuple<T1, T2>(item1, item2);

        internal static int CombineHashCodes(int h1, int h2)
		{
			return Combine(Combine(_randomSeed, h1), h2);
		}

		internal static int CombineHashCodes(int h1, int h2, int h3)
		{
			return Combine(CombineHashCodes(h1, h2), h3);
		}

		internal static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return Combine(CombineHashCodes(h1, h2, h3), h4);
		}

		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return Combine(CombineHashCodes(h1, h2, h3, h4), h5);
		}

		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return Combine(CombineHashCodes(h1, h2, h3, h4, h5), h6);
		}

		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return Combine(CombineHashCodes(h1, h2, h3, h4, h5, h6), h7);
		}

		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
		{
			return Combine(CombineHashCodes(h1, h2, h3, h4, h5, h6, h7), h8);
		}

		static readonly int _randomSeed = new Random().Next(int.MinValue, int.MaxValue);

		static int Combine(int h1, int h2)
		{
			// RyuJIT optimizes this to use the ROL instruction
			// Related GitHub pull request: dotnet/coreclr#1830
			var rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
			return ((int)rol5 + h1) ^ h2;
		}
	}

    /// <summary>Represents a 1-tuple, or singleton, as a value type.</summary>
	/// <typeparam name="T1">The type of the tuple's only component.</typeparam>
	[Serializable]
    public struct ValueTuple<T1> : IEquatable<ValueTuple<T1>>,
        IComparable, IComparable<ValueTuple<T1>>, IValueTupleInternal
    {
        /// <summary>
        /// The current <see cref="ValueTuple{T1}"/> instance's first component.
        /// </summary>
        public readonly T1 Item1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTuple{T1}"/> value type.
        /// </summary>
        /// <param name="item1">The value of the tuple's first component.</param>
        public ValueTuple(T1 item1)
        {
            Item1 = item1;
        }

        /// <summary>
        /// Returns a value that indicates whether the current <see cref="ValueTuple{T1}"/> instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// The <paramref name="obj"/> parameter is considered to be equal to the current instance under the following conditions:
        /// <list type="bullet">
        ///     <item><description>It is a <see cref="ValueTuple{T1}"/> value type.</description></item>
        ///     <item><description>Its components are of the same types as those of the current instance.</description></item>
        ///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
        /// </list>
        /// </remarks>
        public override bool Equals(object obj)
        {
            return obj is ValueTuple<T1> && Equals((ValueTuple<T1>)obj);
        }

        /// <summary>
        /// Returns a value that indicates whether the current <see cref="ValueTuple{T1}"/>
        /// instance is equal to a specified <see cref="ValueTuple{T1}"/>.
        /// </summary>
        /// <param name="other">The tuple to compare with this instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the specified tuple; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// The <paramref name="other"/> parameter is considered to be equal to the current instance if each of its field
        /// is equal to that of the current instance, using the default comparer for that field's type.
        /// </remarks>
        public bool Equals(ValueTuple<T1> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1);
        }

        int IComparable.CompareTo(object other)
        {
            if (other == null)
                return 1;

            if (!(other is ValueTuple<T1>))
            {
                throw new ArgumentException();
            }

            var objTuple = (ValueTuple<T1>)other;

            return Comparer<T1>.Default.Compare(Item1, objTuple.Item1);
        }

        /// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
        /// <param name="other">An instance to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and <paramref name="other"/>.
        /// Returns less than zero if this instance is less than <paramref name="other"/>, zero if this
        /// instance is equal to <paramref name="other"/>, and greater than zero if this instance is greater
        /// than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(ValueTuple<T1> other)
        {
            return Comparer<T1>.Default.Compare(Item1, other.Item1);
        }

        /// <summary>
        /// Returns the hash code for the current <see cref="ValueTuple{T1}"/> instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Item1?.GetHashCode() ?? 0;
        }

        int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Item1);
        }

        /// <summary>
        /// Returns a string that represents the value of this <see cref="ValueTuple{T1}"/> instance.
        /// </summary>
        /// <returns>The string representation of this <see cref="ValueTuple{T1}"/> instance.</returns>
        /// <remarks>
        /// The string returned by this method takes the form <c>(Item1)</c>,
        /// where <c>Item1</c> represents the value of <see cref="Item1"/>. If the field is <see langword="null"/>,
        /// it is represented as <see cref="string.Empty"/>.
        /// </remarks>
        public override string ToString()
        {
            return $"({Item1})";
        }

        string IValueTupleInternal.ToStringEnd()
        {
            return $"{Item1})";
        }

        /// <summary>
        /// The number of positions in this data structure.
        /// </summary>
        int ITuple.Length => 1;

        /// <summary>
        /// Get the element at position <param name="index"/>.
        /// </summary>
        object ITuple.this[int index]
        {
            get
            {
                if (index != 0)
                    throw new IndexOutOfRangeException();
                return Item1;
            }
        }
    }

    /// <summary>
	/// Represents a 2-tuple, or pair, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	[Serializable]
    [StructLayout(LayoutKind.Auto)]
    public struct ValueTuple<T1, T2> : IEquatable<ValueTuple<T1, T2>>,
        IComparable, IComparable<ValueTuple<T1, T2>>, IValueTupleInternal
    {
        /// <summary>
        /// The current <see cref="ValueTuple{T1,T2}"/> instance's first component.
        /// </summary>
        public readonly T1 Item1;

        /// <summary>
        /// The current <see cref="ValueTuple{T1,T2}"/> instance's first component.
        /// </summary>
        public readonly T2 Item2;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTuple{T1,T2}"/> value type.
        /// </summary>
        /// <param name="item1">The value of the tuple's first component.</param>
        /// <param name="item2">The value of the tuple's second component.</param>
        public ValueTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        /// <summary>
        /// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2}"/> instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        ///
        /// <remarks>
        /// The <paramref name="obj"/> parameter is considered to be equal to the current instance under the following conditions:
        /// <list type="bullet">
        ///     <item><description>It is a <see cref="ValueTuple{T1,T2}"/> value type.</description></item>
        ///     <item><description>Its components are of the same types as those of the current instance.</description></item>
        ///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
        /// </list>
        /// </remarks>
        public override bool Equals(object obj)
        {
            return obj is ValueTuple<T1, T2> && Equals((ValueTuple<T1, T2>)obj);
        }

        /// <summary>
        /// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2}"/> instance is equal to a specified <see cref="ValueTuple{T1,T2}"/>.
        /// </summary>
        /// <param name="other">The tuple to compare with this instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the specified tuple; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// The <paramref name="other"/> parameter is considered to be equal to the current instance if each of its fields
        /// are equal to that of the current instance, using the default comparer for that field's type.
        /// </remarks>
        public bool Equals(ValueTuple<T1, T2> other)
        {
            return
                EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
                EqualityComparer<T2>.Default.Equals(Item2, other.Item2);
        }

        int IComparable.CompareTo(object other)
        {
            if (other == null)
                return 1;

            if (!(other is ValueTuple<T1, T2>))
                throw new ArgumentException();

            return CompareTo((ValueTuple<T1, T2>)other);
        }

        /// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
        /// <param name="other">An instance to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and <paramref name="other"/>.
        /// Returns less than zero if this instance is less than <paramref name="other"/>, zero if this
        /// instance is equal to <paramref name="other"/>, and greater than zero if this instance is greater
        /// than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(ValueTuple<T1, T2> other)
        {
            var c = Comparer<T1>.Default.Compare(Item1, other.Item1);
            if (c != 0)
                return c;
            return Comparer<T2>.Default.Compare(Item2, other.Item2);
        }

        /// <summary>
        /// Returns the hash code for the current <see cref="ValueTuple{T1,T2}"/> instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return ValueTuple.CombineHashCodes(
                Item1?.GetHashCode() ?? 0,
                Item2?.GetHashCode() ?? 0);
        }

        int GetHashCodeCore(IEqualityComparer comparer)
        {
            return ValueTuple.CombineHashCodes(
                comparer.GetHashCode(Item1),
                comparer.GetHashCode(Item2));
        }

        int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
        {
            return GetHashCodeCore(comparer);
        }

        /// <summary>
        /// Returns a string that represents the value of this <see cref="ValueTuple{T1,T2}"/> instance.
        /// </summary>
        /// <returns>The string representation of this <see cref="ValueTuple{T1,T2}"/> instance.</returns>
        /// <remarks>
        /// The string returned by this method takes the form <c>(Item1, Item2)</c>,
        /// where <c>Item1</c> and <c>Item2</c> represent the values of the <see cref="Item1"/>
        /// and <see cref="Item2"/> fields. If either field value is <see langword="null"/>,
        /// it is represented as <see cref="String.Empty"/>.
        /// </remarks>
        public override string ToString()
        {
            return $"({Item1}, {Item2})";
        }

        string IValueTupleInternal.ToStringEnd()
        {
            return $"{Item1}, {Item2})";
        }

        /// <summary>
        /// The number of positions in this data structure.
        /// </summary>
        int ITuple.Length => 2;

        /// <summary>
        /// Get the element at position <param name="index"/>.
        /// </summary>
        object ITuple.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
    }
}
