namespace Spectre.Console.Tests.Unit;

public partial class AnsiConsoleTests
{
    public sealed class Markup
    {
        [Theory]
        [InlineData("[yellow]Hello[/]", "\e[93mHello\e[0m")]
        [InlineData("[yellow]Hello [italic]World[/]![/]", "\e[93mHello \e[0m\e[3;93mWorld\e[0m\e[93m!\e[0m")]
        public void Should_Output_Expected_Ansi_For_Markup(string markup, string expected)
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.Markup(markup);

            // Then
            console.Output.ShouldBe(expected);
        }

        [Fact]
        public void Should_Output_Expected_Ansi_For_Link_With_Url_And_Text()
        {
            // Given
            var console = new TestConsole()
                .EmitAnsiSequences();

            // When
            console.Markup("[link=https://patriksvensson.se]Click to visit my blog[/]");

            // Then
            console.Output.ShouldMatch(
                "\e]8;id=[0-9]*;https:\\/\\/patriksvensson\\.se\e\\\\Click to visit my blog\e]8;;\e\\\\");
        }

        [Fact]
        public void Should_Output_Expected_Ansi_For_Link_With_Only_Url()
        {
            // Given
            var console = new TestConsole()
                .EmitAnsiSequences();

            // When
            console.Markup("[link]https://patriksvensson.se[/]");

            // Then
            console.Output.ShouldMatch(
                "\e]8;id=[0-9]*;https:\\/\\/patriksvensson\\.se\e\\\\https:\\/\\/patriksvensson\\.se\e]8;;\e\\\\");
        }

        [Fact]
        public void Should_Output_Expected_Ansi_For_Link_With_Bracket_In_Url_Only()
        {
            // Given
            var console = new TestConsole()
                .EmitAnsiSequences();

            // When
            const string Path = "file://c:/temp/[x].txt";
            console.Markup($"[link]{Path.EscapeMarkup()}[/]");

            // Then
            console.Output.ShouldMatch(
                "\e]8;id=[0-9]*;file:\\/\\/c:\\/temp\\/\\[x\\].txt\e\\\\file:\\/\\/c:\\/temp\\/\\[x\\].txt\e]8;;\e\\\\");
        }

        [Fact]
        public void Should_Output_Expected_Ansi_For_Link_With_Bracket_In_Url()
        {
            // Given
            var console = new TestConsole()
                .EmitAnsiSequences();

            // When
            const string Path = "file://c:/temp/[x].txt";
            console.Markup($"[link={Path.EscapeMarkup()}]{Path.EscapeMarkup()}[/]");

            // Then
            console.Output.ShouldMatch(
                "\e]8;id=[0-9]*;file:\\/\\/c:\\/temp\\/\\[x\\].txt\e\\\\file:\\/\\/c:\\/temp\\/\\[x\\].txt\e]8;;\e\\\\");
        }

        [Theory]
        [InlineData("[yellow]Hello [[ World[/]", "\e[93mHello [ World\e[0m")]
        public void Should_Be_Able_To_Escape_Tags(string markup, string expected)
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.Markup(markup);

