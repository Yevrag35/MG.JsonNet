using MG.JsonNet.Internal;
using MG.JsonNet.Naming;

namespace MG.JsonNet.Extensions;

public static partial class Utf8WriterExtensions
{
    /// <summary>
    /// Writes a property name and its associated value to the specified <see cref="Utf8JsonWriter"/> using the provided naming policy.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance to which the property and value will be written. Cannot be null.</param>
    /// <param name="policy">The <see cref="WorkingNamingPolicy"/> that determines how the property name is formatted and written. Cannot be
    /// null.</param>
    /// <param name="value">The string value to write as the property value.</param>
    /// <param name="propertyName">The name of the property to write. Captures the argument expression for <paramref name="value"/> if not specified.</param>
    /// <exception cref="ArgumentNullException"><paramref name="writer"/> or <paramref name="policy"/> is null.</exception>
    /// <inheritdoc cref="WorkingNamingPolicy.WritePropertyName(Utf8JsonWriter, ReadOnlySpan{char})" path="/exception"/>
    /// <inheritdoc cref="Utf8JsonWriter.WriteStringValue(ReadOnlySpan{char})" path="/exception"/>
    public static void WriteString(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        ReadOnlySpan<char> value,
        [CallerArgumentExpression(nameof(value))]
		string propertyName = "")
    {
        Guard.ThrowIfNull(policy);

        ReadOnlySpan<char> propName = FormatPropertyName(propertyName);
        policy.WritePropertyName(writer, propName);
        writer.WriteStringValue(value);
    }
}