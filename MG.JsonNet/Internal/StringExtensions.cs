namespace MG.JsonNet.Internal;

internal static class StringExtensions
{
    /// <summary>
	/// Attempts to find the last occurrence of a specified character within the span.
	/// </summary>
	/// <remarks>This method does not throw an exception if the character is not found. Instead, it returns <see
	/// langword="false"/>  and sets <paramref name="index"/> to -1.</remarks>
	/// <param name="chars">The <see cref="ReadOnlySpan{T}"/> of characters to search.</param>
	/// <param name="c">The character to locate within the span.</param>
	/// <param name="index">When this method returns, contains the zero-based index of the last occurrence of the specified character,  if
	/// found; otherwise, -1.</param>
	/// <returns><see langword="true"/> if the specified character is found in the span; otherwise, <see langword="false"/>.</returns>
	public static bool TryLastIndexOf(this ReadOnlySpan<char> chars, char c, out int index)
    {
        index = chars.LastIndexOf(c);
        return index != -1;
    }

//#if !NET10_0_OR_GREATER
//    /// <summary>
//    /// Attempts to find the last occurrence of a specified character within the span.
//    /// </summary>
//    /// <remarks>This method does not throw an exception if the character is not found. Instead, it returns <see
//    /// langword="false"/>  and sets <paramref name="index"/> to -1.</remarks>
//    /// <param name="chars">The <see cref="ReadOnlySpan{T}"/> of characters to search.</param>
//    /// <param name="c">The character to locate within the span.</param>
//    /// <param name="index">When this method returns, contains the zero-based index of the last occurrence of the specified character,  if
//    /// found; otherwise, -1.</param>
//    /// <returns><see langword="true"/> if the specified character is found in the span; otherwise, <see langword="false"/>.</returns>
//    public static bool TryLastIndexOf(this Span<char> chars, char c, out int index)
//    {
//        index = chars.LastIndexOf(c);
//        return index != -1;
//    }
//    /// <summary>
//    /// Attempts to find the last occurrence of a specified character within the span.
//    /// </summary>
//    /// <remarks>This method does not throw an exception if the character is not found. Instead, it returns <see
//    /// langword="false"/>  and sets <paramref name="index"/> to -1.</remarks>
//    /// <param name="chars">The <see cref="ReadOnlySpan{T}"/> of characters to search.</param>
//    /// <param name="c">The character to locate within the span.</param>
//    /// <param name="index">When this method returns, contains the zero-based index of the last occurrence of the specified character,  if
//    /// found; otherwise, -1.</param>
//    /// <returns><see langword="true"/> if the specified character is found in the span; otherwise, <see langword="false"/>.</returns>
//    public static bool TryLastIndexOf(this string s, char c, out int index)
//    {
//        index = s.LastIndexOf(c);
//        return index != -1;
//    }
//#endif
}