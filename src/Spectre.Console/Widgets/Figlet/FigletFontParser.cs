namespace Spectre.Console;

internal static class FigletFontParser
{
    public static FigletFont Parse(string source)
    {
        var lines = source.SplitLines();

        var headerLine = lines.FirstOrDefault();
        if (headerLine == null)
        {
            throw new InvalidOperationException("Could not read header line");
        }

        var header = ParseHeader(headerLine);

        var index = 32;
        var indexOverridden = false;
        var hasOverriddenIndex = false;
        var buffer = new List<string>();
        var characters = new List<FigletCharacter>();
        var accumulatedHeight = 0;

        var eolMarker = ParseEndOfLineMarker(lines, header);

        foreach (var line in lines.Skip(header.CommentLines + 1))
        {
            // This might be an index?
            if (!line.EndsWith(eolMarker))
            {
                var words = line.SplitWords();
                if (words.Length > 0 && TryParseIndex(words[0], out var newIndex))
                {
                    index = newIndex;
                    indexOverridden = true;
                    hasOverriddenIndex = true;
                    accumulatedHeight = 0;
                    continue;
                }
            }

            if (hasOverriddenIndex && !indexOverridden)
            {
                continue;
            }

            buffer.Add(line.TrimEnd(eolMarker));
            accumulatedHeight++;

            if (accumulatedHeight == header.Height)
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

                // Reset the accumulated height
                accumulatedHeight = 0;
            }
        }

        return new FigletFont(characters, header);
    }

    private static char ParseEndOfLineMarker(string[] lines, FigletHeader header)
    {
        var firstEndLine = lines.Skip(header.CommentLines + 1).Take(1).FirstOrDefault();
        var endChar = firstEndLine?.Trim().LastOrDefault();
        if (endChar == null)
        {
            // Default
            return '@';
        }

        return endChar.Value;
    }

    private static bool TryParseIndex(string index, out int result)
    {
        var style = NumberStyles.Integer;
        if (index.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            index = index.ReplaceExact("0x", string.Empty);
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

        int? fullLayout = null;
        if (parts.Length > 7 && int.TryParse(parts[7], NumberStyles.Integer, CultureInfo.InvariantCulture, out var fl))
        {
            fullLayout = fl;
        }

        return new FigletHeader
        {
            Hardblank = parts[0][5],
            Height = int.Parse(parts[1], CultureInfo.InvariantCulture),
            Baseline = int.Parse(parts[2], CultureInfo.InvariantCulture),
            MaxLength = int.Parse(parts[3], CultureInfo.InvariantCulture),
            OldLayout = int.Parse(parts[4], CultureInfo.InvariantCulture),
            CommentLines = int.Parse(parts[5], CultureInfo.InvariantCulture),
            FullLayout = fullLayout,
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