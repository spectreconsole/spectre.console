namespace Spectre.Console.Tests.Unit;

public partial class AnsiConsoleTests
{
    public sealed class Markup
    {
        [Theory]
        [InlineData("[yellow]Hello[/]", "[93mHello[0m")]
        [InlineData("[yellow]Hello [italic]World[/]![/]", "[93mHello [0m[3;93mWorld[0m[93m![0m")]
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
            console.Output.ShouldMatch("]8;id=[0-9]*;https:\\/\\/patriksvensson\\.se\\\\Click to visit my blog]8;;\\\\");
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
            console.Output.ShouldMatch("]8;id=[0-9]*;https:\\/\\/patriksvensson\\.se\\\\https:\\/\\/patriksvensson\\.se]8;;\\\\");
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
            console.Output.ShouldMatch("]8;id=[0-9]*;file:\\/\\/c:\\/temp\\/\\[x\\].txt\\\\file:\\/\\/c:\\/temp\\/\\[x\\].txt]8;;\\\\");
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
            console.Output.ShouldMatch("]8;id=[0-9]*;file:\\/\\/c:\\/temp\\/\\[x\\].txt\\\\file:\\/\\/c:\\/temp\\/\\[x\\].txt]8;;\\\\");
        }

        [Theory]
        [InlineData("[yellow]Hello [[ World[/]", "[93mHello [ World[0m")]
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
    }
}
