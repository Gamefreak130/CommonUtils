namespace System.Runtime.CompilerServices
{
    /// <remarks>
    /// This attribute allows us to define readonly references, such as readonly structs.
    /// </remarks>

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    sealed partial class IsReadOnlyAttribute : Attribute { }
}
