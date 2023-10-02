namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("Widgets/Rows")]
public sealed class RowsTests
{
    [Fact]
    [Expectation("GH-1188-Rows")]
    [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/1188")]
    public Task Should_Render_Rows_In_Panel_Without_Breaking_Lines()
    {
        // Given
        var console = new TestConsole().Width(60);
        var rows = new Rows(
            new IRenderable[]
            {
                new Text("1"),
                new Text("22"),
                new Text("333"),
            });
        var panel = new Panel(rows);

        // When
        console.Write(panel);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render")]
    public Task Should_Render_Rows()
    {
        // Given
        var console = new TestConsole().Width(60);
        var rows = new Rows(
            new IRenderable[]
            {
                    new Markup("Hello"),
                    new Table()
                        .AddColumns("Foo", "Bar")
                        .AddRow("Baz", "Qux"),
                    new Markup("World"),
            });

        // When
        console.Write(rows);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Nested")]
    public Task Should_Render_Rows_Correctly_Inside_Other_Widget()
    {
        // Given
        var console = new TestConsole().Width(60);
        var table = new Table()
            .AddColumns("Foo", "Bar")
            .AddRow("HELLO WORLD")
            .AddRow(
            new Rows(new IRenderable[]
            {
                    new Markup("Hello"),
                    new Markup("World"),
            }), new Text("Qux"));

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Empty")]
    public Task Should_Not_Throw_Exception_On_Empty_Rows()
    {
        // Given
        var console = new TestConsole().Width(60);
        var table = new Table()
            .AddColumns("Foo", "Bar")
            .AddRow("HELLO WORLD")
            .AddRow(
                new Rows(), new Text("Qux"));

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Expanded_And_Nested")]
    public Task Should_Render_Rows_Correctly_Inside_Other_Widget_When_Expanded()
    {
        // Given
        var console = new TestConsole().Width(60);
        var table = new Table()
            .AddColumns("Foo", "Bar")
            .AddRow("HELLO WORLD")
            .AddRow(
            new Rows(new IRenderable[]
            {
                    new Markup("Hello"),
                    new Markup("World"),
            }).Expand(), new Text("Qux"));

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }
}
