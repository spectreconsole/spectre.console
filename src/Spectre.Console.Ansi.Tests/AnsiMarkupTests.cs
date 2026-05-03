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

        [Fact]
        public void Should_Not_Corrupt_Escaped_Brackets_When_Highlighting()
        {
            // Given
            var value = "MSFT-Provisioning-01[[Prod]] (guid-2)";
            var searchText = "M";

            // When
            var result = AnsiMarkup.Highlight(value, searchText, _highlightStyle);

            // Then
            // The result should be valid markup that can be parsed without throwing
            var exception = Record.Exception(() => AnsiMarkup.Parse(result));
            exception.ShouldBeNull();
        }

        [Theory]
        [InlineData("Hello [[World]]", "World", "Hello [[[bold on yellow]World[/]]]")]
        [InlineData("[[Prod]] Service", "Prod", "[[[bold on yellow]Prod[/]]] Service")]
        [InlineData("Item [[A]] and [[B]]", "A", "Item [[[bold on yellow]A[/]]] and [[B]]")]
        public void Should_Produce_Valid_Markup_With_Escaped_Brackets(
            string value, string searchText, string expected)
        {
            // Given, When
            var result = AnsiMarkup.Highlight(value, searchText, _highlightStyle);

            // Then
            result.ShouldBe(expected);
        }

        [Fact]
        public void Should_Highlight_Text_Adjacent_To_Escaped_Brackets()
        {
            // Given
            var value = "MSFT[[Prod]]Service";
            var searchText = "MSFT";

            // When
            var result = AnsiMarkup.Highlight(value, searchText, _highlightStyle);

            // Then
            var exception = Record.Exception(() => AnsiMarkup.Parse(result));
            exception.ShouldBeNull();
            result.ShouldBe("[bold on yellow]MSFT[/][[Prod]]Service");
        }

        [Fact]
        public void Should_Highlight_Text_After_Escaped_Brackets()
        {
            // Given
            var value = "[[Dev]] Environment";
            var searchText = "Env";

            // When
            var result = AnsiMarkup.Highlight(value, searchText, _highlightStyle);

            // Then
            var exception = Record.Exception(() => AnsiMarkup.Parse(result));
            exception.ShouldBeNull();
            result.ShouldBe("[[Dev]] [bold on yellow]Env[/]ironment");
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
        [InlineData("[yellow]Hello[/]", "\e[93mHello\e[0m")]
        [InlineData("[yellow]Hello [italic]World[/]![/]", "\e[93mHello \e[0m\e[3;93mWorld\e[0m\e[93m!\e[0m")]
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

        [Theory]
        [InlineData("[clear:to eol]", "\x1B[K")]
        [InlineData("[clear:eol]", "\x1B[K")]
        [InlineData("[clear:screen]", "\x1B[H\x1B[2J")]
        [InlineData("[clear:to eos]", "\x1B[0J")]
        [InlineData("[clear:#23]", "\x1B[23;1H")]
        public void Should_Output_Expected_Ansi_For_Clear_Directives(string text, string expected)
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Markup.Write(text);

            // Then
            fixture.Output
                .ShouldBe(expected);
        }

        [Theory]
        [InlineData("[move:to 10]", "\x1B[10;1H")]
        [InlineData("[move:10;20]", "\x1B[10;20H")]
        [InlineData("[move:up 3]", "\x1B[3A")]
        [InlineData("[move:down 4]", "\x1B[4B")]
        [InlineData("[move:right 5]", "\x1B[5C")]
        [InlineData("[move:left 2]", "\x1B[2D")]
        [InlineData("[move:home]", "\x1B[H")]
        public void Should_Output_Expected_Ansi_For_Move_Directives(string text, string expected)
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Markup.Write(text);

            // Then
            fixture.Output
                .ShouldBe(expected);
        }

        [Theory]
        [InlineData("[move:up blue]")]
        [InlineData("[move:down abc]")]
        [InlineData("[move:left -5]")]
        [InlineData("[move:to]")]
        [InlineData("[move:to -1]")]
        [InlineData("[move:to 0]")]
        [InlineData("[move:a;b]")]
        [InlineData("[move:10;0]")]
        [InlineData("[move:0;10]")]
        [InlineData("[move:-1;5]")]
        [InlineData("[move:1;-5]")]
        [InlineData("[move:]]")]
        [InlineData("[move:unknown]")]
        public void Should_Throw_For_Invalid_Move_Directives(string text)
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            var exception = Record.Exception(() => fixture.Markup.Write(text));

            // Then
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidOperationException>();
            exception.Message.ShouldContain("Could not parse move directive");
        }

        [Theory]
        [InlineData("[escnext][clear:to eol]", "")]
        [InlineData("[escnext][move:up 2]", "")]
        [InlineData("[escnext][escnext]", "[escnext]")]
        public void Should_Skip_Self_Closing_Markup_When_Escaped(string text, string expected)
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Markup.Write(text);

            // Then
            fixture.Output
                .ShouldBe(expected);
        }

        [Theory]
        [InlineData("[wnext][clear:to eol]", "[clear:to eol]")]
        [InlineData("[wnext][move:up 2]", "[move:up 2]")]
        [InlineData("[wnext][wnext]", "[wnext]")]
        public void Should_Write_Next_Markup_As_Raw_Text_When_Wnext(string text, string expected)
        {
            // Given
            var fixture = new AnsiFixture();

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
                "\e]8;id=[0-9]*;https:\\/\\/patriksvensson\\.se\e\\\\Click to visit my blog\e]8;;\e\\\\");
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
                "\e]8;id=[0-9]*;https:\\/\\/patriksvensson\\.se\e\\\\https:\\/\\/patriksvensson\\.se\e]8;;\e\\\\");
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
                "\e]8;id=[0-9]*;file:\\/\\/c:\\/temp\\/\\[x\\].txt\e\\\\file:\\/\\/c:\\/temp\\/\\[x\\].txt\e]8;;\e\\\\");
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
                "\e]8;id=[0-9]*;file:\\/\\/c:\\/temp\\/\\[x\\].txt\e\\\\file:\\/\\/c:\\/temp\\/\\[x\\].txt\e]8;;\e\\\\");
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