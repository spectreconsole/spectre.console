using Spectre.Console;

namespace Namespace;

public class HighlightTests
{
    [Fact]
    public void Should_Return_Same_Value_When_SearchText_Is_Empty()
    {
        // Given
        var value = "Sample text";
        var searchText = string.Empty;
        var highlightStyle = new Style();

        // When
        var result = value.Highlight(searchText, highlightStyle);

        // Then
        result.ShouldBe(value);
    }

    [Fact]
    public void Should_Highlight_Matched_Text()
    {
        // Given
        var value = "Sample text with test word";
        var searchText = "test";
        var highlightStyle = new Style(foreground: Color.Default, background: Color.Yellow, Decoration.Bold);

        // When
        var result = value.Highlight(searchText, highlightStyle);

        // Then
        result.ShouldBe("Sample text with [bold on yellow]test[/] word");
    }

    [Fact]
    public void Should_Not_Match_Text_Across_Tokens()
    {
        // Given
        var value = "[red]Sample text[/] with test word";
        var searchText = "text with";
        var highlightStyle = new Style(foreground: Color.Default, background: Color.Yellow, Decoration.Bold);

        // When
        var result = value.Highlight(searchText, highlightStyle);

        // Then
        result.ShouldBe(value);
    }
}