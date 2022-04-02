namespace System
{
    namespace Runtime.CompilerServices
    {
        using Linq;

        public static partial class RuntimeHelpers
        {
            /*public static Span<int> get_IndexerExtension(this int[] array, Range range) 
                => array.Slice(range);

            public static string get_IndexerExtension(this string s, Range range) 
                => s.Substring(range);

            public static Span<T> Slice<T>(this T[] array, Range range)
                => array.AsSpan().Slice(range);

            public static string Substring(this string s, Range range)
            {
                var (start, length) = GetStartAndLength(range, s.Length);
                return s.Substring(start, length);
            }

            private static (int start, int length) GetStartAndLength(Range range, int entityLength)
            {
                var start = range.Start.IsFromEnd ? entityLength - range.Start.Value : range.Start.Value;
                var end = range.End.IsFromEnd ? entityLength - range.End.Value : range.End.Value;
                var length = end - start;

                return (start, length);
            }*/

            /// <summary>
            /// Slices the specified array using the specified range.
            /// </summary>
            public static T[] GetSubArray<T>(T[] array, Range range)
            {
                if (array == null)
                {
                    throw new ArgumentNullException(nameof(array));
                }

                (int offset, int length) = range.GetOffsetAndLength(array.Length);

                if (default(T) != null || typeof(T[]) == array.GetType())
                {
                    // We know the type of the array to be exactly T[].

                    if (length == 0)
                    {
                        return (T[])Enumerable.Empty<T>();
                    }

                    var dest = new T[length];
                    Array.Copy(array, offset, dest, 0, length);
                    return dest;
                }
                else
                {
                    // The array is actually a U[] where U:T.
                    T[] dest = (T[])Array.CreateInstance(array.GetType().GetElementType(), length);
                    Array.Copy(array, offset, dest, 0, length);
                    return dest;
                }
            }
        }
    }

    /// <summary>Represent a range has start and end indexes.</summary>
    /// <remarks>
    /// Range is used by the C# compiler to support the range syntax.
    /// <code>
    /// int[] someArray = new int[5] { 1, 2, 3, 4, 5 };
    /// int[] subArray1 = someArray[0..2]; // { 1, 2 }
    /// int[] subArray2 = someArray[1..^0]; // { 2, 3, 4, 5 }
    /// </code>
    /// </remarks>
    public readonly struct Range : IEquatable<Range>
    {
        /// <summary>Represent the inclusive start index of the Range.</summary>
        public Index Start { get; }

        /// <summary>Represent the exclusive end index of the Range.</summary>
        public Index End { get; }

        /// <summary>Construct a Range object using the start and end indexes.</summary>
        /// <param name="start">Represent the inclusive start index of the range.</param>
        /// <param name="end">Represent the exclusive end index of the range.</param>
        public Range(Index start, Index end)
        {
            Start = start;
            End = end;
        }

        /// <summary>Indicates whether the current Range object is equal to another object of the same type.</summary>
        /// <param name="value">An object to compare with this object</param>
        public override bool Equals(object value) =>
            value is Range r &&
            r.Start.Equals(Start) &&
            r.End.Equals(End);

        /// <summary>Indicates whether the current Range object is equal to another Range object.</summary>
        /// <param name="other">An object to compare with this object</param>
        public bool Equals(Range other) => other.Start.Equals(Start) && other.End.Equals(End);

        /// <summary>Returns the hash code for this instance.</summary>
        public override int GetHashCode()
        {
            return Combine(Start.GetHashCode(), End.GetHashCode());
        }

        /// <summary>Converts the value of the current Range object to its equivalent string representation.</summary>
        public override string ToString()
        {
            string str = ""; // 2 for "..", then for each index 1 for '^' and 10 for longest possible uint

            if (Start.IsFromEnd)
            {
                str += '^';
            }

            str += (uint)Start.Value;
            str += "..";

            if (End.IsFromEnd)
            {
                str += '^';
            }

            str += (uint)End.Value;

            return str;
        }

        /// <summary>Create a Range object starting from start index to the end of the collection.</summary>
        public static Range StartAt(Index start) => new Range(start, Index.End);

        /// <summary>Create a Range object starting from first element in the collection to the end Index.</summary>
        public static Range EndAt(Index end) => new Range(Index.Start, end);

        /// <summary>Create a Range object starting from first element to the end.</summary>
        public static Range All => new Range(Index.Start, Index.End);

        /// <summary>Calculate the start offset and length of range object using a collection length.</summary>
        /// <param name="length">The length of the collection that the range will be used with. length has to be a positive value.</param>
        /// <remarks>
        /// For performance reason, we don't validate the input length parameter against negative values.
        /// It is expected Range will be used with collections which always have non negative length/count.
        /// We validate the range is inside the length scope though.
        /// </remarks>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Offset, int Length) GetOffsetAndLength(int length)
        {
            int start;
            Index startIndex = Start;
            if (startIndex.IsFromEnd)
                start = length - startIndex.Value;
            else
                start = startIndex.Value;

            int end;
            Index endIndex = End;
            if (endIndex.IsFromEnd)
                end = length - endIndex.Value;
            else
                end = endIndex.Value;

            if ((uint)end > (uint)length || (uint)start > (uint)end)
            {
                throw new ArgumentOutOfRangeException();
            }

            return (start, end - start);
        }

        static int Combine<T1, T2>(T1 value1, T2 value2)
        {
            int hc1 = value1?.GetHashCode() ?? 0;
            int hc2 = value2?.GetHashCode() ?? 0;

            uint rol5 = ((uint)hc1 << 5) | ((uint)hc1 >> 27);
            return ((int)rol5 + hc1) ^ hc2;
        }
    }
}