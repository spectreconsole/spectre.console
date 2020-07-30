using System;

namespace Spectre.Console.Tests
{
    public static class StringExtensions
    {
        public static string NormalizeLineEndings(this string text)
        {
            return text?.Replace("\r\n", "\n", StringComparison.OrdinalIgnoreCase)
                ?.Replace("\r", string.Empty, StringComparison.OrdinalIgnoreCase);
        }
    }
}
