using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spectre.Console
{
    public static class StringExtensions
    {
        private static readonly Regex _lineNumberRegex = new Regex(":\\d+", RegexOptions.Singleline);
        private static readonly Regex _filenameRegex = new Regex("\\sin\\s.*cs:nn", RegexOptions.Multiline);

        public static string TrimLines(this string value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            var result = new List<string>();
            var lines = value.Split(new[] { '\n' });

            foreach (var line in lines)
            {
                var current = line.TrimEnd();
                if (string.IsNullOrWhiteSpace(current))
                {
                    result.Add(string.Empty);
                }
                else
                {
                    result.Add(current);
                }
            }

            return string.Join("\n", result);
        }

        public static string NormalizeLineEndings(this string value)
        {
            if (value != null)
            {
                value = value.Replace("\r\n", "\n");
                return value.Replace("\r", string.Empty);
            }

            return string.Empty;
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

        internal static string ReplaceExact(this string text, string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                return text;
            }

#if NET5_0
            return text.Replace(oldValue, newValue, StringComparison.Ordinal);
#else
            return text.Replace(oldValue, newValue);
#endif
        }
    }
}
