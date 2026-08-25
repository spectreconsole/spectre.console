namespace Spectre.Console;

internal static class Cell
{
    private const sbyte Sentinel = -2;

    /// <summary>
    /// UnicodeCalculator.GetWidth documents the width as (-1, 0, 1, 2). We only need space for these values and a sentinel for uninitialized values.
    /// This is only five values in total so we are storing one byte per value. We could store 2 per byte but that would add more logic to the retrieval.
    /// We should add one to char.MaxValue because the total number of characters includes \0 too so there are 65536 valid chars.
    /// </summary>
    private static readonly sbyte[] _runeWidthCache = new sbyte[char.MaxValue + 1];

    static Cell()
    {
#if !NETSTANDARD2_0
        Array.Fill(_runeWidthCache, Sentinel);
#else
        for (var i = 0; i < _runeWidthCache.Length; i++)
        {
            _runeWidthCache[i] = Sentinel;
        }
#endif
    }

    public static int GetCellLength(string text)
    {
#if !NETSTANDARD2_0
        // Strings without surrogate pairs, zero width joiners (U+200D) or
        // variation selector 16 (U+FE0F) measure as the sum of their individual
        // UTF-16 code unit widths, which the cache below answers without the
        // per-call rune enumeration UnicodeCalculator.GetWidth(string) performs.
        var sum = 0;
        for (var i = 0; i < text.Length; i++)
        {
            var current = text[i];
            if (char.IsSurrogate(current) || current == '\u200D' || current == '\uFE0F')
            {
                return UnicodeCalculator.GetWidth(text);
            }

            var width = _runeWidthCache[current];
            if (width == Sentinel)
            {
                width = (sbyte)UnicodeCalculator.GetWidth(current);
                _runeWidthCache[current] = width;
            }

            if (width < 0)
            {
                // UnicodeCalculator.GetWidth(string) returns -1 for the whole
                // string when it contains C0/C1 control characters.
                return -1;
            }

            sum += width;
        }

        return sum;
#else
        var sum = 0;
        foreach (var rune in text)
        {
            sum += GetCellLength(rune);
        }

        return sum;
#endif
    }

    public static int GetCellLength(ReadOnlySpan<char> text)
        => GetCellLength(text.ToString());

    public static int GetCellLength(char rune)
    {
        // TODO: We need to figure out why Segment.SplitLines fails
        // if we let wcwidth (which returns -1 instead of 1)
        // calculate the size for new line characters.
        // That is correct from a Unicode perspective, but the
        // algorithm was written before wcwidth was added and used
        // to work with string length and not cell length.
        if (rune == '\n')
        {
            return 1;
        }

        var width = _runeWidthCache[rune];
        if (width == Sentinel)
        {
            width = (sbyte)UnicodeCalculator.GetWidth(rune);
            _runeWidthCache[rune] = width;
        }

        return width;
    }
}