using MG.JsonNet.Internal;

namespace MG.JsonNet.Extensions;

/// <summary>
/// Provides a set of extension methods for writing to <see cref="Utf8JsonWriter"/> instances.
/// </summary>
public static partial class Utf8WriterExtensions
{
    private const char DIVIDER = '.';
    private const int MAX_STACKALLOC = 256;

    /// <summary>
    /// Writes an empty array to the JSON writer.
    /// </summary>
    /// <remarks>
    /// Equivalent to calling <see cref="Utf8JsonWriter.WriteStartArray"/> and <see cref="Utf8JsonWriter.WriteEndArray"/>
    /// in succession.
    /// </remarks>
    /// <param name="writer">The JSON writer to write the empty array to.</param>
    /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
    /// <inheritdoc cref="Utf8JsonWriter.WriteStartArray" path="/exception"/>
    /// <inheritdoc cref="Utf8JsonWriter.WriteEndArray" path="/exception"/>
    public static void WriteEmptyArray(this Utf8JsonWriter writer)
	{
		Guard.ThrowIfNull(writer);

        writer.WriteStartArray();
		writer.WriteEndArray();
	}

    /// <summary>
    /// Writes an empty object to the JSON writer.
    /// </summary>
    /// <remarks>
    /// Equivalent to calling <see cref="Utf8JsonWriter.WriteStartObject"/> and <see cref="Utf8JsonWriter.WriteEndObject"/>
    /// in succession.
    /// </remarks>
    /// <param name="writer">The JSON writer to write the empty object to.</param>
    /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
    /// <inheritdoc cref="Utf8JsonWriter.WriteStartObject" path="/exception"/>
    /// <inheritdoc cref="Utf8JsonWriter.WriteEndObject" path="/exception"/>
    public static void WriteEmptyObject(this Utf8JsonWriter writer)
	{
        Guard.ThrowIfNull(writer);

        writer.WriteStartObject();
		writer.WriteEndObject();
	}

    private static ReadOnlySpan<char> FormatPropertyName(ReadOnlySpan<char> propertyName)
    {
		if (propertyName.TryLastIndexOf(DIVIDER, out int index))
		{
			propertyName = propertyName.Slice(index + 1);
        }

		return propertyName;
    }
}
