using Shouldly;
using Spectre.Console.Composition;
using Xunit;

namespace Spectre.Console.Tests.Composition
{
    public sealed class TextTests
    {
        [Fact]
        public void Should_Render_Text_To_Console()
        {
            // Given
            var console = new PlainConsole();

            // When
            console.Render(new Text("Hello World"));

            // Then
            console.Output.ShouldBe("Hello World");
        }

        [Fact]
        public void Should_Right_Align_Text_To_Parent()
        {
            // Given
            var console = new PlainConsole(width: 15);

            // When
            console.Render(new Text("Hello World", justify: Justify.Right));

            // Then
            console.Output.ShouldBe("    Hello World");
        }

        [Fact]
        public void Should_Center_Text_To_Parent()
        {
            // Given
            var console = new PlainConsole(width: 15);

            // When
            console.Render(new Text("Hello World", justify: Justify.Center));

            // Then
            console.Output.ShouldBe("  Hello World  ");
        }

        [Fact]
        public void Should_Split_Text_To_Multiple_Lines_If_It_Does_Not_Fit()
        {
            // Given
            var console = new PlainConsole(width: 5);

            // When
            console.Render(new Text("Hello World"));

            // Then
            console.Output.ShouldBe("Hello\n Worl\nd");
        }
    }
}
