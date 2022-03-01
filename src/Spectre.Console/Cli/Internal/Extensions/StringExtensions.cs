namespace Spectre.Console.Cli;

internal static class StringExtensions
{
    internal static int OrdinalIndexOf(this string text, char token)
    {
#if NETSTANDARD2_0
        return text.IndexOf(token);
#else
        return text.IndexOf(token, System.StringComparison.Ordinal);
#endif
    }
}