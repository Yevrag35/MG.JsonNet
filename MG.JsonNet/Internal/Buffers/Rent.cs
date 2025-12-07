namespace MG.JsonNet.Internal.Buffers;

internal static class Rent
{
    internal static Span<byte> Array([NotNull] ref byte[]? array, int length)
    {
        array = ArrayPool<byte>.Shared.Rent(length);
        return array.AsSpan(0, length);
    }
    internal static void Return(byte[]? array)
    {
        if (array is not null)
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }
    internal static Span<char> Array([NotNull] ref char[]? array, int length)
    {
        array = ArrayPool<char>.Shared.Rent(length);
        return array.AsSpan(0, length);
    }
    internal static void Return(char[]? array)
    {
        if (array is not null)
        {
            ArrayPool<char>.Shared.Return(array);
        }
    }
}
