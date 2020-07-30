using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public partial class AnsiConsoleTests
    {
        public sealed class TrueColor
        {
            [Theory]
            [InlineData(true, "\u001b[38;2;128;0;128mHello\u001b[0m")]
            [InlineData(false, "\u001b[48;2;128;0;128mHello\u001b[0m")]
            public void Should_Return_Correct_Code(bool foreground, string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.TrueColor);
                fixture.Console.SetColor(new Color(128, 0, 128), foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }

            [Theory]
            [InlineData(true, "\u001b[38;5;5mHello\u001b[0m")]
            [InlineData(false, "\u001b[48;5;5mHello\u001b[0m")]
            public void Should_Return_Eight_Bit_Ansi_Code_For_Known_Colors(bool foreground, string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.TrueColor);
                fixture.Console.SetColor(Color.Purple, foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }
        }

        public sealed class EightBit
        {
            [Theory]
            [InlineData(true, "\u001b[38;5;3mHello\u001b[0m")]
            [InlineData(false, "\u001b[48;5;3mHello\u001b[0m")]
            public void Should_Return_Correct_Code_For_Known_Color(bool foreground, string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.EightBit);
                fixture.Console.SetColor(Color.Olive, foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }

            [Theory]
            [InlineData(true, "\u001b[38;5;3mHello\u001b[0m")]
            [InlineData(false, "\u001b[48;5;3mHello\u001b[0m")]
            public void Should_Map_TrueColor_To_Nearest_Eight_Bit_Color_If_Possible(bool foreground, string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.EightBit);
                fixture.Console.SetColor(new Color(128, 128, 0), foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }

            [Theory]
            [InlineData(true, "\u001b[38;5;3mHello\u001b[0m")]
            [InlineData(false, "\u001b[48;5;3mHello\u001b[0m")]
            public void Should_Estimate_TrueColor_To_Nearest_Eight_Bit_Color(bool foreground, string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.EightBit);
                fixture.Console.SetColor(new Color(126, 127, 0), foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }
        }

        public sealed class Standard
        {
            [Theory]
            [InlineData(true, "\u001b[33mHello\u001b[0m")]
            [InlineData(false, "\u001b[43mHello\u001b[0m")]
            public void Should_Return_Correct_Code_For_Known_Color(bool foreground, string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard);
                fixture.Console.SetColor(Color.Olive, foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }

            [Theory]
            [InlineData(true, 128, 128, 128, "\u001b[90mHello\u001b[0m")]
            [InlineData(false, 128, 128, 128, "\u001b[100mHello\u001b[0m")]
            [InlineData(true, 0, 128, 0, "\u001b[32mHello\u001b[0m")]
            [InlineData(false, 0, 128, 0, "\u001b[42mHello\u001b[0m")]
            public void Should_Map_TrueColor_To_Nearest_Four_Bit_Color_If_Possible(
                bool foreground,
                byte r, byte g, byte b,
                string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard);
                fixture.Console.SetColor(new Color(r, g, b), foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }

            [Theory]
            [InlineData(true, 112, 120, 128, "\u001b[90mHello\u001b[0m")]
            [InlineData(false, 112, 120, 128, "\u001b[100mHello\u001b[0m")]
            [InlineData(true, 0, 120, 12, "\u001b[32mHello\u001b[0m")]
            [InlineData(false, 0, 120, 12, "\u001b[42mHello\u001b[0m")]
            public void Should_Estimate_TrueColor_To_Nearest_Four_Bit_Color(
                bool foreground,
                byte r, byte g, byte b,
                string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard);
                fixture.Console.SetColor(new Color(r, g, b), foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }
        }

        public sealed class Legacy
        {
            [Theory]
            [InlineData(true, "\u001b[33mHello\u001b[0m")]
            [InlineData(false, "\u001b[43mHello\u001b[0m")]
            public void Should_Return_Correct_Code_For_Known_Color(bool foreground, string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Legacy);
                fixture.Console.SetColor(Color.Olive, foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }

            [Theory]
            [InlineData(true, 128, 128, 128, "\u001b[37mHello\u001b[0m")]
            [InlineData(false, 128, 128, 128, "\u001b[47mHello\u001b[0m")]
            [InlineData(true, 0, 128, 0, "\u001b[32mHello\u001b[0m")]
            [InlineData(false, 0, 128, 0, "\u001b[42mHello\u001b[0m")]
            public void Should_Map_TrueColor_To_Nearest_Three_Bit_Color_If_Possible(
                bool foreground,
                byte r, byte g, byte b,
                string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Legacy);
                fixture.Console.SetColor(new Color(r, g, b), foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }

            [Theory]
            [InlineData(true, 112, 120, 128, "\u001b[36mHello\u001b[0m")]
            [InlineData(false, 112, 120, 128, "\u001b[46mHello\u001b[0m")]
            [InlineData(true, 0, 120, 12, "\u001b[32mHello\u001b[0m")]
            [InlineData(false, 0, 120, 12, "\u001b[42mHello\u001b[0m")]
            public void Should_Estimate_TrueColor_To_Nearest_Three_Bit_Color(
                bool foreground,
                byte r, byte g, byte b,
                string expected)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Legacy);
                fixture.Console.SetColor(new Color(r, g, b), foreground);

                // When
                fixture.Console.Write("Hello");

                // Then
                fixture.Output.ShouldBe(expected);
            }
        }
    }
}
