#if !NETCOREAPP
using MG.JsonNet.Internal;
using MG.JsonNet.Naming;

namespace MG.JsonNet.Extensions;

public static partial class Utf8WriterExtensions
{
    /// <summary>
	/// Writes a property name and its associated numeric value to the JSON output using the specified naming policy.
	/// </summary>
	/// <param name="writer">The JSON writer to which the value is written.</param>
	/// <param name="policy">A reference to the working naming policy used to format the property name.</param>
	/// <param name="value">The numeric value to write.</param>
	/// <param name="propertyName">
	/// The name of the property, automatically formatted to remove any path prefixes.
	/// Captured via the caller argument expression.
	/// </param>
	/// <exception cref="ArgumentNullException"><paramref name="policy"/> is null.</exception>
	/// <inheritdoc cref="WorkingNamingPolicy.WritePropertyName(Utf8JsonWriter, ReadOnlySpan{char})" path="/exception"/>
    /// <inheritdoc cref="Utf8JsonWriter.WriteNumberValue(int)" path="/exception"/>
	public static void WriteNumber(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        int value,
        [CallerArgumentExpression(nameof(value))] string propertyName = "")
    {
		Guard.ThrowIfNull(policy);

        ReadOnlySpan<char> propName = FormatPropertyName(propertyName);

        policy.WritePropertyName(writer, propName);
        writer.WriteNumberValue(value);
    }
    /// <summary>
    /// Writes a property name and its associated numeric value to the JSON output using the specified naming policy.
    /// </summary>
    /// <param name="writer">The JSON writer to which the value is written.</param>
    /// <param name="policy">A reference to the working naming policy used to format the property name.</param>
    /// <param name="value">The numeric value to write.</param>
    /// <param name="propertyName">
    /// The name of the property, automatically formatted to remove any path prefixes.
    /// Captured via the caller argument expression.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="policy"/> is null.</exception>
    /// <inheritdoc cref="WorkingNamingPolicy.WritePropertyName(Utf8JsonWriter, ReadOnlySpan{char})" path="/exception"/>
    /// <inheritdoc cref="Utf8JsonWriter.WriteNumberValue(int)" path="/exception"/>
    public static void WriteNumber(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        long value,
        [CallerArgumentExpression(nameof(value))] string propertyName = "")
    {
        Guard.ThrowIfNull(policy);

        ReadOnlySpan<char> propName = FormatPropertyName(propertyName);

        policy.WritePropertyName(writer, propName);
        writer.WriteNumberValue(value);
    }

    /// <inheritdoc cref="WriteNumber(Utf8JsonWriter, WorkingNamingPolicy, ulong, string)"/>
    [DebuggerStepThrough]
	public static void WriteNumber(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        uint value,
        [CallerArgumentExpression(nameof(value))] string propertyName = "")
    {
        WriteNumber(writer, policy, (ulong)value, propertyName);
    }
    /// <summary>
    /// Writes a property name and its associated numeric value to the JSON output using the specified naming policy.
    /// </summary>
    /// <param name="writer">The JSON writer to which the value is written.</param>
    /// <param name="policy">A reference to the working naming policy used to format the property name.</param>
    /// <param name="value">The numeric value to write.</param>
    /// <param name="propertyName">
    /// The name of the property, automatically formatted to remove any path prefixes.
    /// Captured via the caller argument expression.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="policy"/> is null.</exception>
    /// <inheritdoc cref="WorkingNamingPolicy.WritePropertyName(Utf8JsonWriter, ReadOnlySpan{char})" path="/exception"/>
    /// <inheritdoc cref="Utf8JsonWriter.WriteNumberValue(int)" path="/exception"/>
    public static void WriteNumber(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        ulong value,
        [CallerArgumentExpression(nameof(value))] string propertyName = "")
    {
        Guard.ThrowIfNull(policy);

        ReadOnlySpan<char> propName = FormatPropertyName(propertyName);

        policy.WritePropertyName(writer, propName);
        writer.WriteNumberValue(value);
    }
    /// <summary>
    /// Writes a property name and its associated numeric value to the JSON output using the specified naming policy.
    /// </summary>
    /// <param name="writer">The JSON writer to which the value is written.</param>
    /// <param name="policy">A reference to the working naming policy used to format the property name.</param>
    /// <param name="value">The numeric value to write.</param>
    /// <param name="propertyName">
    /// The name of the property, automatically formatted to remove any path prefixes.
    /// Captured via the caller argument expression.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="policy"/> is null.</exception>
    /// <inheritdoc cref="WorkingNamingPolicy.WritePropertyName(Utf8JsonWriter, ReadOnlySpan{char})" path="/exception"/>
    /// <inheritdoc cref="Utf8JsonWriter.WriteNumberValue(int)" path="/exception"/>
    public static void WriteNumber(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        double value,
        [CallerArgumentExpression(nameof(value))] string propertyName = "")
    {
        Guard.ThrowIfNull(policy);

        ReadOnlySpan<char> propName = FormatPropertyName(propertyName);

        policy.WritePropertyName(writer, propName);
        writer.WriteNumberValue(value);
    }
    /// <summary>
    /// Writes a property name and its associated numeric value to the JSON output using the specified naming policy.
    /// </summary>
    /// <param name="writer">The JSON writer to which the value is written.</param>
    /// <param name="policy">A reference to the working naming policy used to format the property name.</param>
    /// <param name="value">The numeric value to write.</param>
    /// <param name="propertyName">
    /// The name of the property, automatically formatted to remove any path prefixes.
    /// Captured via the caller argument expression.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="policy"/> is null.</exception>
    /// <inheritdoc cref="WorkingNamingPolicy.WritePropertyName(Utf8JsonWriter, ReadOnlySpan{char})" path="/exception"/>
    /// <inheritdoc cref="Utf8JsonWriter.WriteNumberValue(int)" path="/exception"/>
    public static void WriteNumber(this Utf8JsonWriter writer,
        WorkingNamingPolicy policy,
        decimal value,
        [CallerArgumentExpression(nameof(value))] string propertyName = "")
    {
        Guard.ThrowIfNull(policy);

        ReadOnlySpan<char> propName = FormatPropertyName(propertyName);

        policy.WritePropertyName(writer, propName);
        writer.WriteNumberValue(value);
    }
}
#endif