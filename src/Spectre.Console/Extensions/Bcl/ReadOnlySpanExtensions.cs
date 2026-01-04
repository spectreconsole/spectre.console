namespace Spectre.Console;

internal static class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        public int IndexOf(T value, int startIndex)
        {
            var indexInSlice = span[startIndex..].IndexOf(value);
            if (indexInSlice == -1)
            {
                return -1;
            }

            return startIndex + indexInSlice;
        }
    }
}