using MG.JsonNet.Naming;

namespace MG.JsonNet.Extensions;

public static partial class Utf8WriterExtensions
{
    /// <summary>
    /// Writes a boolean property to the specified <see cref="Utf8JsonWriter"/> using the provided naming policy.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance to which the property will be written.</param>
    /// <param name="policy">The naming policy used to format and write the property name.</param>
    /// <param name="value">The boolean value to write as the property's value.</param>
    /// <param name="propertyName">The name of the property to write. If <see langword="null"/> or empty, the naming policy may determine the
    /// property name.</param>
    public static void WriteBoolean(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        bool value,
        [CallerArgumentExpression(nameof(value))]
		string? propertyName = null)
    {
        ReadOnlySpan<char> propName = FormatPropertyName(GetPropertyName(propertyName));
        policy.WritePropertyName(writer, propName);
        writer.WriteBooleanValue(value);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="policy"></param>
    /// <param name="value"></param>
    /// <param name="propertyName"></param>
    public static void WriteString(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        ReadOnlySpan<char> value,
        [CallerArgumentExpression(nameof(value))]
		string? propertyName = null)
    {
        ReadOnlySpan<char> propName = FormatPropertyName(GetPropertyName(propertyName));
        policy.WritePropertyName(writer, propName);
        writer.WriteStringValue(value);
    }
}