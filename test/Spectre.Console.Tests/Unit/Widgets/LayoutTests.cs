namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("Widgets/Layout")]
public sealed class LayoutTests
{
    [Fact]
    [Expectation("Render_Empty_Layout")]
    public Task Should_Render_Empty_Layout()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout();

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Layout")]
    public Task Should_Render_Empty_Layout_With_Renderable()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout().Update(new Panel("Hello").DoubleBorder().Expand());

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Layout_With_Columns")]
    public Task Should_Render_Layout_With_Columns()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout()
            .SplitColumns(
                new Layout("Left"),
                new Layout("Right"));

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Layout_With_Rows")]
    public Task Should_Render_Layout_With_Rows()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout()
            .SplitRows(
                new Layout("Top"),
                new Layout("Bottom"));

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Layout_With_Nested_Columns")]
    public Task Should_Render_Layout_With_Nested_Columns()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout()
            .SplitColumns(
                new Layout("Left")
                    .SplitColumns(
                        new Layout("L1"),
                        new Layout("L2")),
                new Layout("Right")
                    .SplitColumns(
                        new Layout("R1"),
                        new Layout("R2")));

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Layout_With_Nested_Rows")]
    public Task Should_Render_Layout_With_Nested_Rows()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout()
            .SplitRows(
                new Layout("Top")
                    .SplitRows(
                        new Layout("T1"),
                        new Layout("T2")),
                new Layout("Bottom")
                    .SplitRows(
                        new Layout("B1"),
                        new Layout("B2")));

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Layout_With_Nested_Rows_And_Columns")]
    public Task Should_Render_Layout_With_Nested_Rows_And_Columns()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout()
            .SplitRows(
                new Layout("Top")
                    .SplitRows(
                        new Layout("T1")
                            .SplitColumns(
                                new Layout("A"),
                                new Layout("B")),
                        new Layout("T2")
                            .SplitColumns(
                                new Layout("C"),
                                new Layout("D"))),
                new Layout("Bottom")
                    .SplitRows(
                        new Layout("B1")
                            .SplitColumns(
                                new Layout("E"),
                                new Layout("F")),
                        new Layout("B2")
                            .SplitColumns(
                                    new Layout("G"),
                                    new Layout("H"))));

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Layout_Without_Invisible_Children")]
    public Task Should_Render_Layout_Without_Invisible_Children()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout()
            .SplitRows(
                new Layout("Top")
                    .SplitRows(
                        new Layout("T1")
                            .SplitColumns(
                                new Layout("A").Invisible(),
                                new Layout("B")),
                        new Layout("T2")
                            .SplitColumns(
                                new Layout("C"),
                                new Layout("D"))),
                new Layout("Bottom")
                    .SplitRows(
                        new Layout("B1")
                            .SplitColumns(
                                new Layout("E"),
                                new Layout("F")),
                        new Layout("B2")
                            .SplitColumns(
                                    new Layout("G"),
                                    new Layout("H").Invisible())));

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Layout_With_Respect_To_Ratio")]
    public Task Should_Render_Layout_With_Respect_To_Ratio()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout()
            .SplitColumns(
                new Layout("Left").Ratio(3),
                new Layout("Right"));

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Layout_With_Respect_To_Size")]
    public Task Should_Render_Layout_With_Respect_To_Size()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout()
            .SplitColumns(
                new Layout("Left").Size(28),
                new Layout("Right"));

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Layout_With_Respect_To_Minimum_Size")]
    public Task Should_Render_Layout_With_Respect_To_Minimum_Size()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout()
            .SplitColumns(
                new Layout("Left").Size(28).MinimumSize(30),
                new Layout("Right"));

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Fallback_Layout")]
    public Task Should_Fall_Back_To_Parent_Layout_If_All_Children_Are_Invisible()
    {
        // Given
        var console = new TestConsole().Size(new Size(40, 15));
        var layout = new Layout()
            .SplitRows(
                new Layout("T1").SplitColumns(
                    new Layout("A").Invisible(),
                    new Layout("B").Invisible()),
                new Layout("T2").SplitColumns(
                    new Layout("C").Invisible(),
                    new Layout("D").Invisible()));

        // When
        console.Write(layout);

        // Then
        return Verifier.Verify(console.Output);
    }
}
