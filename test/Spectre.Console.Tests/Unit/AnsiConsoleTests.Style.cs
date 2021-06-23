using Shouldly;
using Spectre.Console.Testing;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public partial class AnsiConsoleTests
    {
        [Theory]
        [InlineData(Decoration.Bold, "\u001b[1mHello World[0m")]
        [InlineData(Decoration.Dim, "\u001b[2mHello World[0m")]
        [InlineData(Decoration.Italic, "\u001b[3mHello World[0m")]
        [InlineData(Decoration.Underline, "\u001b[4mHello World[0m")]
        [InlineData(Decoration.Invert, "\u001b[7mHello World[0m")]
        [InlineData(Decoration.Conceal, "\u001b[8mHello World[0m")]
        [InlineData(Decoration.SlowBlink, "\u001b[5mHello World[0m")]
        [InlineData(Decoration.RapidBlink, "\u001b[6mHello World[0m")]
        [InlineData(Decoration.Strikethrough, "\u001b[9mHello World[0m")]
        public void Should_Write_Decorated_Text_Correctly(Decoration decoration, string expected)
        {
            // Given
            var console = new TestConsole()
                .EmitAnsiSequences();

            // When
            console.Write("Hello World", new Style().Decoration(decoration));

            // Then
            console.Output.ShouldBe(expected);
        }

        [Theory]
        [InlineData(Decoration.Bold | Decoration.Underline, "\u001b[1;4mHello World[0m")]
        [InlineData(Decoration.Bold | Decoration.Underline | Decoration.Conceal, "\u001b[1;4;8mHello World[0m")]
        public void Should_Write_Text_With_Multiple_Decorations_Correctly(Decoration decoration, string expected)
        {
            // Given
            var console = new TestConsole()
                .EmitAnsiSequences();

            // When
            console.Write("Hello World", new Style().Decoration(decoration));

            // Then
            console.Output.ShouldBe(expected);
        }
    }
}
