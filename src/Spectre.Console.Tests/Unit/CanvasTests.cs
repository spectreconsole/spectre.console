using System;
using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/Canvas")]
    public class CanvasTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Width_Is_Less_Than_Zero()
            {
                // Given, When
                var result = Record.Exception(() => new Canvas(0, 1));

                // Then
                result.ShouldBeOfType<ArgumentException>()
                    .And(ex => ex.ParamName.ShouldBe("width"));
            }

            [Fact]
            public void Should_Throw_If_Height_Is_Less_Than_Zero()
            {
                // Given, When
                var result = Record.Exception(() => new Canvas(1, 0));

                // Then
                result.ShouldBeOfType<ArgumentException>()
                    .And(ex => ex.ParamName.ShouldBe("height"));
            }
        }

        [Fact]
        [Expectation("Render")]
        public async Task Should_Render_Canvas_Correctly()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            var canvas = new Canvas(width: 5, height: 5);
            canvas.SetPixel(0, 0, Color.Red);
            canvas.SetPixel(4, 0, Color.Green);
            canvas.SetPixel(0, 4, Color.Blue);
            canvas.SetPixel(4, 4, Color.Yellow);

            // When
            console.Write(canvas);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render_Nested")]
        public async Task Simple_Measure()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            var panel = new Panel(new Canvas(width: 2, height: 2)
                .SetPixel(0, 0, Color.Aqua)
                .SetPixel(1, 1, Color.Grey));

            // When
            console.Write(panel);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render_NarrowTerminal")]
        public async Task Should_Scale_Down_Canvas_Is_Bigger_Than_Terminal()
        {
            // Given
            var console = new TestConsole()
                .Width(10)
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            var canvas = new Canvas(width: 20, height: 10);
            canvas.SetPixel(0, 0, Color.Aqua);
            canvas.SetPixel(19, 9, Color.Grey);

            // When
            console.Write(canvas);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render_MaxWidth")]
        public async Task Should_Scale_Down_Canvas_If_MaxWidth_Is_Set()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            var canvas = new Canvas(width: 20, height: 10) { MaxWidth = 10 };
            canvas.SetPixel(0, 0, Color.Aqua);
            canvas.SetPixel(19, 9, Color.Aqua);

            // When
            console.Write(canvas);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        public void Should_Not_Render_Canvas_If_Canvas_Cannot_Be_Scaled_Down()
        {
            // Given
            var console = new TestConsole()
                .Width(10)
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            var canvas = new Canvas(width: 20, height: 2);
            canvas.SetPixel(0, 0, Color.Aqua);
            canvas.SetPixel(19, 1, Color.Grey);

            // When
            console.Write(canvas);

            // Then
            console.Output.ShouldBeEmpty();
        }
    }
}