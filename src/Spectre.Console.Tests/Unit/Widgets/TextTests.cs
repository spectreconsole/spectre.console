namespace Spectre.Console.Tests.Unit;

public sealed class TextTests
{
    public sealed class TheLengthProperty
    {
        [Theory]
        [InlineData("Hello", 5)]
        [InlineData("Hello\nWorld", 11)]
        public void Should_Return_The_Number_Of_Characters(string input, int expected)
        {
            // Given
            var markup = new Text(input);

            // When
            var result = markup.Length;

            // Then
            result.ShouldBe(expected);
        }
    }

    public sealed class TheLinesProperty
    {
        [Theory]
        [InlineData("Hello", 1)]
        [InlineData("Hello\nWorld", 2)]
        public void Should_Return_The_Number_Of_Lines(string input, int expected)
        {
            // Given
            var markup = new Text(input);

            // When
            var result = markup.Lines;

            // Then
            result.ShouldBe(expected);
        }
    }

    [Fact]
    public void Should_Consider_The_Longest_Word_As_Minimum_Width()
    {
        // Given
        var console = new TestConsole();
        var caps = new TestCapabilities { Unicode = true };
        var text = new Text("Foo Bar Baz\nQux\nLol mobile");

        // When
        var result = ((IRenderable)text).Measure(
            caps.CreateRenderContext(console), 80);

        // Then
        result.Min.ShouldBe(6);
    }

    [Fact]
    public void Should_Consider_The_Longest_Line_As_Maximum_Width()
    {
        // Given
        var console = new TestConsole();
        var caps = new TestCapabilities { Unicode = true };
        var text = new Text("Foo Bar Baz\nQux\nLol mobile");

        // When
        var result = ((IRenderable)text).Measure(
            caps.CreateRenderContext(console), 80);

        // Then
        result.Max.ShouldBe(11);
    }

    [Fact]
    public void Should_Render_Unstyled_Text_As_Expected()
    {
        // Given
        var console = new TestConsole();
        var text = new Text("Hello World");

        // When
        console.Write(text);

        // Then
        console.Output.ShouldBe("Hello World");
    }

    [Theory]
    [InlineData("Hello\n\nWorld\n\n")]
    [InlineData("Hello\r\n\r\nWorld\r\n\r\n")]
    public void Should_Write_Line_Breaks(string input)
    {
        // Given
        var console = new TestConsole();
        var text = new Text(input);

        // When
        console.Write(text);

        // Then
        console.Output.ShouldBe("Hello\n\nWorld\n\n");
    }

    [Fact]
    public void Should_Render_Panel_2()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Markup("[b]Hello World[/]\n[yellow]Hello World[/]"));

        // Then
        console.Lines.Count.ShouldBe(2);
        console.Lines[0].ShouldBe("Hello World");
        console.Lines[1].ShouldBe("Hello World");
    }

    [Theory]
    [InlineData(5, "Hello World", "Hello\nWorld")]
    [InlineData(10, "Hello Sweet Nice World", "Hello \nSweet Nice\nWorld")]
    public void Should_Split_Unstyled_Text_To_New_Lines_If_Width_Exceeds_Console_Width(
        int width, string input, string expected)
    {
        // Given
        var console = new TestConsole().Width(width);
        var text = new Text(input);

        // When
        console.Write(text);

        // Then
        console.Output
            .NormalizeLineEndings()
            .ShouldBe(expected);
    }

    [Theory]
    [InlineData(Overflow.Fold, "foo \npneumonoultram\nicroscopicsili\ncovolcanoconio\nsis bar qux")]
    [InlineData(Overflow.Crop, "foo \npneumonoultram\nbar qux")]
    [InlineData(Overflow.Ellipsis, "foo \npneumonoultra…\nbar qux")]
    public void Should_Overflow_Text_Correctly(Overflow overflow, string expected)
    {
        // Given
        var console = new TestConsole().Width(14);
        var text = new Text("foo pneumonoultramicroscopicsilicovolcanoconiosis bar qux")
            .Overflow(overflow);

        // When
        console.Write(text);

        // Then
        console.Output
            .NormalizeLineEndings()
            .ShouldBe(expected);
    }
}
