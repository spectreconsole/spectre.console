namespace Docs.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNotEmpty(this string source)
        {
            return !string.IsNullOrWhiteSpace(source);
        }
    }
}
