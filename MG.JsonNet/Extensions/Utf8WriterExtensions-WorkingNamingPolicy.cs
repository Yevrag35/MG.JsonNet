using MG.JsonNet.Internal;
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
    /// <exception cref="ArgumentNullException"><paramref name="writer"/> or <paramref name="policy"/> is null.</exception>
    /// <inheritdoc cref="WorkingNamingPolicy.WritePropertyName(Utf8JsonWriter, ReadOnlySpan{char})" path="/exception"/>
    /// <inheritdoc cref="Utf8JsonWriter.WriteBoolean(ReadOnlySpan{char}, bool)" path="/exception"/>
    public static void WriteBoolean(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        bool value,
        [CallerArgumentExpression(nameof(value))]
		string propertyName = "")
    {
        Guard.ThrowIfNull(policy);
        // Writer null check is done in WritePropertyName.

        ReadOnlySpan<char> propName = FormatPropertyName(propertyName);
        policy.WritePropertyName(writer, propName);
        writer.WriteBooleanValue(value);
    }
    /// <summary>
	/// Writes a property name and its associated nullable boolean value to the specified <see cref="Utf8JsonWriter"/> using the provided naming policy.
	/// </summary>
	/// <param name="writer">The JSON writer to which the value is written.</param>
	/// <param name="policy">A reference to the working naming policy to format the property name.</param>
	/// <param name="utf8PropertyName">The UTF-8 encoded property name.</param>
	/// <param name="value">The nullable boolean value to write. If <see langword="null"/>, a <c>null</c> is written to the writer.</param>
    /// <exception cref="ArgumentNullException"><paramref name="writer"/> or <paramref name="policy"/> is null.</exception>
    /// <inheritdoc cref="WorkingNamingPolicy.WritePropertyName(Utf8JsonWriter, ReadOnlySpan{byte})" path="/exception"/>
	public static void WriteBoolean(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        ReadOnlySpan<byte> utf8PropertyName,
        bool? value)
    {
        Guard.ThrowIfNull(policy);

        policy.WritePropertyName(writer, utf8PropertyName);

        if (value.HasValue)
            writer.WriteBooleanValue(value.Value);
        else
            writer.WriteNullValue();
    }

    /// <summary>
    /// Writes a property name and its associated value to the specified <see cref="Utf8JsonWriter"/> using the provided naming policy.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance to which the property and value will be written. Cannot be null.</param>
    /// <param name="policy">The <see cref="WorkingNamingPolicy"/> that determines how the property name is formatted and written. Cannot be
    /// null.</param>
    /// <param name="value">The string value to write as the property value.</param>
    /// <param name="propertyName">The expression representing the property name. If not specified, an empty string is used.</param>
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