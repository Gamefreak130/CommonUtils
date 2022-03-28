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
    public partial struct ValueTuple : IEquatable<ValueTuple>,
        IComparable, IComparable<ValueTuple>, IValueTupleInternal
    {
        /// <summary>Creates a new struct 3-tuple, or triple.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
		/// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <param name="item2">The value of the second component of the tuple.</param>
		/// <param name="item3">The value of the third component of the tuple.</param>
		/// <returns>A 3-tuple (triple) whose value is (item1, item2, item3).</returns>
		public static ValueTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) =>
            new ValueTuple<T1, T2, T3>(item1, item2, item3);

        /// <summary>Creates a new struct 4-tuple, or quadruple.</summary>
        /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
        /// <param name="item1">The value of the first component of the tuple.</param>
        /// <param name="item2">The value of the second component of the tuple.</param>
        /// <param name="item3">The value of the third component of the tuple.</param>
        /// <param name="item4">The value of the fourth component of the tuple.</param>
        /// <returns>A 4-tuple (quadruple) whose value is (item1, item2, item3, item4).</returns>
        public static ValueTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) =>
            new ValueTuple<T1, T2, T3, T4>(item1, item2, item3, item4);

        /// <summary>Creates a new struct 5-tuple, or quintuple.</summary>
        /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
        /// <param name="item1">The value of the first component of the tuple.</param>
        /// <param name="item2">The value of the second component of the tuple.</param>
        /// <param name="item3">The value of the third component of the tuple.</param>
        /// <param name="item4">The value of the fourth component of the tuple.</param>
        /// <param name="item5">The value of the fifth component of the tuple.</param>
        /// <returns>A 5-tuple (quintuple) whose value is (item1, item2, item3, item4, item5).</returns>
        public static ValueTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) =>
            new ValueTuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
    }

    /// <summary>
    /// Represents a 3-tuple, or triple, as a value type.
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">The type of the tuple's second component.</typeparam>
    /// <typeparam name="T3">The type of the tuple's third component.</typeparam>
    [Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1,T2,T3> : IEquatable<ValueTuple<T1,T2,T3>>,
		IComparable, IComparable<ValueTuple<T1,T2,T3>>, IValueTupleInternal
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3}"/> instance's first component.
		/// </summary>
		public readonly T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3}"/> instance's second component.
		/// </summary>
		public readonly T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3}"/> instance's third component.
		/// </summary>
		public readonly T3 Item3;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1,T2,T3}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3}"/> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="obj"/> parameter is considered to be equal to the current instance under the following conditions:
		/// <list type="bullet">
		///     <item><description>It is a <see cref="ValueTuple{T1,T2,T3}"/> value type.</description></item>
		///     <item><description>Its components are of the same types as those of the current instance.</description></item>
		///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
		/// </list>
		/// </remarks>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1,T2,T3> && Equals((ValueTuple<T1,T2,T3>)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3}"/>
		/// instance is equal to a specified <see cref="ValueTuple{T1,T2,T3}"/>.
		/// </summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified tuple; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="other"/> parameter is considered to be equal to the current instance if each of its fields
		/// are equal to that of the current instance, using the default comparer for that field's type.
		/// </remarks>
		public bool Equals(ValueTuple<T1,T2,T3> other)
		{
			return
				EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
				EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
				EqualityComparer<T3>.Default.Equals(Item3, other.Item3);
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null) return 1;

			if (!(other is ValueTuple<T1,T2,T3>))
				throw new ArgumentException();

			return CompareTo((ValueTuple<T1,T2,T3>)other);
		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other"/>.
		/// Returns less than zero if this instance is less than <paramref name="other"/>, zero if this
		/// instance is equal to <paramref name="other"/>, and greater than zero if this instance is greater
		/// than <paramref name="other"/>.
		/// </returns>
		public int CompareTo(ValueTuple<T1,T2,T3> other)
		{
			int c;

			c = Comparer<T1>.Default.Compare(Item1, other.Item1); if (c != 0) return c;
			c = Comparer<T2>.Default.Compare(Item2, other.Item2); if (c != 0) return c;

			return Comparer<T3>.Default.Compare(Item3, other.Item3);
		}

		/// <summary>
		/// Returns the hash code for the current <see cref="ValueTuple{T1,T2,T3}"/> instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return ValueTuple.CombineHashCodes(
				Item1?.GetHashCode() ?? 0,
				Item2?.GetHashCode() ?? 0,
				Item3?.GetHashCode() ?? 0);
		}

		int GetHashCodeCore(IEqualityComparer comparer)
		{
			return ValueTuple.CombineHashCodes(
				comparer.GetHashCode(Item1),
				comparer.GetHashCode(Item2),
				comparer.GetHashCode(Item3));
		}

		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return GetHashCodeCore(comparer);
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="ValueTuple{T1,T2,T3}"/> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="ValueTuple{T1,T2,T3}"/> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>(Item1, Item2, Item3)</c>.
		/// If any field value is <see langword="null"/>, it is represented as <see cref="String.Empty"/>.
		/// </remarks>
		public override string ToString()
		{
			return $"({Item1}, {Item2}, {Item3})";
		}

		string IValueTupleInternal.ToStringEnd()
		{
			return $"{Item1}, {Item2}, {Item3})";
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 3;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index]
		{
			get
			{
				switch (index)
				{
					case 0 : return Item1;
					case 1 : return Item2;
					case 2 : return Item3;
					default: throw new IndexOutOfRangeException();
				}
			}
		}
	}

	/// <summary>
	/// Represents a 4-tuple, or quadruple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1,T2,T3,T4> : IEquatable<ValueTuple<T1,T2,T3,T4>>,
		IComparable, IComparable<ValueTuple<T1,T2,T3,T4>>, IValueTupleInternal
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4}"/> instance's first component.
		/// </summary>
		public readonly T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4}"/> instance's second component.
		/// </summary>
		public readonly T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4}"/> instance's third component.
		/// </summary>
		public readonly T3 Item3;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4}"/> instance's fourth component.
		/// </summary>
		public readonly T4 Item4;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1,T2,T3,T4}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3,T4}"/> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="obj"/> parameter is considered to be equal to the current instance under the following conditions:
		/// <list type="bullet">
		///     <item><description>It is a <see cref="ValueTuple{T1,T2,T3,T4}"/> value type.</description></item>
		///     <item><description>Its components are of the same types as those of the current instance.</description></item>
		///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
		/// </list>
		/// </remarks>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1,T2,T3,T4> && Equals((ValueTuple<T1,T2,T3,T4>)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3,T4}"/>
		/// instance is equal to a specified <see cref="ValueTuple{T1,T2,T3,T4}"/>.
		/// </summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified tuple; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="other"/> parameter is considered to be equal to the current instance if each of its fields
		/// are equal to that of the current instance, using the default comparer for that field's type.
		/// </remarks>
		public bool Equals(ValueTuple<T1,T2,T3,T4> other)
		{
			return
				EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
				EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
				EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
				EqualityComparer<T4>.Default.Equals(Item4, other.Item4);
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null) return 1;

			if (!(other is ValueTuple<T1,T2,T3,T4>))
				throw new ArgumentException();

			return CompareTo((ValueTuple<T1,T2,T3,T4>)other);
		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other"/>.
		/// Returns less than zero if this instance is less than <paramref name="other"/>, zero if this
		/// instance is equal to <paramref name="other"/>, and greater than zero if this instance is greater
		/// than <paramref name="other"/>.
		/// </returns>
		public int CompareTo(ValueTuple<T1,T2,T3,T4> other)
		{
			int c;

			c = Comparer<T1>.Default.Compare(Item1, other.Item1); if (c != 0) return c;
			c = Comparer<T2>.Default.Compare(Item2, other.Item2); if (c != 0) return c;
			c = Comparer<T3>.Default.Compare(Item3, other.Item3); if (c != 0) return c;

			return Comparer<T4>.Default.Compare(Item4, other.Item4);
		}

		/// <summary>
		/// Returns the hash code for the current <see cref="ValueTuple{T1,T2,T3,T4}"/> instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return ValueTuple.CombineHashCodes(
				Item1?.GetHashCode() ?? 0,
				Item2?.GetHashCode() ?? 0,
				Item3?.GetHashCode() ?? 0,
				Item4?.GetHashCode() ?? 0);
		}

		int GetHashCodeCore(IEqualityComparer comparer)
		{
			return ValueTuple.CombineHashCodes(
				comparer.GetHashCode(Item1),
				comparer.GetHashCode(Item2),
				comparer.GetHashCode(Item3),
				comparer.GetHashCode(Item4));
		}

		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return GetHashCodeCore(comparer);
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="ValueTuple{T1,T2,T3,T4}"/> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="ValueTuple{T1,T2,T3,T4}"/> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>(Item1, Item2, Item3, Item4)</c>.
		/// If any field value is <see langword="null"/>, it is represented as <see cref="String.Empty"/>.
		/// </remarks>
		public override string ToString()
		{
			return $"({Item1}, {Item2}, {Item3}, {Item4})";
		}

		string IValueTupleInternal.ToStringEnd()
		{
			return $"{Item1}, {Item2}, {Item3}, {Item4})";
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 4;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index]
		{
			get
			{
				switch (index)
				{
					case 0 : return Item1;
					case 1 : return Item2;
					case 2 : return Item3;
					case 3 : return Item4;
					default: throw new IndexOutOfRangeException();
				}
			}
		}
	}

	/// <summary>
	/// Represents a 5-tuple, or quintuple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1,T2,T3,T4,T5> : IEquatable<ValueTuple<T1,T2,T3,T4,T5>>,
		IComparable, IComparable<ValueTuple<T1,T2,T3,T4,T5>>, IValueTupleInternal
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> instance's first component.
		/// </summary>
		public readonly T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> instance's second component.
		/// </summary>
		public readonly T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> instance's third component.
		/// </summary>
		public readonly T3 Item3;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> instance's fourth component.
		/// </summary>
		public readonly T4 Item4;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> instance's fifth component.
		/// </summary>
		public readonly T5 Item5;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		/// <param name="item5">The value of the tuple's fifth component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="obj"/> parameter is considered to be equal to the current instance under the following conditions:
		/// <list type="bullet">
		///     <item><description>It is a <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> value type.</description></item>
		///     <item><description>Its components are of the same types as those of the current instance.</description></item>
		///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
		/// </list>
		/// </remarks>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1,T2,T3,T4,T5> && Equals((ValueTuple<T1,T2,T3,T4,T5>)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3,T4,T5}"/>
		/// instance is equal to a specified <see cref="ValueTuple{T1,T2,T3,T4,T5}"/>.
		/// </summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified tuple; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="other"/> parameter is considered to be equal to the current instance if each of its fields
		/// are equal to that of the current instance, using the default comparer for that field's type.
		/// </remarks>
		public bool Equals(ValueTuple<T1,T2,T3,T4,T5> other)
		{
			return
				EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
				EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
				EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
				EqualityComparer<T4>.Default.Equals(Item4, other.Item4) &&
				EqualityComparer<T5>.Default.Equals(Item5, other.Item5);
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null) return 1;

			if (!(other is ValueTuple<T1,T2,T3,T4,T5>))
				throw new ArgumentException();

			return CompareTo((ValueTuple<T1,T2,T3,T4,T5>)other);
		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other"/>.
		/// Returns less than zero if this instance is less than <paramref name="other"/>, zero if this
		/// instance is equal to <paramref name="other"/>, and greater than zero if this instance is greater
		/// than <paramref name="other"/>.
		/// </returns>
		public int CompareTo(ValueTuple<T1,T2,T3,T4,T5> other)
		{
			int c;

			c = Comparer<T1>.Default.Compare(Item1, other.Item1); if (c != 0) return c;
			c = Comparer<T2>.Default.Compare(Item2, other.Item2); if (c != 0) return c;
			c = Comparer<T3>.Default.Compare(Item3, other.Item3); if (c != 0) return c;
			c = Comparer<T4>.Default.Compare(Item4, other.Item4); if (c != 0) return c;

			return Comparer<T5>.Default.Compare(Item5, other.Item5);
		}

		/// <summary>
		/// Returns the hash code for the current <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return ValueTuple.CombineHashCodes(
				Item1?.GetHashCode() ?? 0,
				Item2?.GetHashCode() ?? 0,
				Item3?.GetHashCode() ?? 0,
				Item4?.GetHashCode() ?? 0,
				Item5?.GetHashCode() ?? 0);
		}

		int GetHashCodeCore(IEqualityComparer comparer)
		{
			return ValueTuple.CombineHashCodes(
				comparer.GetHashCode(Item1),
				comparer.GetHashCode(Item2),
				comparer.GetHashCode(Item3),
				comparer.GetHashCode(Item4),
				comparer.GetHashCode(Item5));
		}

		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return GetHashCodeCore(comparer);
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="ValueTuple{T1,T2,T3,T4,T5}"/> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>(Item1, Item2, Item3, Item4, Item5)</c>.
		/// If any field value is <see langword="null"/>, it is represented as <see cref="String.Empty"/>.
		/// </remarks>
		public override string ToString()
		{
			return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5})";
		}

		string IValueTupleInternal.ToStringEnd()
		{
			return $"{Item1}, {Item2}, {Item3}, {Item4}, {Item5})";
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 5;

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index]
		{
			get
			{
				switch (index)
				{
					case 0 : return Item1;
					case 1 : return Item2;
					case 2 : return Item3;
					case 3 : return Item4;
					case 4 : return Item5;
					default: throw new IndexOutOfRangeException();
				}
			}
		}
	}
}
