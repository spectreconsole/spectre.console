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
            var fixture = new PlainConsole();

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
            var fixture = new PlainConsole();
            var markup = new Markup("Hello [[ World ]] !");

            // When
            fixture.Render(markup);

            // Then
            fixture.Output.ShouldBe("Hello [ World ] !");
        }
    }
}
