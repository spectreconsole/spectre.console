using System;
using System.Text.RegularExpressions;

namespace Spectre.Console.Tests
{
    public static class StringExtensions
    {
        private static readonly Regex _lineNumberRegex = new Regex(":\\d+", RegexOptions.Singleline);
        private static readonly Regex _filenameRegex = new Regex("\\sin\\s.*cs:nn", RegexOptions.Multiline);

        public static string NormalizeLineEndings(this string text)
        {
            return text?.Replace("\r\n", "\n", StringComparison.OrdinalIgnoreCase)
                ?.Replace("\r", string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        public static string NormalizeStackTrace(this string text)
        {
            text = _lineNumberRegex.Replace(text, match =>
            {
                return ":nn";
            });

            return _filenameRegex.Replace(text, match =>
            {
                var value = match.Value;
                var index = value.LastIndexOfAny(new[] { '\\', '/' });
                var filename = value.Substring(index + 1, value.Length - index - 1);

                return $" in /xyz/{filename}";
            });
        }
    }
}
