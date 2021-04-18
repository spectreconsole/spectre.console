using System.Threading.Tasks;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/BreakdownChart")]
    public sealed class BreakdownChartTests
    {
        [Fact]
        [Expectation("Default")]
        public async Task Should_Render_Correctly()
        {
            // Given
            var console = new TestConsole();
            var chart = Fixture.GetChart();

            // When
            console.Write(chart);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Width")]
        public async Task Should_Render_With_Specific_Width()
        {
            // Given
            var console = new TestConsole();
            var chart = Fixture.GetChart().Width(60);

            // When
            console.Write(chart);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("TagFormat")]
        public async Task Should_Render_Correctly_With_Specific_Value_Formatter()
        {
            // Given
            var console = new TestConsole();
            var chart = Fixture.GetChart()
                .Width(60)
                .Culture("sv-SE")
                .UseValueFormatter((v, c) => string.Format(c, "{0}%", v));

            // When
            console.Write(chart);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("HideTags")]
        public async Task Should_Render_Correctly_Without_Tags()
        {
            // Given
            var console = new TestConsole();
            var chart = Fixture.GetChart().Width(60).HideTags();

            // When
            console.Write(chart);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("HideTagValues")]
        public async Task Should_Render_Correctly_Without_Tag_Values()
        {
            // Given
            var console = new TestConsole();
            var chart = Fixture.GetChart().Width(60).HideTagValues();

            // When
            console.Write(chart);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Culture")]
        public async Task Should_Render_Correctly_With_Specific_Culture()
        {
            // Given
            var console = new TestConsole();
            var chart = Fixture.GetChart().Width(60).Culture("sv-SE");

            // When
            console.Write(chart);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("FullSize")]
        public async Task Should_Render_FullSize_Mode_Correctly()
        {
            // Given
            var console = new TestConsole();
            var chart = Fixture.GetChart().Width(60).FullSize();

            // When
            console.Write(chart);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Ansi")]
        public async Task Should_Render_Correct_Ansi()
        {
            // Given
            var console = new TestConsole().EmitAnsiSequences();
            var chart = Fixture.GetChart().Width(60).FullSize();

            // When
            console.Write(chart);

            // Then
            await Verifier.Verify(console.Output);
        }

        public static class Fixture
        {
            public static BreakdownChart GetChart()
            {
                return new BreakdownChart()
                    .AddItem("SCSS", 37, Color.Red)
                    .AddItem("HTML", 28.3, Color.Blue)
                    .AddItem("C#", 22.6, Color.Green)
                    .AddItem("JavaScript", 6, Color.Yellow)
                    .AddItem("Ruby", 6, Color.LightGreen)
                    .AddItem("Shell", 0.1, Color.Aqua);
            }
        }
    }
}
