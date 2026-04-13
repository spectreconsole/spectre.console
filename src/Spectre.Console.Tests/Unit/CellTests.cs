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
    public void GetCellLength_Returns_Correct_Display_Width(string text, int expectedWidth)
    {
        Cell.GetCellLength(text).ShouldBe(expectedWidth);
    }

}
