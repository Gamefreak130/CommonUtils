namespace System.Runtime.CompilerServices
{
    using System.Collections.Generic;

    /// <summary>
    /// This interface is required for types that want to be indexed into by dynamic patterns.
    /// </summary>
    internal interface ITuple
    {
        /// <summary>
        /// The number of positions in this data structure.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Get the element at position <param name="index"/>.
        /// </summary>
        object this[int index] { get; }
    }

    /// <summary>
    /// Indicates that the use of <see cref="T:System.ValueTuple" /> on a member is meant to be treated as a tuple with element names.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    internal sealed class TupleElementNamesAttribute : Attribute
    {
        readonly string[] _transformNames;

        /// <summary>
        /// Specifies, in a pre-order depth-first traversal of a type's
        /// construction, which <see cref="T:System.ValueTuple" /> elements are
        /// meant to carry element names.
        /// </summary>
        public IList<string> TransformNames => _transformNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Runtime.CompilerServices.TupleElementNamesAttribute" /> class.
        /// </summary>
        /// <param name="transformNames">
        /// Specifies, in a pre-order depth-first traversal of a type's
        /// construction, which <see cref="T:System.ValueTuple" /> occurrences are
        /// meant to carry element names.
        /// </param>
        /// <remarks>
        /// This constructor is meant to be used on types that contain an
        /// instantiation of <see cref="T:System.ValueTuple" /> that contains
        /// element names.  For instance, if <c>C</c> is a generic type with
        /// two type parameters, then a use of the constructed type <c>C{<see cref="T:System.ValueTuple`2" />, <see cref="T:System.ValueTuple`3" /></c> might be intended to
        /// treat the first type argument as a tuple with element names and the
        /// second as a tuple without element names. In which case, the
        /// appropriate attribute specification should use a
        /// <c>transformNames</c> value of <c>{ "name1", "name2", null, null,
        /// null }</c>.
        /// </remarks>
        public TupleElementNamesAttribute(string[] transformNames)
        {
            if (transformNames == null)
                throw new ArgumentNullException(nameof(transformNames));

            _transformNames = transformNames;
        }
    }
}
