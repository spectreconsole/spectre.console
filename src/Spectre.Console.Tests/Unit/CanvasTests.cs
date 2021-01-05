using System;
using System.Linq;
using System.Text;
using Shouldly;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public class CanvasTests
    {
        [Fact]
        public void Canvas_Must_Have_Proper_Size()
        {
            Should.Throw<ArgumentException>(() => new Canvas(1, 0));

            Should.Throw<ArgumentException>(() => new Canvas(0, 1));
        }

        [Fact]
        public void Simple_Render()
        {
            // Given
            var console = new FakeAnsiConsole(ColorSystem.Standard);
            var canvas = new Canvas(width: 2, height: 2);
            canvas.SetPixel(0, 0, Color.Aqua);
            canvas.SetPixel(1, 1, Color.Grey);

            // When
            console.Render(canvas);

            // Then
            console.Output.ShouldBe($"\u001b[106m  \u001b[0m  {Environment.NewLine}  \u001b[100m  \u001b[0m{Environment.NewLine}");
        }

        [Fact]
        public void Render_Wider_Than_Terminal_Cannot_Be_Reduced_Further()
        {
            // Given
            var console = new FakeAnsiConsole(ColorSystem.Standard, width: 10);
            var canvas = new Canvas(width: 20, height: 2);
            canvas.SetPixel(0, 0, Color.Aqua);
            canvas.SetPixel(19, 1, Color.Grey);

            // When
            console.Render(canvas);

            // Then
            console.Output.ShouldBe(string.Empty);
        }

        [Fact]
        public void Render_Wider_Than_Terminal()
        {
            // Given
            var console = new FakeAnsiConsole(ColorSystem.Standard, width: 10);
            var canvas = new Canvas(width: 20, height: 10);
            canvas.SetPixel(0, 0, Color.Aqua);
            canvas.SetPixel(19, 9, Color.Grey);

            // When
            console.Render(canvas);

            // Then
            var numNewlines = console.Output.Count(x => x == '\n');

            // Small terminal shrinks the canvas
            numNewlines.ShouldBe(expected: 2);
        }

        [Fact]
        public void Render_Wider_Configured_With_Max_Width()
        {
            // Given
            var console = new FakeAnsiConsole(ColorSystem.Standard, width: 80);
            var canvas = new Canvas(width: 20, height: 10) { MaxWidth = 10 };
            canvas.SetPixel(0, 0, Color.Aqua);
            canvas.SetPixel(19, 9, Color.Grey);

            // When
            console.Render(canvas);

            // Then
            var numNewlines = console.Output.Count(x => x == '\n');

            // MaxWidth truncates the canvas
            numNewlines.ShouldBe(expected: 5);
        }

        [Fact]
        public void Simple_Measure()
        {
            // Given
            var console = new FakeAnsiConsole(ColorSystem.Standard);
            var canvas = new Canvas(width: 2, height: 2);
            canvas.SetPixel(0, 0, Color.Aqua);
            canvas.SetPixel(1, 1, Color.Grey);

            // When
            var measurement = ((IRenderable)canvas).Measure(new RenderContext(Encoding.Unicode, false), 80);

            // Then
            measurement.Max.ShouldBe(expected: 4);
            measurement.Min.ShouldBe(expected: 4);
        }
    }
}