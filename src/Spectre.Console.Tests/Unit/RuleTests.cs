using System.Threading.Tasks;
using Shouldly;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class RuleTests
    {
        [Fact]
        public Task Should_Render_Default_Rule_Without_Title()
        {
            // Given
            var console = new PlainConsole(width: 40);

            // When
            console.Render(new Rule());

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Render_Default_Rule_With_Specified_Box()
        {
            // Given
            var console = new PlainConsole(width: 40);

            // When
            console.Render(new Rule().DoubleBorder());

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Render_With_Specified_Box()
        {
            // Given
            var console = new PlainConsole(width: 40);

            // When
            console.Render(new Rule("Hello World").DoubleBorder());

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Render_Default_Rule_With_Title_Centered_By_Default()
        {
            // Given
            var console = new PlainConsole(width: 40);

            // When
            console.Render(new Rule("Hello World"));

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Render_Default_Rule_With_Title_Left_Aligned()
        {
            // Given
            var console = new PlainConsole(width: 40);

            // When
            console.Render(new Rule("Hello World")
            {
                Alignment = Justify.Left,
            });

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Render_Default_Rule_With_Title_Right_Aligned()
        {
            // Given
            var console = new PlainConsole(width: 40);

            // When
            console.Render(new Rule("Hello World")
            {
                Alignment = Justify.Right,
            });

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Convert_Line_Breaks_In_Title_To_Spaces()
        {
            // Given
            var console = new PlainConsole(width: 40);

            // When
            console.Render(new Rule("Hello\nWorld\r\n!"));

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Truncate_Title()
        {
            // Given
            var console = new PlainConsole(width: 40);

            // When
            console.Render(new Rule("          Hello World    "));

            // Then
            return Verifier.Verify(console.Output);
        }

        [Theory]
        [InlineData(0, "Hello World Hello World Hello World Hello World Hello World", "")]
        [InlineData(1, "Hello World Hello World Hello World Hello World Hello World", "─")]
        [InlineData(2, "Hello World Hello World Hello World Hello World Hello World", "──")]
        [InlineData(3, "Hello World Hello World Hello World Hello World Hello World", "───")]
        [InlineData(4, "Hello World Hello World Hello World Hello World Hello World", "────")]
        [InlineData(5, "Hello World Hello World Hello World Hello World Hello World", "─────")]
        [InlineData(6, "Hello World Hello World Hello World Hello World Hello World", "──────")]
        [InlineData(7, "Hello World Hello World Hello World Hello World Hello World", "───────")]
        [InlineData(8, "Hello World Hello World Hello World Hello World Hello World", "── H… ──")]
        [InlineData(8, "A", "── A ───")]
        [InlineData(8, "AB", "── AB ──")]
        [InlineData(8, "ABC", "── A… ──")]
        [InlineData(40, "Hello World Hello World Hello World Hello World Hello World", "──── Hello World Hello World Hello… ────")]
        public void Should_Truncate_Too_Long_Title(int width, string input, string expected)
        {
            // Given
            var console = new PlainConsole(width);

            // When
            console.Render(new Rule(input));

            // Then
            console.Lines.Count.ShouldBe(1);
            console.Lines[0].ShouldBe(expected);
        }
    }
}
