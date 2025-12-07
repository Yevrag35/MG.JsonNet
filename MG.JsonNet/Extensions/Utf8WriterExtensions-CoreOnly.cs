#if NET6_0_OR_GREATER
using MG.JsonNet.Internal;
using MG.JsonNet.Internal.Buffers;
using MG.JsonNet.Naming;

namespace MG.JsonNet.Extensions;

#nullable enable

public static partial class Utf8WriterExtensions
{
    /// <summary>
	/// Writes a formattable property name and its associated value to the specified <see cref="Utf8JsonWriter"/> using the provided naming policy.
	/// </summary>
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
}
#endif