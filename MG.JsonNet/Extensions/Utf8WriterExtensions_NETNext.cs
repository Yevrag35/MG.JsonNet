#if NET6_0_OR_GREATER
using MG.JsonNet.Naming;

namespace MG.JsonNet.Extensions;

#nullable enable

public static partial class Utf8WriterExtensions
{
	private const int MAX_STACKALLOC = 256;

	public static void WriteBoolean(this Utf8JsonWriter writer,
		ref readonly WorkingNamingPolicy policy,
		bool value,
		[CallerArgumentExpression(nameof(value))] string propertyName = "")
	{
		ReadOnlySpan<char> propName = FormatPropertyName(propertyName.AsSpan());
		policy.WritePropertyName(writer, propName);
		writer.WriteBooleanValue(value);
	}
	public static void WriteFormattable<T>(this Utf8JsonWriter writer,
		ref readonly WorkingNamingPolicy policy,
		T value,
		int maxLength,
		[CallerArgumentExpression(nameof(value))] string propertyName = "",
		ReadOnlySpan<char> format = default,
		IFormatProvider? provider = null)
		where T : ISpanFormattable
	{
		ReadOnlySpan<char> propName = FormatPropertyName(propertyName.AsSpan());
		policy.WritePropertyName(writer, propName);

		char[]? array = null;
		bool isRented = false;
		Span<char> buffer = maxLength <= MAX_STACKALLOC
			? stackalloc char[maxLength]
			: Rent(maxLength, ref array, ref isRented);

		if (!value.TryFormat(buffer, out int charsWritten, format, provider))
		{
			throw new FormatException("The specified format is invalid.");
		}

		writer.WriteStringValue(buffer.Slice(0, charsWritten));
		if (isRented)
		{
			ArrayPool<char>.Shared.Return(array!, clearArray: policy.ClearBuffers);
		}
	}
	public static void WriteString(this Utf8JsonWriter writer,
		ref readonly WorkingNamingPolicy policy,
		ReadOnlySpan<char> value,
		[CallerArgumentExpression(nameof(value))] string propertyName = "")
	{
		ReadOnlySpan<char> propName = FormatPropertyName(propertyName.AsSpan());
		policy.WritePropertyName(writer, propName);
		writer.WriteStringValue(value);
	}

	private const char DIVIDER = '.';
#if NET8_0_OR_GREATER
	private static readonly SearchValues<char> _divider = SearchValues.Create([DIVIDER]);
#endif

	private static int GetLastIndex(ReadOnlySpan<char> propertyName)
	{
		int index =
#if NET8_0_OR_GREATER
			propertyName.LastIndexOfAny(_divider);
#else
            propertyName.LastIndexOf(DIVIDER);
#endif

		return index == -1 || index >= propertyName.Length
			? -1
			: index;
	}

	private static ReadOnlySpan<char> FormatPropertyName(ReadOnlySpan<char> propertyName)
	{
		int lastIndex = GetLastIndex(propertyName);
		return lastIndex == -1
			? propertyName
			: propertyName.Slice(lastIndex + 1);
	}

	private static Span<T> Rent<T>(int length, [NotNull] ref T[]? array, ref bool isRented) where T : unmanaged
	{
		array = ArrayPool<T>.Shared.Rent(length);
		isRented = true;
		return array.AsSpan(0, length);
	}
}
#endif