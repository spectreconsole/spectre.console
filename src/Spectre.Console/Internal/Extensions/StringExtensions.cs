using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal static class StringExtensions
    {
        // Cache whether or not internally normalized line endings
        // already are normalized. No reason to do yet another replace if it is.
        private static readonly bool _alreadyNormalized
            = Environment.NewLine.Equals("\n", StringComparison.OrdinalIgnoreCase);

        public static int CellLength(this string text, RenderContext context)
        {
            return Cell.GetCellLength(context, text);
        }

        public static string NormalizeLineEndings(this string text, bool native = false)
        {
            text ??= string.Empty;

            var normalized = text?.Replace("\r\n", "\n")?.Replace("\r", string.Empty) ?? string.Empty;
            if (native && !_alreadyNormalized)
            {
                normalized = normalized.Replace("\n", Environment.NewLine);
            }

            return normalized;
        }

        public static string[] SplitLines(this string text)
        {
            var result = text?.NormalizeLineEndings()?.Split(new[] { '\n' }, StringSplitOptions.None);
            return result ?? Array.Empty<string>();
        }

        public static string[] SplitWords(this string word, StringSplitOptions options = StringSplitOptions.None)
        {
            var result = new List<string>();

            static string Read(StringBuffer reader, Func<char, bool> criteria)
            {
                var buffer = new StringBuilder();
                while (!reader.Eof)
                {
                    var current = reader.Peek();
                    if (!criteria(current))
                    {
                        break;
                    }

                    buffer.Append(reader.Read());
                }

                return buffer.ToString();
            }

            using (var reader = new StringBuffer(word))
            {
                while (!reader.Eof)
                {
                    var current = reader.Peek();
                    if (char.IsWhiteSpace(current))
                    {
                        var x = Read(reader, c => char.IsWhiteSpace(c));
                        if (options != StringSplitOptions.RemoveEmptyEntries)
                        {
                            result.Add(x);
                        }
                    }
                    else
                    {
                        result.Add(Read(reader, c => !char.IsWhiteSpace(c)));
                    }
                }
            }

            return result.ToArray();
        }

        // https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
        public static int GetDeterministicHashCode(this string str)
        {
            unchecked
            {
                var hash1 = (5381 << 16) + 5381;
                var hash2 = hash1;

                for (var i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                    {
                        break;
                    }

                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}
