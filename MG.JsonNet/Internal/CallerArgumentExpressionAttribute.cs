#if NETSTANDARD

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Runtime.CompilerServices;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Indicates that the expression passed to a method parameter should be captured as a string for diagnostic purposes.
/// </summary>
/// <remarks>
/// This attribute has no effect in .NET Standard. A value must be provided manually.
/// </remarks>
[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
internal sealed class CallerArgumentExpressionAttribute : Attribute
{
    public string ParameterName { get; }
    public CallerArgumentExpressionAttribute(string parameterName)
    {
        this.ParameterName = parameterName;
    }
}
#endif