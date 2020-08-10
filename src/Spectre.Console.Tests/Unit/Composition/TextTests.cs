using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class TextTests
    {
        [Fact]
        public void Should_Render_Unstyled_Text_As_Expected()
        {
            // Given
            var fixture = new PlainConsole(width: 80);
            var text = Text.New("Hello World");

            // When
            fixture.Render(text);

            // Then
            fixture.Output
                .NormalizeLineEndings()
                .ShouldBe("Hello World");
        }

        [Fact]
        public void Should_Write_Line_Breaks()
        {
            // Given
            var fixture = new PlainConsole(width: 5);
            var text = Text.New("\n\n");

            // When
            fixture.Render(text);

            // Then
            fixture.RawOutput.ShouldBe("\n\n");
        }

        [Fact]
        public void Should_Split_Unstyled_Text_To_New_Lines_If_Width_Exceeds_Console_Width()
        {
            // Given
            var fixture = new PlainConsole(width: 5);
            var text = Text.New("Hello World");

            // When
            fixture.Render(text);

            // Then
            fixture.Output
                .NormalizeLineEndings()
                .ShouldBe("Hello\n Worl\nd");
        }

        public sealed class TheStylizeMethod
        {
            [Fact]
            public void Should_Apply_Style_To_Text()
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard);
                var text = Text.New("Hello World");
                text.Stylize(start: 3, end: 8, new Style(decoration: Decoration.Underline));

                // When
                fixture.Console.Render(text);

                // Then
                fixture.Output
                    .NormalizeLineEndings()
                    .ShouldBe("Hel[4mlo Wo[0mrld");
            }

            [Fact]
            public void Should_Apply_Style_To_Text_Which_Spans_Over_Multiple_Lines()
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, width: 5);
                var text = Text.New("Hello World");
                text.Stylize(start: 3, end: 8, new Style(decoration: Decoration.Underline));

                // When
                fixture.Console.Render(text);

                // Then
                fixture.Output
                    .NormalizeLineEndings()
                    .ShouldBe("Hel[4mlo[0m\n[4m Wo[0mrl\nd");
            }
        }
    }
}
