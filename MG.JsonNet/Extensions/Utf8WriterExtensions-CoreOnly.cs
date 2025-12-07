#if NET6_0_OR_GREATER
using MG.JsonNet.Internal;
using MG.JsonNet.Internal.Buffers;
using MG.JsonNet.Naming;
using System.Numerics;

namespace MG.JsonNet.Extensions;

#nullable enable

public static partial class Utf8WriterExtensions
{
    /// <summary>
	/// Writes a property name and its associated value of <typeparamref name="T"/> that implements <see cref="ISpanFormattable"/> 
	/// to the specified <see cref="Utf8JsonWriter"/> using the provided naming policy.
	/// </summary>
	/// <param name="writer">The JSON writer to which the value is written.</param>
	/// <param name="policy">The <see cref="WorkingNamingPolicy"/> that determines how the property name is formatted and written. Cannot be null.</param>
	/// <param name="value">The formattable value to write as the property's value.</param>
	/// <param name="format">The format to apply to the value. If not specified, the default format is used.</param>
	/// <param name="maxLength">The maximum UTF-16 character length the formatted value can be.</param>
	/// <param name="provider">An optional format provider to use when formatting the value.</param>
	/// <param name="propertyName">The name of the property to write. Captures the argument expression for <paramref name="value"/> if not specified.</param>
    /// <exception cref="ArgumentNullException"><paramref name="policy"/> is null.</exception>
    /// <exception cref="FormatException"><paramref name="format"/> is invalid.</exception>
	/// <inheritdoc cref="WorkingNamingPolicy.WritePropertyName(Utf8JsonWriter, ReadOnlySpan{char})" path="/exception"/>
	/// <inheritdoc cref="Utf8JsonWriter.WriteStringValue(ReadOnlySpan{char})" path="/exception"/>
    public static void WriteFormattable<T>(this Utf8JsonWriter writer,
		WorkingNamingPolicy policy,
		T value,
		int maxLength,
		ReadOnlySpan<char> format = default,
		IFormatProvider? provider = null,
        [CallerArgumentExpression(nameof(value))]
        string propertyName = "")
			where T : ISpanFormattable
	{
        Guard.ThrowIfNull(policy);

        ReadOnlySpan<char> propName = FormatPropertyName(propertyName);
		policy.WritePropertyName(writer, propName);

		char[]? array = null;
		Span<char> buffer = maxLength <= MAX_STACKALLOC
			? stackalloc char[maxLength]
			: Rent.Array(ref array, maxLength);

		try
		{
            if (!value.TryFormat(buffer, out int charsWritten, format, provider))
            {
                throw new FormatException("The specified format is invalid.");
            }

            writer.WriteStringValue(buffer.Slice(0, charsWritten));
        }
		finally
		{
			Rent.Return(array);
		}
	}

	public static void WriteNumber<T>(this Utf8JsonWriter writer,
		WorkingNamingPolicy policy,
		T value,
		[CallerArgumentExpression(nameof(value))] string propertyName = "")
			where T : unmanaged, INumber<T>
    {
		Guard.ThrowIfNull(policy);

        ReadOnlySpan<char> propName = FormatPropertyName(propertyName);
        policy.WritePropertyName(writer, propName);

		WriteNumericValue(writer, value);
    }

	private static void WriteNumericValue<T>(Utf8JsonWriter writer, T value) where T : unmanaged, INumber<T>
	{
		bool isNegative = T.IsNegative(value);
        if (T.IsInteger(value))
        {
            if (isNegative)
			{
				writer.WriteNumberValue(long.CreateSaturating(value));
			}
			else
			{
				writer.WriteNumberValue(ulong.CreateSaturating(value));
            }
        }
		else
		{
			writer.WriteNumberValue(double.CreateSaturating(value));
        }
    }
}
#endif