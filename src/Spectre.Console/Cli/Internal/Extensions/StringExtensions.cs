#if NET5_0
using System;
#endif

namespace Spectre.Console.Cli
{
    internal static class StringExtensions
    {
        internal static int OrdinalIndexOf(this string text, char token)
        {
#if NET5_0
            return text.IndexOf(token, StringComparison.Ordinal);
#else
            return text.IndexOf(token);
#endif
        }
    }
}
