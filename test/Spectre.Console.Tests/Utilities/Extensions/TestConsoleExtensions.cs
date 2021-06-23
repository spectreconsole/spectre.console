using System;
using System.Linq;
using System.Text.RegularExpressions;
using Spectre.Console.Testing;

namespace Spectre.Console.Tests
{
    public static class TestConsoleExtensions
    {
        private static readonly Regex _lineNumberRegex = new Regex(":\\d+", RegexOptions.Singleline);
        private static readonly Regex _filenameRegex = new Regex("\\sin\\s.*cs:nn", RegexOptions.Multiline);

        public static string WriteNormalizedException(this TestConsole console, Exception ex, ExceptionFormats formats = ExceptionFormats.Default)
        {
            if (!string.IsNullOrWhiteSpace(console.Output))
            {
                throw new InvalidOperationException("Output buffer is not empty.");
            }

            console.WriteException(ex, formats);
            return string.Join("\n", NormalizeStackTrace(console.Output)
                .NormalizeLineEndings()
                .Split(new char[] { '\n' })
                .Select(line => line.TrimEnd()));
        }

        public static string NormalizeStackTrace(string text)
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
