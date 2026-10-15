namespace Spectre.Console.Tests.Unit;

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

    [Fact]
    [Expectation("ValueColor")]
    public async Task Should_Render_Correct_ValueColor()
    {
        // Given
        var console = new TestConsole().EmitAnsiSequences();
        var chart = Fixture.GetChart().Width(60).WithValueColor(Color.Red);

        // When
        console.Write(chart);

        // Then
        await Verifier.Verify(console.Output);
    }

    [Fact]
    public void Should_Not_Render_Bar_When_All_Values_Are_Zero()
    {
        // Given
        var console = new TestConsole().Width(40);
        var chart = new BreakdownChart()
            .AddItem("A", 0, Color.Red)
            .AddItem("B", 0, Color.Blue);

        // When
        console.Write(chart);

        // Then
        console.Output.ShouldNotContain("█");
        console.Output.ShouldContain("■ A 0");
        console.Output.ShouldContain("■ B 0");
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