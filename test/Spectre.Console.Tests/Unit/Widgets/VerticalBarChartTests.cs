namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("Widgets/VerticalBarChart")]
public sealed class VerticalBarChartTests
{
    [Fact]
    [Expectation("Render")]
    public async Task Should_Render_Correctly()
    {
        // Given
        var console = new TestConsole()
            .Colors(ColorSystem.Standard);

        // When
        var chart = new VerticalBarChart()
            .SetHeight(6)
            .SetData(new double[] { 4, 2, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 2, 3, 4, 5, 4, 3, 5, 8, 12 });
        console.Write(chart);

        // Then
        await Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render.Negative")]
    public async Task Should_Render_Negative_Correctly()
    {
        // Given
        var console = new TestConsole()
            .Colors(ColorSystem.Standard);

        // When
        var chart = new VerticalBarChart()
            .SetHeight(6)
            .SetData(new double[] { 4, 2, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 2, 3, 4, 5, 4, 3, 5, 8, 12 }.Select(v => v * -1));
        console.Write(chart);

        // Then
        await Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render.BothPositiveAndNegative")]
    public async Task Should_Render_BothPositiveAndNegative_Correctly()
    {
        // Given
        var console = new TestConsole()
            .Colors(ColorSystem.Standard);

        // When
        var chart = new VerticalBarChart()
            .SetHeight(10)
            .SetData(
                new double[] { 4, 2, 0, 1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2 }
                    .Select(v => v * -1)
                    .Concat(new double[] { 4, 2, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 2, 3, 4, 5, 4, 3, 5, 8, 12 }));
        console.Write(chart);

        // Then
        await Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render.Sin")]
    public async Task Should_Render_Sin_Correctly()
    {
        // Given
        var console = new TestConsole()
            .Colors(ColorSystem.Standard);

        // When
        var chart = new VerticalBarChart()
            .SetHeight(35)
            .SetData(
                Enumerable.Range(0, 250).Select(v => Math.Sin((v * Math.PI) / 20)));
        console.Write(chart);

        // Then
        await Verifier.Verify(console.Output);
    }
}
