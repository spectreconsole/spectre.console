using System;
using System.Text;

namespace Spectre.Console.Internal
{
    internal static class StringExtensions
    {
        // Cache whether or not internally normalized line endings
        // already are normalized. No reason to do yet another replace if it is.
        private static readonly bool _alreadyNormalized
            = Environment.NewLine.Equals("\n", StringComparison.OrdinalIgnoreCase);

        public static int CellLength(this string text, Encoding encoding)
        {
            return Cell.GetCellLength(encoding, text);
        }

        public static string NormalizeLineEndings(this string text, bool native = false)
        {
            var normalized = text?.Replace("\r\n", "\n")
                ?.Replace("\r", string.Empty);

            if (native && !_alreadyNormalized)
            {
                normalized = normalized.Replace("\n", Environment.NewLine);
            }

            return normalized;
        }

        public static string[] SplitLines(this string text)
        {
            return text.NormalizeLineEndings().Split(new[] { '\n' }, StringSplitOptions.None);
        }
    }
}