            // Then
            console.Output.ShouldBe(expected);
        }

        [Theory]
        [InlineData("[yellow]Hello[", "Encountered malformed markup tag at position 14.")]
        [InlineData("[yellow]Hello[/", "Encountered malformed markup tag at position 15.")]
        [InlineData("[yellow]Hello[/foo", "Encountered malformed markup tag at position 15.")]
        [InlineData("[yellow Hello", "Encountered malformed markup tag at position 13.")]
        [InlineData("[yellow[green]]Hello", "Encountered malformed markup tag at position 7.")]
        public void Should_Throw_If_Encounters_Malformed_Tag(string markup, string expected)
        {
            // Given
            var console = new TestConsole();

            // When
            var result = Record.Exception(() => console.Markup(markup));

            // Then
            result.ShouldBeOfType<InvalidOperationException>()
                .Message.ShouldBe(expected);
        }

        [Fact]
        public void Should_Throw_If_Tags_Are_Unbalanced()
        {
            // Given
            var console = new TestConsole();

            // When
            var result = Record.Exception(() => console.Markup("[yellow][blue]Hello[/]"));

            // Then
            result.ShouldBeOfType<InvalidOperationException>()
                .Message.ShouldBe("Unbalanced markup stack. Did you forget to close a tag?");
        }

        [Fact]
        public void Should_Throw_If_Encounters_Closing_Tag()
        {
            // Given
            var console = new TestConsole();

            // When
            var result = Record.Exception(() => console.Markup("Hello[/]World"));

            // Then
            result.ShouldBeOfType<InvalidOperationException>()
                .Message.ShouldBe("Encountered closing tag when none was expected near position 5.");
        }

        [Fact]
        public void Should_Not_Get_Confused_When_Mixing_Escaped_And_Unescaped()
        {
            // Given
            var console = new TestConsole();

            // When
            console.Markup("[grey][[grey]][/][white][[white]][/]");

            // Then
            console.Output.ShouldBe("[grey][white]");
        }

        [Theory]
        [InlineData("[white][[[/][white]]][/]", "[]")]
        [InlineData("[white][[[/]", "[")]
        [InlineData("[white]]][/]", "]")]
        [InlineData("[black on white link=https://www.gooole.com/q=]]]Search for a bracket[/]", "Search for a bracket")]
        [InlineData("[link=https://www.gooole.com/q=]] black on white]Search for a bracket[/]", "Search for a bracket")]
        [InlineData("[link]https://www.gooole.com/q=]][/]", "https://www.gooole.com/q=]")]
        public void Should_Not_Fail_As_In_GH1024(string markup, string expected)
        {
            // Given
            var console = new TestConsole();
            console.EmitAnsiSequences = false;

            // When
            console.Markup(markup);

            // Then
            console.Output.ShouldBe(expected);
        }

        [Fact]
        [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/2083")]
        [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/2078")]
        public void Should_Preserve_Links_When_Multiple_Segments_Are_Merged()
        {
            // Given
            var console = new TestConsole()
                .Width(8)
                .SupportsAnsi(true)
                .EmitAnsiSequences();

            // When
            console.Write(
                Align.Center(
                    new Spectre.Console.Markup(
                        "[link=https://example.com/readme.md]Docs[/]")));

            // Then
            console.Output.NormalizeLineEndings().ShouldMatch(
                "  \e]8;id=[0-9]*;https:\\/\\/example\\.com\\/readme.md\e\\\\Docs\e]8;;\e\\\\  ");
        }

        [Fact]
        [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/2083")]
        [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/2078")]
        public void Should_Preserve_Links_Over_Line_Breaks_When_Multiple_Segments_Are_Merged()
        {
            // Given
            var console = new TestConsole()
                .Width(8)
                .SupportsAnsi(true)
                .EmitAnsiSequences();

            // When
            console.Write(
                Align.Center(
                    new Spectre.Console.Markup(
                        "[link=https://example.com/readme.md]Foo and Bar[/]")));

            // Then
            console.Output.NormalizeLineEndings().ShouldMatch(
                "\e]8;id=[0-9]*;https:\\/\\/example\\.com\\/readme.md\e\\\\Foo and \e]8;;\e\\\\\n" +
                "  \e]8;id=[0-9]*;https:\\/\\/example\\.com\\/readme.md\e\\\\Bar\e]8;;\e\\\\   ");
        }

        [Fact]
        public void Should_Not_Apply_Link_To_Text_After_Link_Close_Tag()
        {
            // Given
            var console = new TestConsole()
                .EmitAnsiSequences();

            // When - text after the [/] closing tag should NOT have the link
            console.Markup("Before [link=https://example.com]LINK[/] After");

            // Then
            // The link should only wrap "LINK", not " After"
            var output = console.Output;
            output.ShouldMatch(@"Before \e\]8;id=\d+;https://example\.com\e\\LINK\e\]8;;\e\\ After");
        }

        [Fact]
        public void Should_Properly_Handle_Nested_Link_With_Styles()
        {
            // Given
            var console = new TestConsole()
                .EmitAnsiSequences();

            // When - link with styled text inside
            console.Markup("[link=https://example.com][bold]Bold Link[/][/] Plain");

            // Then
            // The link should only wrap "Bold Link", not " Plain"
            var output = console.Output;

            // Check that "Plain" is NOT inside a link
            var linkEndIndex = output.LastIndexOf("\u001b]8;;\u001b\\");
            var plainIndex = output.IndexOf("Plain");
            plainIndex.ShouldBeGreaterThan(linkEndIndex, "Plain should appear after the link ends");
        }
    }
}