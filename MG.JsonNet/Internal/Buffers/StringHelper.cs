namespace MG.JsonNet.Internal.Buffers;

internal static class StringHelper
{
	internal static string Create(ReadOnlySpan<char> chars)
	{
		if (chars.IsEmpty) return string.Empty;

#if !NETSTANDARD2_0
		return new(chars);
#else
		unsafe
		{
			fixed (char* ptr = chars)
			{
				return new string(ptr, 0, chars.Length);
			}
		}
#endif
	}
}
