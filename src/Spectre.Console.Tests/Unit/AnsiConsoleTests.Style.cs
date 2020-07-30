using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public partial class AnsiConsoleTests
    {
        [Theory]
        [InlineData(Styles.Bold, "\u001b[1mHello World[0m")]
        [InlineData(Styles.Dim, "\u001b[2mHello World[0m")]
        [InlineData(Styles.Italic, "\u001b[3mHello World[0m")]
        [InlineData(Styles.Underline, "\u001b[4mHello World[0m")]
        [InlineData(Styles.Invert, "\u001b[7mHello World[0m")]
        [InlineData(Styles.Conceal, "\u001b[8mHello World[0m")]
        [InlineData(Styles.SlowBlink, "\u001b[5mHello World[0m")]
        [InlineData(Styles.RapidBlink, "\u001b[6mHello World[0m")]
        [InlineData(Styles.Strikethrough, "\u001b[9mHello World[0m")]
        public void Should_Write_Style_Correctly(Styles style, string expected)
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.TrueColor);
            fixture.Console.Style = style;

            // When
            fixture.Console.Write("Hello World");

            // Then
            fixture.Output.ShouldBe(expected);
        }

        [Theory]
        [InlineData(Styles.Bold | Styles.Underline, "\u001b[1;4mHello World[0m")]
        [InlineData(Styles.Bold | Styles.Underline | Styles.Conceal, "\u001b[1;4;8mHello World[0m")]
        public void Should_Write_Combined_Styles_Correctly(Styles style, string expected)
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.TrueColor);
            fixture.Console.Style = style;

            // When
            fixture.Console.Write("Hello World");

            // Then
            fixture.Output.ShouldBe(expected);
        }
    }
}
