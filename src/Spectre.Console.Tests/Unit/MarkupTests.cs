using System;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class MarkupTests
    {
        [Theory]
        [InlineData("Hello [[ World ]")]
        [InlineData("Hello [[ World ] !")]
        public void Should_Throw_If_Closing_Tag_Is_Not_Properly_Escaped(string input)
        {
            // Given
            var console = new PlainConsole();

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
            var console = new PlainConsole();
            var markup = new Markup("Hello [[ World ]] !");

            // When
            console.Render(markup);

            // Then
            console.Output.ShouldBe("Hello [ World ] !");
        }

        [Theory]
        [InlineData("Hello [link=http://example.com]example.com[/]", "Hello example.com")]
        [InlineData("Hello [link=http://example.com]http://example.com[/]", "Hello http://example.com")]
        public void Should_Render_Links_As_Expected(string input, string output)
        {
            // Given
            var console = new PlainConsole();
            var markup = new Markup(input);

            // When
            console.Render(markup);

            // Then
            console.Output.ShouldBe(output);
        }
    }
}
