namespace Spectre.Console.Tests.Unit;

public sealed class MarkupTests
{
    public sealed class TheLengthProperty
    {
        [Theory]
        [InlineData("Hello", 5)]
        [InlineData("Hello\nWorld", 11)]
        [InlineData("[yellow]Hello[/]", 5)]
        public void Should_Return_The_Number_Of_Characters(string input, int expected)
        {
            // Given
            var markup = new Markup(input);

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
        [InlineData("[yellow]Hello[/]\nWorld", 2)]
        public void Should_Return_The_Number_Of_Lines(string input, int expected)
        {
            // Given
            var markup = new Markup(input);

            // When
            var result = markup.Lines;

            // Then
            result.ShouldBe(expected);
        }
    }

    public sealed class TheEscapeMethod
    {
        [Theory]
        [InlineData("Hello World", "Hello World")]
        [InlineData("Hello World [", "Hello World [[")]
        [InlineData("Hello World ]", "Hello World ]]")]
        [InlineData("Hello [World]", "Hello [[World]]")]
        [InlineData("Hello [[World]]", "Hello [[[[World]]]]")]
        public void Should_Escape_Markup_As_Expected(string input, string expected)
        {
            // Given, When
            var result = Markup.Escape(input);

            // Then
            result.ShouldBe(expected);
        }
    }

    public sealed class TheRemoveMethod
    {
        [Theory]
        [InlineData("Hello World", "Hello World")]
        [InlineData("Hello [blue]World", "Hello World")]
        [InlineData("Hello [blue]World[/]", "Hello World")]
        public void Should_Remove_Markup_From_Text(string input, string expected)
        {
            // Given, When
            var result = Markup.Remove(input);

            // Then
            result.ShouldBe(expected);
        }

        [Theory]
        [InlineData("Hello", "World", "\x1B[38;5;11mHello\x1B[0m \x1B[38;5;9mWorld\x1B[0m 2021-02-03")]
        [InlineData("Hello", "World [", "\x1B[38;5;11mHello\x1B[0m \x1B[38;5;9mWorld [\x1B[0m 2021-02-03")]
        [InlineData("Hello", "World ]", "\x1B[38;5;11mHello\x1B[0m \x1B[38;5;9mWorld ]\x1B[0m 2021-02-03")]
        [InlineData("[Hello]", "World", "\x1B[38;5;11m[Hello]\x1B[0m \x1B[38;5;9mWorld\x1B[0m 2021-02-03")]
        [InlineData("[[Hello]]", "[World]", "\x1B[38;5;11m[[Hello]]\x1B[0m \x1B[38;5;9m[World]\x1B[0m 2021-02-03")]
        public void Should_Escape_Markup_When_Using_MarkupInterpolated(string input1, string input2, string expected)
        {
            // Given
            var console = new TestConsole().EmitAnsiSequences();
            var date = new DateTime(2021, 2, 3);

            // When
            console.MarkupInterpolated($"[yellow]{input1}[/] [red]{input2}[/] {date:yyyy-MM-dd}");

            // Then
            console.Output.ShouldBe(expected);
        }
    }

    [Theory]
    [InlineData("Hello [[ World ]")]
    [InlineData("Hello [[ World ] !")]
    public void Should_Throw_If_Closing_Tag_Is_Not_Properly_Escaped(string input)
    {
        // Given
        var console = new TestConsole();

        // When
        var result = Record.Exception(() => new Markup(input));

        // Then
        result.ShouldNotBeNull();
        result.ShouldBeOfType<InvalidOperationException>();
        result.Message.ShouldBe("Encountered unescaped ']' token at position 16");
    }

    [Fact]
    public void Should_Escape_Markup_Blocks_As_Expected()
    {
        // Given
        var console = new TestConsole();
        var markup = new Markup("Hello [[ World ]] !");

        // When
        console.Write(markup);

        // Then
        console.Output.ShouldBe("Hello [ World ] !");
    }

    [Theory]
    [InlineData("Hello [link=http://example.com]example.com[/]", "Hello example.com")]
    [InlineData("Hello [link=http://example.com]http://example.com[/]", "Hello http://example.com")]
    public void Should_Render_Links_As_Expected(string input, string output)
    {
        // Given
        var console = new TestConsole();
        var markup = new Markup(input);

        // When
        console.Write(markup);

        // Then
        console.Output.ShouldBe(output);
    }

    [Fact]
    public void Should_Not_Fail_With_Brackets_On_Calls_Without_Args()
    {
        // Given
        var console = new TestConsole();

        // When
        console.MarkupLine("{");

        // Then
        console.Output.NormalizeLineEndings()
            .ShouldBe("{\n");
    }

    [Fact]
    public void Can_Use_Interpolated_Markup_As_IRenderable()
    {
        // Given
        var console = new TestConsole();
        const string Num = "[value[";
        var table = new Table().AddColumns("First Column");
        table.AddRow(Markup.FromInterpolated($"Result: {Num}"));

        // When
        console.Write(table);

        // Then
        console.Output.NormalizeLineEndings().ShouldBe(@"┌─────────────────┐
│ First Column    │
├─────────────────┤
│ Result: [value[ │
└─────────────────┘
".NormalizeLineEndings());
    }
}
