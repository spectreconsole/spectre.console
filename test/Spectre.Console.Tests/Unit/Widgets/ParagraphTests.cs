namespace Spectre.Console.Tests.Unit.Widgets;

[UsesVerify]
[ExpectationPath("Widgets/Paragraph")]
public sealed class ParagraphTests
{
    [Fact]
    [Expectation("Console_Profile_DefalaultPreserveSpacing")]
    public void Should_Have_PreserveSpacing_Disabled_By_Default()
    {
        var console = new TestConsole();
        console.Profile.PreserveSpacing.ShouldBeFalse();
    }

    [Theory]
    [InlineData(
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
        "Donec ultrices lorem sit amet quam malesuada, quis cursus lacus posuere. " +
        "Aenean id felis a lorem molestie eleifend. " +
        "Mauris vehicula convallis sagittis. Donec ut rutrum quam, ut pretium magna. ")]
    public void Should_Preserve_Spacing_Across_Word_Wrapping_NewLines(string text)
    {
        var tab = new string(' ', 4);
        var paragraph = MarkupParser.Parse(tab + text);

        var defaultConsole = new TestConsole();
        Regex.Matches(paragraph.GetSegments(defaultConsole).First().Text, tab)
            .Count.ShouldBe(1);

        var consoleWithSpacing = new TestConsole();
        consoleWithSpacing.Profile.PreserveSpacing = true;
        Regex.Matches(paragraph.GetSegments(consoleWithSpacing).First().Text, tab)
            .Count.ShouldBeGreaterThan(1);
    }
}
