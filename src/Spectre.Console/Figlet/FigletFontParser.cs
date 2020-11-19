using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Spectre.Console
{
    internal static class FigletFontParser
    {
        public static FigletFont Parse(string source)
        {
            var lines = source.SplitLines();
            var header = ParseHeader(lines.FirstOrDefault());

            var characters = new List<FigletCharacter>();

            var index = 32;
            var indexOverridden = false;
            var hasOverriddenIndex = false;
            var buffer = new List<string>();

            foreach (var line in lines.Skip(header.CommentLines + 1))
            {
                if (!line.EndsWith("@", StringComparison.Ordinal))
                {
                    var words = line.SplitWords();
                    if (words.Length > 0 && TryParseIndex(words[0], out var newIndex))
                    {
                        index = newIndex;
                        indexOverridden = true;
                        hasOverriddenIndex = true;
                        continue;
                    }

                    continue;
                }

                if (hasOverriddenIndex && !indexOverridden)
                {
                    throw new InvalidOperationException("Unknown index for FIGlet character");
                }

                buffer.Add(line.Replace('$', ' ').ReplaceExact("@", string.Empty));

                if (line.EndsWith("@@", StringComparison.Ordinal))
                {
                    characters.Add(new FigletCharacter(index, buffer));
                    buffer.Clear();

                    if (!hasOverriddenIndex)
                    {
                        index++;
                    }

                    // Reset the flag so we know if we're trying to parse
                    // a character that wasn't prefixed with an ASCII index.
                    indexOverridden = false;
                }
            }

            return new FigletFont(characters, header);
        }

        private static bool TryParseIndex(string index, out int result)
        {
            var style = NumberStyles.Integer;
            if (index.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                // TODO: ReplaceExact should not be used
                index = index.ReplaceExact("0x", string.Empty).ReplaceExact("0x", string.Empty);
                style = NumberStyles.HexNumber;
            }

            return int.TryParse(index, style, CultureInfo.InvariantCulture, out result);
        }

        private static FigletHeader ParseHeader(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException("Invalid Figlet font");
            }

            var parts = text.SplitWords(StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 6)
            {
                throw new InvalidOperationException("Invalid Figlet font header");
            }

            if (!IsValidSignature(parts[0]))
            {
                throw new InvalidOperationException("Invalid Figlet font header signature");
            }

            return new FigletHeader
            {
                Hardblank = parts[0][5],
                Height = int.Parse(parts[1], CultureInfo.InvariantCulture),
                Baseline = int.Parse(parts[2], CultureInfo.InvariantCulture),
                MaxLength = int.Parse(parts[3], CultureInfo.InvariantCulture),
                OldLayout = int.Parse(parts[4], CultureInfo.InvariantCulture),
                CommentLines = int.Parse(parts[5], CultureInfo.InvariantCulture),
            };
        }

        private static bool IsValidSignature(string signature)
        {
            return signature.Length == 6
                && signature[0] == 'f' && signature[1] == 'l'
                && signature[2] == 'f' && signature[3] == '2'
                && signature[4] == 'a';
        }
    }
}
