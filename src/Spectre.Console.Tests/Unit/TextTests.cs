using System.Text;
using Shouldly;
using Spectre.Console.Composition;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class TextTests
    {
        public sealed class Measuring
        {
            [Fact]
            public void Should_Return_The_Longest_Word_As_Minimum_Width()
            {
                var text = new Text("Foo Bar Baz\nQux\nLol mobile");

                var result = text.Measure(new RenderContext(Encoding.Unicode, false), 80);

                result.Min.ShouldBe(6);
            }

            [Fact]
            public void Should_Return_The_Longest_Line_As_Maximum_Width()
            {
                var text = new Text("Foo Bar Baz\nQux\nLol mobile");

                var result = text.Measure(new RenderContext(Encoding.Unicode, false), 80);

                result.Max.ShouldBe(11);
            }
        }

        public sealed class Rendering
        {
            [Fact]
            public void Should_Render_Unstyled_Text_As_Expected()
            {
                // Given
                var fixture = new PlainConsole(width: 80);
                var text = new Text("Hello World");

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
                var text = new Text("Hello\n\nWorld");

                // When
                fixture.Render(text);

                // Then
                fixture.RawOutput.ShouldBe("Hello\n\nWorld");
            }

            [Fact]
            public void Should_Write_Line_Breaks_At_End()
            {
                // Given
                var fixture = new PlainConsole(width: 5);
                var text = new Text("Hello\n\nWorld\n\n");

                // When
                fixture.Render(text);

                // Then
                fixture.RawOutput.ShouldBe("Hello\n\nWorld\n\n");
            }

            [Theory]
            [InlineData(5, "Hello World", "Hello\nWorld")]
            [InlineData(10, "Hello Sweet Nice World", "Hello \nSweet Nice\nWorld")]
            public void Should_Split_Unstyled_Text_To_New_Lines_If_Width_Exceeds_Console_Width(
                int width, string input, string expected)
            {
                // Given
                var fixture = new PlainConsole(width);
                var text = new Text(input);

                // When
                fixture.Render(text);

                // Then
                fixture.Output
                    .NormalizeLineEndings()
                    .ShouldBe(expected);
            }
        }
    }
}
