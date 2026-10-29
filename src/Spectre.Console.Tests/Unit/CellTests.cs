using System.Reflection;

namespace Spectre.Console.Tests.Unit;

public sealed class CellTests
{
    [Theory]
    [InlineData("A", 1)]               // ASCII
    [InlineData("中", 2)]              // CJK wide
    [InlineData("♥", 1)]              // U+2665 text heart, no variation selector
    [InlineData("❤️", 2)]             // U+2764 + U+FE0F emoji variation selector
    [InlineData("👨‍👩‍👧", 2)]       // ZWJ family sequence
    [InlineData("❤️‍🔥", 2)]          // U+2764 + FE0F + ZWJ + U+1F525 (heart on fire)
    [InlineData("🇩🇪", 2)]           // Regional Indicator pair (flag)
    [InlineData("", 0)]               // empty string
    [InlineData("Hello World", 11)]
    [InlineData("e\u0301", 1)]        // e + combining acute accent (U+0301)
    [InlineData("Hello \u4E16\u754C", 10)] // mixed ASCII + CJK wide
    [InlineData("\u0007", -1)]          // C0 control character
    public void GetCellLength_Returns_Correct_Display_Width(string text, int expectedWidth)
    {
        Cell.GetCellLength(text).ShouldBe(expectedWidth);
        Cell.GetCellLength(text.AsSpan()).ShouldBe(expectedWidth);
    }

    [Theory]
    [InlineData("ab中cd", 2, 1, 2)]           // CJK wide slice
    [InlineData("ab❤️cd", 2, 2, 2)]     // emoji presentation slice falls back
    [InlineData("\U0001F600xyz", 2, 3, 3)]        // ASCII slice after a surrogate pair
    public void GetCellLength_Of_Span_Measures_Only_The_Slice(string text, int start, int length, int expectedWidth)
    {
        Cell.GetCellLength(text.AsSpan(start, length)).ShouldBe(expectedWidth);
    }

#if NET6_0_OR_GREATER
    // The fast path in Cell.GetCellLength(string) mirrors which characters
    // UnicodeCalculator.GetWidth(string) treats as sequences. If a Wcwidth
    // update adds a new sequence rule for a BMP character, these fail and
    // the fast path's fallback condition needs extending.
    // Spectre.Console and Spectre.Console.Ansi each compile in their own copy
    // of Wcwidth, so the copy Cell uses is looked up from Cell's assembly.
    private static readonly MethodInfo _wcwidthGetWidth = typeof(Cell).Assembly
        .GetType("Wcwidth.UnicodeCalculator", throwOnError: true)!
        .GetMethods()
        .Single(m => m.Name == "GetWidth" && m.GetParameters()[0].ParameterType == typeof(string));

    private static int WcwidthGetWidth(string text)
        => (int)_wcwidthGetWidth.Invoke(null, [text, null])!;

    [Fact]
    [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/2194")]
    public void GetCellLength_Matches_Wcwidth_For_Every_Bmp_Character()
    {
        var mismatches = new List<string>();
        for (var c = 0; c <= char.MaxValue; c++)
        {
            if (char.IsSurrogate((char)c))
            {
                continue;
            }

            var text = ((char)c).ToString();
            if (Cell.GetCellLength(text) != WcwidthGetWidth(text))
            {
                mismatches.Add($"U+{c:X4}");
            }
        }

        mismatches.ShouldBeEmpty();
    }

    [Fact]
    [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/2194")]
    public void GetCellLength_Matches_Wcwidth_For_Pairs_Of_Sequence_Sensitive_Characters()
    {
        var candidates = new List<char>
        {
            'a', ' ', 'é', '中', 'Ａ', // narrow, space, accented, CJK wide, fullwidth
            '❤', '☺', '☹', '©', '#', // characters with VS16 emoji forms
            'ᄀ', 'ᅠ', 'ᆨ', '가', // Hangul jamo and syllable
        };

        for (var c = 0; c <= char.MaxValue; c++)
        {
            var ch = (char)c;
            if (char.IsSurrogate(ch))
            {
                continue;
            }

            var category = char.GetUnicodeCategory(ch);
            if (category is UnicodeCategory.Format or UnicodeCategory.EnclosingMark
                || (ch >= '︀' && ch <= '️')
                || (category is UnicodeCategory.NonSpacingMark && ch < 'Ͱ'))
            {
                candidates.Add(ch);
            }
        }

        var mismatches = new List<string>();
        foreach (var first in candidates)
        {
            foreach (var second in candidates)
            {
                var text = new string([first, second]);
                if (Cell.GetCellLength(text) != WcwidthGetWidth(text))
                {
                    mismatches.Add($"U+{(int)first:X4} U+{(int)second:X4}");
                }
            }
        }

        mismatches.ShouldBeEmpty();
    }
#endif
}
