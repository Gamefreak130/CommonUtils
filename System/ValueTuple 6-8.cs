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
    internal partial struct ValueTuple : IEquatable<ValueTuple>,
        IComparable, IComparable<ValueTuple>, IValueTupleInternal
    {
        /// <summary>Creates a new struct 6-tuple, or sextuple.</summary>
		/// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
		/// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
		/// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
		/// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
		/// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
		/// <typeparam name="T6">The type of the sixth component of the tuple.</typeparam>
		/// <param name="item1">The value of the first component of the tuple.</param>
		/// <param name="item2">The value of the second component of the tuple.</param>
		/// <param name="item3">The value of the third component of the tuple.</param>
		/// <param name="item4">The value of the fourth component of the tuple.</param>
		/// <param name="item5">The value of the fifth component of the tuple.</param>
		/// <param name="item6">The value of the sixth component of the tuple.</param>
		/// <returns>A 6-tuple (sextuple) whose value is (item1, item2, item3, item4, item5, item6).</returns>
		public static ValueTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) =>
            new ValueTuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);

        /// <summary>Creates a new struct 7-tuple, or septuple.</summary>
        /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth component of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh component of the tuple.</typeparam>
        /// <param name="item1">The value of the first component of the tuple.</param>
        /// <param name="item2">The value of the second component of the tuple.</param>
        /// <param name="item3">The value of the third component of the tuple.</param>
        /// <param name="item4">The value of the fourth component of the tuple.</param>
        /// <param name="item5">The value of the fifth component of the tuple.</param>
        /// <param name="item6">The value of the sixth component of the tuple.</param>
        /// <param name="item7">The value of the seventh component of the tuple.</param>
        /// <returns>A 7-tuple (septuple) whose value is (item1, item2, item3, item4, item5, item6, item7).</returns>
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) =>
            new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);

        /// <summary>Creates a new struct 8-tuple, or octuple.</summary>
        /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth component of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh component of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth component of the tuple.</typeparam>
        /// <param name="item1">The value of the first component of the tuple.</param>
        /// <param name="item2">The value of the second component of the tuple.</param>
        /// <param name="item3">The value of the third component of the tuple.</param>
        /// <param name="item4">The value of the fourth component of the tuple.</param>
        /// <param name="item5">The value of the fifth component of the tuple.</param>
        /// <param name="item6">The value of the sixth component of the tuple.</param>
        /// <param name="item7">The value of the seventh component of the tuple.</param>
        /// <param name="item8">The value of the eighth component of the tuple.</param>
        /// <returns>An 8-tuple (octuple) whose value is (item1, item2, item3, item4, item5, item6, item7, item8).</returns>
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) =>
            new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(item1, item2, item3, item4, item5, item6, item7, Create(item8));
    }

    /// <summary>
    /// Represents a 6-tuple, or sixtuple, as a value type.
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">The type of the tuple's second component.</typeparam>
    /// <typeparam name="T3">The type of the tuple's third component.</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
    /// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
    /// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
    [Serializable]
	[StructLayout(LayoutKind.Auto)]
	internal struct ValueTuple<T1,T2,T3,T4,T5,T6> : IEquatable<ValueTuple<T1,T2,T3,T4,T5,T6>>,
		IComparable, IComparable<ValueTuple<T1,T2,T3,T4,T5,T6>>, IValueTupleInternal
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> instance's first component.
		/// </summary>
		public readonly T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> instance's second component.
		/// </summary>
		public readonly T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> instance's third component.
		/// </summary>
		public readonly T3 Item3;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> instance's fourth component.
		/// </summary>
		public readonly T4 Item4;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> instance's fifth component.
		/// </summary>
		public readonly T5 Item5;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> instance's sixth component.
		/// </summary>
		public readonly T6 Item6;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		/// <param name="item5">The value of the tuple's fifth component.</param>
		/// <param name="item6">The value of the tuple's sixth component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="obj"/> parameter is considered to be equal to the current instance under the following conditions:
		/// <list type="bullet">
		///     <item><description>It is a <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> value type.</description></item>
		///     <item><description>Its components are of the same types as those of the current instance.</description></item>
		///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
		/// </list>
		/// </remarks>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1,T2,T3,T4,T5,T6> && Equals((ValueTuple<T1,T2,T3,T4,T5,T6>)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/>
		/// instance is equal to a specified <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/>.
		/// </summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified tuple; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="other"/> parameter is considered to be equal to the current instance if each of its fields
		/// are equal to that of the current instance, using the default comparer for that field's type.
		/// </remarks>
		public bool Equals(ValueTuple<T1,T2,T3,T4,T5,T6> other)
		{
			return
				EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
				EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
				EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
				EqualityComparer<T4>.Default.Equals(Item4, other.Item4) &&
				EqualityComparer<T5>.Default.Equals(Item5, other.Item5) &&
				EqualityComparer<T6>.Default.Equals(Item6, other.Item6);
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null) return 1;

			if (!(other is ValueTuple<T1,T2,T3,T4,T5,T6>))
				throw new ArgumentException();

			return CompareTo((ValueTuple<T1,T2,T3,T4,T5,T6>)other);
		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other"/>.
		/// Returns less than zero if this instance is less than <paramref name="other"/>, zero if this
		/// instance is equal to <paramref name="other"/>, and greater than zero if this instance is greater
		/// than <paramref name="other"/>.
		/// </returns>
		public int CompareTo(ValueTuple<T1,T2,T3,T4,T5,T6> other)
		{
			int c;

			c = Comparer<T1>.Default.Compare(Item1, other.Item1); if (c != 0) return c;
			c = Comparer<T2>.Default.Compare(Item2, other.Item2); if (c != 0) return c;
			c = Comparer<T3>.Default.Compare(Item3, other.Item3); if (c != 0) return c;
			c = Comparer<T4>.Default.Compare(Item4, other.Item4); if (c != 0) return c;
			c = Comparer<T5>.Default.Compare(Item5, other.Item5); if (c != 0) return c;

			return Comparer<T6>.Default.Compare(Item6, other.Item6);
		}

		/// <summary>
		/// Returns the hash code for the current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return ValueTuple.CombineHashCodes(
				Item1?.GetHashCode() ?? 0,
				Item2?.GetHashCode() ?? 0,
				Item3?.GetHashCode() ?? 0,
				Item4?.GetHashCode() ?? 0,
				Item5?.GetHashCode() ?? 0,
				Item6?.GetHashCode() ?? 0);
		}

		int GetHashCodeCore(IEqualityComparer comparer)
		{
			return ValueTuple.CombineHashCodes(
				comparer.GetHashCode(Item1),
				comparer.GetHashCode(Item2),
				comparer.GetHashCode(Item3),
				comparer.GetHashCode(Item4),
				comparer.GetHashCode(Item5),
				comparer.GetHashCode(Item6));
		}

		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return GetHashCodeCore(comparer);
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="ValueTuple{T1,T2,T3,T4,T5,T6}"/> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>(Item1, Item2, Item3, Item4, Item5, Item6)</c>.
		/// If any field value is <see langword="null"/>, it is represented as <see cref="String.Empty"/>.
		/// </remarks>
		public override string ToString()
		{
			return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6})";
		}

		string IValueTupleInternal.ToStringEnd()
		{
			return $"{Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6})";
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 6;

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
					case 5 : return Item6;
					default: throw new IndexOutOfRangeException();
				}
			}
		}
	}

	/// <summary>
	/// Represents a 7-tuple, or sentuple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
	/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
	/// <typeparam name="T7">The type of the tuple's seventh component.</typeparam>
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	internal struct ValueTuple<T1,T2,T3,T4,T5,T6,T7> : IEquatable<ValueTuple<T1,T2,T3,T4,T5,T6,T7>>,
		IComparable, IComparable<ValueTuple<T1,T2,T3,T4,T5,T6,T7>>, IValueTupleInternal
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance's first component.
		/// </summary>
		public readonly T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance's second component.
		/// </summary>
		public readonly T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance's third component.
		/// </summary>
		public readonly T3 Item3;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance's fourth component.
		/// </summary>
		public readonly T4 Item4;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance's fifth component.
		/// </summary>
		public readonly T5 Item5;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance's sixth component.
		/// </summary>
		public readonly T6 Item6;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance's seventh component.
		/// </summary>
		public readonly T7 Item7;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		/// <param name="item5">The value of the tuple's fifth component.</param>
		/// <param name="item6">The value of the tuple's sixth component.</param>
		/// <param name="item7">The value of the tuple's seventh component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
			Item7 = item7;
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="obj"/> parameter is considered to be equal to the current instance under the following conditions:
		/// <list type="bullet">
		///     <item><description>It is a <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> value type.</description></item>
		///     <item><description>Its components are of the same types as those of the current instance.</description></item>
		///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
		/// </list>
		/// </remarks>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1,T2,T3,T4,T5,T6,T7> && Equals((ValueTuple<T1,T2,T3,T4,T5,T6,T7>)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/>
		/// instance is equal to a specified <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/>.
		/// </summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified tuple; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="other"/> parameter is considered to be equal to the current instance if each of its fields
		/// are equal to that of the current instance, using the default comparer for that field's type.
		/// </remarks>
		public bool Equals(ValueTuple<T1,T2,T3,T4,T5,T6,T7> other)
		{
			return
				EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
				EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
				EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
				EqualityComparer<T4>.Default.Equals(Item4, other.Item4) &&
				EqualityComparer<T5>.Default.Equals(Item5, other.Item5) &&
				EqualityComparer<T6>.Default.Equals(Item6, other.Item6) &&
				EqualityComparer<T7>.Default.Equals(Item7, other.Item7);
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null) return 1;

			if (!(other is ValueTuple<T1,T2,T3,T4,T5,T6,T7>))
				throw new ArgumentException();

			return CompareTo((ValueTuple<T1,T2,T3,T4,T5,T6,T7>)other);
		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other"/>.
		/// Returns less than zero if this instance is less than <paramref name="other"/>, zero if this
		/// instance is equal to <paramref name="other"/>, and greater than zero if this instance is greater
		/// than <paramref name="other"/>.
		/// </returns>
		public int CompareTo(ValueTuple<T1,T2,T3,T4,T5,T6,T7> other)
		{
			int c;

			c = Comparer<T1>.Default.Compare(Item1, other.Item1); if (c != 0) return c;
			c = Comparer<T2>.Default.Compare(Item2, other.Item2); if (c != 0) return c;
			c = Comparer<T3>.Default.Compare(Item3, other.Item3); if (c != 0) return c;
			c = Comparer<T4>.Default.Compare(Item4, other.Item4); if (c != 0) return c;
			c = Comparer<T5>.Default.Compare(Item5, other.Item5); if (c != 0) return c;
			c = Comparer<T6>.Default.Compare(Item6, other.Item6); if (c != 0) return c;

			return Comparer<T7>.Default.Compare(Item7, other.Item7);
		}

		/// <summary>
		/// Returns the hash code for the current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return ValueTuple.CombineHashCodes(
				Item1?.GetHashCode() ?? 0,
				Item2?.GetHashCode() ?? 0,
				Item3?.GetHashCode() ?? 0,
				Item4?.GetHashCode() ?? 0,
				Item5?.GetHashCode() ?? 0,
				Item6?.GetHashCode() ?? 0,
				Item7?.GetHashCode() ?? 0);
		}

		int GetHashCodeCore(IEqualityComparer comparer)
		{
			return ValueTuple.CombineHashCodes(
				comparer.GetHashCode(Item1),
				comparer.GetHashCode(Item2),
				comparer.GetHashCode(Item3),
				comparer.GetHashCode(Item4),
				comparer.GetHashCode(Item5),
				comparer.GetHashCode(Item6),
				comparer.GetHashCode(Item7));
		}

		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return GetHashCodeCore(comparer);
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7}"/> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>(Item1, Item2, Item3, Item4, Item5, Item6, Item7)</c>.
		/// If any field value is <see langword="null"/>, it is represented as <see cref="String.Empty"/>.
		/// </remarks>
		public override string ToString()
		{
			return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7})";
		}

		string IValueTupleInternal.ToStringEnd()
		{
			return $"{Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7})";
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length => 7;

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
					case 5 : return Item6;
					case 6 : return Item7;
					default: throw new IndexOutOfRangeException();
				}
			}
		}
	}

	/// <summary>
	/// Represents an 8-tuple, or octuple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
	/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
	/// <typeparam name="T7">The type of the tuple's seventh component.</typeparam>
	/// <typeparam name="TRest">The type of the tuple's eighth component.</typeparam>
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
    internal struct ValueTuple<T1,T2,T3,T4,T5,T6,T7, TRest> : IEquatable<ValueTuple<T1,T2,T3,T4,T5,T6,T7, TRest>>,
		IComparable, IComparable<ValueTuple<T1,T2,T3,T4,T5,T6,T7, TRest>>, IValueTupleInternal
		where TRest : struct
	{
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance's first component.
		/// </summary>
		public readonly T1 Item1;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance's second component.
		/// </summary>
		public readonly T2 Item2;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance's third component.
		/// </summary>
		public readonly T3 Item3;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance's fourth component.
		/// </summary>
		public readonly T4 Item4;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance's fifth component.
		/// </summary>
		public readonly T5 Item5;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance's sixth component.
		/// </summary>
		public readonly T6 Item6;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance's seventh component.
		/// </summary>
		public readonly T7 Item7;
		/// <summary>
		/// The current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance's eighth component.
		/// </summary>
		public readonly TRest Rest;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		/// <param name="item5">The value of the tuple's fifth component.</param>
		/// <param name="item6">The value of the tuple's sixth component.</param>
		/// <param name="item7">The value of the tuple's seventh component.</param>
		/// <param name="rest">The value of the tuple's eight component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
		{
			if (!(rest is IValueTupleInternal))
				throw new ArgumentException();

			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
			Item7 = item7;
			Rest  = rest;
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="obj"/> parameter is considered to be equal to the current instance under the following conditions:
		/// <list type="bullet">
		///     <item><description>It is a <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> value type.</description></item>
		///     <item><description>Its components are of the same types as those of the current instance.</description></item>
		///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
		/// </list>
		/// </remarks>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1,T2,T3,T4,T5,T6,T7, TRest> && Equals((ValueTuple<T1,T2,T3,T4,T5,T6,T7, TRest>)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/>
		/// instance is equal to a specified <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/>.
		/// </summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns><see langword="true"/> if the current instance is equal to the specified tuple; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// The <paramref name="other"/> parameter is considered to be equal to the current instance if each of its fields
		/// are equal to that of the current instance, using the default comparer for that field's type.
		/// </remarks>
		public bool Equals(ValueTuple<T1,T2,T3,T4,T5,T6,T7, TRest> other)
		{
			return
				EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
				EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
				EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
				EqualityComparer<T4>.Default.Equals(Item4, other.Item4) &&
				EqualityComparer<T5>.Default.Equals(Item5, other.Item5) &&
				EqualityComparer<T6>.Default.Equals(Item6, other.Item6) &&
				EqualityComparer<T7>.Default.Equals(Item7, other.Item7) &&
				EqualityComparer<TRest>.Default.Equals(Rest, other.Rest);
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null) return 1;

			if (!(other is ValueTuple<T1,T2,T3,T4,T5,T6,T7, TRest>))
				throw new ArgumentException();

			return CompareTo((ValueTuple<T1,T2,T3,T4,T5,T6,T7, TRest>)other);
		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other"/>.
		/// Returns less than zero if this instance is less than <paramref name="other"/>, zero if this
		/// instance is equal to <paramref name="other"/>, and greater than zero if this instance is greater
		/// than <paramref name="other"/>.
		/// </returns>
		public int CompareTo(ValueTuple<T1,T2,T3,T4,T5,T6,T7, TRest> other)
		{
			int c;

			c = Comparer<T1>.Default.Compare(Item1, other.Item1); if (c != 0) return c;
			c = Comparer<T2>.Default.Compare(Item2, other.Item2); if (c != 0) return c;
			c = Comparer<T3>.Default.Compare(Item3, other.Item3); if (c != 0) return c;
			c = Comparer<T4>.Default.Compare(Item4, other.Item4); if (c != 0) return c;
			c = Comparer<T5>.Default.Compare(Item5, other.Item5); if (c != 0) return c;
			c = Comparer<T6>.Default.Compare(Item6, other.Item6); if (c != 0) return c;
			c = Comparer<T7>.Default.Compare(Item7, other.Item7); if (c != 0) return c;

			return Comparer<TRest>.Default.Compare(Rest, other.Rest);
		}

		/// <summary>
		/// Returns the hash code for the current <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			// We want to have a limited hash in this case.  We'll use the last 8 elements of the tuple
			var rest = Rest as IValueTupleInternal;

			if (rest == null)
			{
				return ValueTuple.CombineHashCodes(
					Item1?.GetHashCode() ?? 0,
					Item2?.GetHashCode() ?? 0,
					Item3?.GetHashCode() ?? 0,
					Item4?.GetHashCode() ?? 0,
					Item5?.GetHashCode() ?? 0,
					Item6?.GetHashCode() ?? 0,
					Item7?.GetHashCode() ?? 0);
			}

			int size = rest.Length;
			if (size >= 8)
				return rest.GetHashCode();

			// In this case, the rest member has less than 8 elements so we need to combine some our elements with the elements in rest
			var k = 8 - size;

			switch (k)
			{
				case 1:
					return ValueTuple.CombineHashCodes(
						Item7?.GetHashCode() ?? 0,
						rest.GetHashCode());
				case 2:
					return ValueTuple.CombineHashCodes(
						Item6?.GetHashCode() ?? 0,
						Item7?.GetHashCode() ?? 0,
						rest.GetHashCode());
				case 3:
					return ValueTuple.CombineHashCodes(
						Item5?.GetHashCode() ?? 0,
						Item6?.GetHashCode() ?? 0,
						Item7?.GetHashCode() ?? 0,
						rest.GetHashCode());
				case 4:
					return ValueTuple.CombineHashCodes(
						Item4?.GetHashCode() ?? 0,
						Item5?.GetHashCode() ?? 0,
						Item6?.GetHashCode() ?? 0,
						Item7?.GetHashCode() ?? 0,
						rest.GetHashCode());
				case 5:
					return ValueTuple.CombineHashCodes(
						Item3?.GetHashCode() ?? 0,
						Item4?.GetHashCode() ?? 0,
						Item5?.GetHashCode() ?? 0,
						Item6?.GetHashCode() ?? 0,
						Item7?.GetHashCode() ?? 0,
						rest.GetHashCode());
				case 6:
					return ValueTuple.CombineHashCodes(
						Item2?.GetHashCode() ?? 0,
						Item3?.GetHashCode() ?? 0,
						Item4?.GetHashCode() ?? 0,
						Item5?.GetHashCode() ?? 0,
						Item6?.GetHashCode() ?? 0,
						Item7?.GetHashCode() ?? 0,
						rest.GetHashCode());
				case 7:
				case 8:
					return ValueTuple.CombineHashCodes(
						Item1?.GetHashCode() ?? 0,
						Item2?.GetHashCode() ?? 0,
						Item3?.GetHashCode() ?? 0,
						Item4?.GetHashCode() ?? 0,
						Item5?.GetHashCode() ?? 0,
						Item6?.GetHashCode() ?? 0,
						Item7?.GetHashCode() ?? 0,
						rest.GetHashCode());
			}

			throw new InvalidOperationException("Missed all cases for computing ValueTuple hash code");
		}

		int GetHashCodeCore(IEqualityComparer comparer)
		{
			// We want to have a limited hash in this case.  We'll use the last 8 elements of the tuple
			var rest = Rest as IValueTupleInternal;

			if (rest == null)
			{
				return ValueTuple.CombineHashCodes(
					comparer.GetHashCode(Item1),
					comparer.GetHashCode(Item2),
					comparer.GetHashCode(Item3),
					comparer.GetHashCode(Item4),
					comparer.GetHashCode(Item5),
					comparer.GetHashCode(Item6),
					comparer.GetHashCode(Item7));
			}

			var size = rest.Length;
			if (size >= 8)
				return rest.GetHashCode(comparer);

			// In this case, the rest member has less than 8 elements so we need to combine some our elements with the elements in rest
			var k = 8 - size;

			switch (k)
			{
				case 1:
					return ValueTuple.CombineHashCodes(
						comparer.GetHashCode(Item7),
						rest.GetHashCode(comparer));
				case 2:
					return ValueTuple.CombineHashCodes(
						comparer.GetHashCode(Item6),
						comparer.GetHashCode(Item7),
						rest.GetHashCode(comparer));
				case 3:
					return ValueTuple.CombineHashCodes(
						comparer.GetHashCode(Item5),
						comparer.GetHashCode(Item6),
						comparer.GetHashCode(Item7),
						rest.GetHashCode(comparer));
				case 4:
					return ValueTuple.CombineHashCodes(
						comparer.GetHashCode(Item4),
						comparer.GetHashCode(Item5),
						comparer.GetHashCode(Item6),
						comparer.GetHashCode(Item7),
						rest.GetHashCode(comparer));
				case 5:
					return ValueTuple.CombineHashCodes(
						comparer.GetHashCode(Item3),
						comparer.GetHashCode(Item4),
						comparer.GetHashCode(Item5),
						comparer.GetHashCode(Item6),
						comparer.GetHashCode(Item7),
						rest.GetHashCode(comparer));
				case 6:
					return ValueTuple.CombineHashCodes(
						comparer.GetHashCode(Item2),
						comparer.GetHashCode(Item3),
						comparer.GetHashCode(Item4),
						comparer.GetHashCode(Item5),
						comparer.GetHashCode(Item6),
						comparer.GetHashCode(Item7),
						rest.GetHashCode(comparer));
				case 7:
				case 8:
					return ValueTuple.CombineHashCodes(
						comparer.GetHashCode(Item1),
						comparer.GetHashCode(Item2),
						comparer.GetHashCode(Item3),
						comparer.GetHashCode(Item4),
						comparer.GetHashCode(Item5),
						comparer.GetHashCode(Item6),
						comparer.GetHashCode(Item7),
						rest.GetHashCode(comparer));
			}

			throw new InvalidOperationException("Missed all cases for computing ValueTuple hash code");
		}

		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return GetHashCodeCore(comparer);
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="ValueTuple{T1,T2,T3,T4,T5,T6,T7, TRest}"/> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>(Item1, Item2, Item3, Item4, Item5, Item6, Item7, Rest)</c>.
		/// If any field value is <see langword="null"/>, it is represented as <see cref="String.Empty"/>.
		/// </remarks>
		public override string ToString()
		{
			var rest = Rest as IValueTupleInternal;

			if (rest == null)
				return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7}, {Rest})";
			else
				return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7}, {rest.ToStringEnd()}";
		}

		string IValueTupleInternal.ToStringEnd()
		{
			var rest = Rest as IValueTupleInternal;

			if (rest == null)
				return $"{Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7}, {Rest})";
			else
				return $"{Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7}, {rest.ToStringEnd()}";
		}

		/// <summary>
		/// The number of positions in this data structure.
		/// </summary>
		int ITuple.Length
		{
			get
			{
				var rest = Rest as IValueTupleInternal;
				return rest == null ? 8 : 7 + rest.Length;
			}
		}

		/// <summary>
		/// Get the element at position <param name="index"/>.
		/// </summary>
		object ITuple.this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return Item1;
					case 1: return Item2;
					case 2: return Item3;
					case 3: return Item4;
					case 4: return Item5;
					case 5: return Item6;
					case 6: return Item7;
				}

				var rest = Rest as IValueTupleInternal;

				if (rest == null)
				{
					if (index == 7)
						return Rest;

					throw new IndexOutOfRangeException();
				}

				return rest[index - 7];
			}
		}
	}
}
