namespace Spectre.Console.Ansi.Tests;

public sealed class AnsiMarkupTests
{
    public sealed class TheParseMethod
    {
        [Theory]
        [InlineData("[yellow]Hello[", "Encountered malformed markup tag at position 14.")]
        [InlineData("[yellow]Hello[/", "Encountered malformed markup tag at position 15.")]
        [InlineData("[yellow]Hello[/foo", "Encountered malformed markup tag at position 15.")]
        [InlineData("[yellow Hello", "Encountered malformed markup tag at position 13.")]
        [InlineData("[yellow[green]]Hello", "Encountered malformed markup tag at position 7.")]
        public void Should_Throw_If_Encounters_Malformed_Tag(string markup, string expected)
        {
            // Given, When
            var result = Record.Exception(() => AnsiMarkup.Parse(markup));

            // Then
            result.ShouldBeOfType<InvalidOperationException>()
                .Message.ShouldBe(expected);
        }

        [Fact]
        public void Should_Throw_If_Tags_Are_Unbalanced()
        {
            // Given, When
            var result = Record.Exception(() => AnsiMarkup.Parse("[yellow][blue]Hello[/]"));

            // Then
            result.ShouldBeOfType<InvalidOperationException>()
                .Message.ShouldBe("Unbalanced markup stack. Did you forget to close a tag?");
        }

        [Fact]
        public void Should_Throw_If_Encounters_Closing_Tag()
        {
            // Given, When
            var result = Record.Exception(() => AnsiMarkup.Parse("Hello[/]World"));

            // Then
            result.ShouldBeOfType<InvalidOperationException>()
                .Message.ShouldBe("Encountered closing tag when none was expected near position 5.");
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
            var result = AnsiMarkup.Escape(input);

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
        [InlineData("[grey][[grey]][/][white][[white]][/]", "[grey][white]")]
        public void Should_Remove_Markup_From_Text(string input, string expected)
        {
            // Given, When
            var result = AnsiMarkup.Remove(input);

            // Then
            result.ShouldBe(expected);
        }
    }

    public sealed class TheHighlightMethod
    {
        private readonly Style _highlightStyle =
            new Style(foreground: Color.Default, background: Color.Yellow, Decoration.Bold);

        [Fact]
        public void Should_Return_Same_Value_When_SearchText_Is_Empty()
        {
            // Given
            var value = "Sample text";
            var searchText = string.Empty;
            var highlightStyle = new Style();

            // When
            var result = AnsiMarkup.Highlight(value, searchText, highlightStyle);

            // Then
            result.ShouldBe(value);
        }

        [Fact]
        public void Should_Highlight_Matched_Text()
        {
            // Given
            var value = "Sample text with test word";
            var searchText = "test";
            var highlightStyle = _highlightStyle;

            // When
            var result = AnsiMarkup.Highlight(value, searchText, highlightStyle);

            // Then
            result.ShouldBe("Sample text with [bold on yellow]test[/] word");
        }

        [Fact]
        public void Should_Match_Text_Across_Tokens()
        {
            // Given
            var value = "[red]Sample text[/] with test word";
            var searchText = "text with";
            var highlightStyle = _highlightStyle;

            // When
            var result = AnsiMarkup.Highlight(value, searchText, highlightStyle);

            // Then
            result.ShouldBe("[red]Sample [/][bold on yellow]text with[/] test word");
        }

        [Fact]
        public void Should_Highlight_Only_First_Matched_Text()
        {
            // Given
            var value = "Sample text with test word";
            var searchText = "te";
            var highlightStyle = _highlightStyle;

            // When
            var result = AnsiMarkup.Highlight(value, searchText, highlightStyle);

            // Then
            result.ShouldBe("Sample [bold on yellow]te[/]xt with test word");
        }

        [Fact]
        public void Should_Not_Match_Text_Outside_Of_Text_Tokens()
        {
            // Given
            var value = "[red]Sample text with test word[/]";
            var searchText = "red";
            var highlightStyle = _highlightStyle;

            // When
            var result = AnsiMarkup.Highlight(value, searchText, highlightStyle);

            // Then
            result.ShouldBe(value);
        }
    }

    public sealed class TheWriteMethod
    {
        [Fact]
        public void Should_Escape_Markup_Blocks_As_Expected()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Markup.Write("Hello [[ World ]] !");

            // Then
            fixture.Output
                .ShouldBe("Hello [ World ] !");
        }

        [Theory]
        [InlineData("[yellow]Hello[/]", "[93mHello[0m")]
        [InlineData("[yellow]Hello [italic]World[/]![/]", "[93mHello [0m[3;93mWorld[0m[93m![0m")]
        public void Should_Output_Expected_Ansi_For_Markup(string text, string expected)
        {
            // Given
            var fixture = new AnsiFixture();
            fixture.Capabilities.ColorSystem = ColorSystem.Standard;

            // When
            fixture.Markup.Write(text);

            // Then
            fixture.Output
                .ShouldBe(expected);
        }

        [Fact]
        public void Should_Output_Expected_Ansi_For_Link_With_Url_And_Text()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Markup.Write("[link=https://patriksvensson.se]Click to visit my blog[/]");

            // Then
            fixture.Output.ShouldMatch(
                "]8;id=[0-9]*;https:\\/\\/patriksvensson\\.se\\\\Click to visit my blog]8;;\\\\");
        }

        [Fact]
        public void Should_Output_Expected_Ansi_For_Link_With_Only_Url()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Markup.Write("[link]https://patriksvensson.se[/]");

            // Then
            fixture.Output.ShouldMatch(
                "]8;id=[0-9]*;https:\\/\\/patriksvensson\\.se\\\\https:\\/\\/patriksvensson\\.se]8;;\\\\");
        }

        [Fact]
        public void Should_Output_Expected_Ansi_For_Link_With_Bracket_In_Url_Only()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            const string Path = "file://c:/temp/[x].txt";
            fixture.Markup.Write($"[link]{AnsiMarkup.Escape(Path)}[/]");

            // Then
            fixture.Output.ShouldMatch(
                "]8;id=[0-9]*;file:\\/\\/c:\\/temp\\/\\[x\\].txt\\\\file:\\/\\/c:\\/temp\\/\\[x\\].txt]8;;\\\\");
        }

        [Fact]
        public void Should_Output_Expected_Ansi_For_Link_With_Bracket_In_Url()
        {
            // Given
            var fixture = new AnsiFixture();
            const string Path = "file://c:/temp/[x].txt";
            var escapedPath = AnsiMarkup.Escape(Path);

            // When
            fixture.Markup.Write($"[link={escapedPath}]{escapedPath}[/]");

            // Then
            fixture.Output.ShouldMatch(
                "]8;id=[0-9]*;file:\\/\\/c:\\/temp\\/\\[x\\].txt\\\\file:\\/\\/c:\\/temp\\/\\[x\\].txt]8;;\\\\");
        }

        [Theory]
        [InlineData("[yellow]Hello [[ World[/]", "\e[93mHello [ World\e[0m")]
        public void Should_Be_Able_To_Escape_Tags(string text, string expected)
        {
            // Given
            var fixture = new AnsiFixture();
            fixture.Capabilities.ColorSystem = ColorSystem.Standard;

            // When
            fixture.Markup.Write(text);

            // Then
            fixture.Output
                .ShouldBe(expected);
        }
    }
}