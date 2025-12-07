namespace MG.JsonNet.Internal.Buffers;

/// <summary>
/// Provides utility methods for encoding and decoding between UTF-8 byte sequences and Unicode character spans.
/// </summary>
/// <remarks>This class is intended for internal use to efficiently convert between UTF-8 encoded data and Unicode
/// characters using spans.</remarks>
internal static class Utf8Helper
{
	/// <summary>
	/// Decodes a span of UTF-8 encoded bytes into Unicode characters and writes the result into the specified character
	/// span.
	/// </summary>
	/// <remarks>If the destination span is not large enough to contain all decoded characters, only as many
	/// characters as will fit are written. The method does not throw an exception in this case; callers should ensure
	/// the destination span is sufficiently sized for the expected output.</remarks>
	/// <param name="utf8Text">A read-only span containing the UTF-8 encoded bytes to decode.</param>
	/// <param name="destination">A span of characters where the decoded Unicode characters will be written.</param>
	/// <returns>The number of characters written to the destination span.</returns>
	internal static int GetChars(ReadOnlySpan<byte> utf8Text, Span<char> destination)
	{
#if !NETSTANDARD2_0
		return Encoding.UTF8.GetChars(utf8Text, destination);
#else
		unsafe
		{
			fixed (byte* srcPtr = utf8Text)
			fixed (char* dstPtr = destination)
			{
				return Encoding.UTF8.GetChars(srcPtr, utf8Text.Length, dstPtr, destination.Length);
			}
		}
#endif
	}

	/// <summary>
	/// Encodes a span of Unicode characters into a UTF-8 byte sequence and writes the result into the provided byte
	/// span.
	/// </summary>
	/// <remarks>If the destination span is not large enough to contain the encoded bytes, no data is written
	/// and the method returns 0. The method does not throw exceptions for insufficient buffer size.</remarks>
	/// <param name="chars">The span of Unicode characters to encode.</param>
	/// <param name="bytes">The destination span where the encoded UTF-8 bytes are written.</param>
	/// <returns>The number of bytes written to the destination span.</returns>
	internal static int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
	{
#if !NETSTANDARD2_0
		return Encoding.UTF8.GetBytes(chars, bytes);
#else
		unsafe
		{
			fixed (char* srcPtr = chars)
			fixed (byte* dstPtr = bytes)
			{
				return Encoding.UTF8.GetBytes(srcPtr, chars.Length, dstPtr, bytes.Length);
			}
		}
#endif
	}
}