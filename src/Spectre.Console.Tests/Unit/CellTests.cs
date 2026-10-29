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
}
