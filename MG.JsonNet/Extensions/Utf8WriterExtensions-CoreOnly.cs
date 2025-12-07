#if NETCOREAPP
using MG.JsonNet.Internal.Buffers;
using MG.JsonNet.Naming;

namespace MG.JsonNet.Extensions;

#nullable enable

public static partial class Utf8WriterExtensions
{
	public static void WriteFormattable<T>(this Utf8JsonWriter writer,
		WorkingNamingPolicy policy,
		T value,
		int maxLength,
        [CallerArgumentExpression(nameof(value))]
		string propertyName = "",
		ReadOnlySpan<char> format = default,
		IFormatProvider? provider = null)
        where T : ISpanFormattable
	{
		ReadOnlySpan<char> propName = FormatPropertyName(propertyName.AsSpan());
		policy.WritePropertyName(writer, propName);

		char[]? array = null;
		Span<char> buffer = maxLength <= MAX_STACKALLOC
			? stackalloc char[maxLength]
			: Rent.Array(ref array, maxLength);

		if (!value.TryFormat(buffer, out int charsWritten, format, provider))
		{
			throw new FormatException("The specified format is invalid.");
		}

		writer.WriteStringValue(buffer.Slice(0, charsWritten));
		Rent.Return(array);
	}
}
#endif