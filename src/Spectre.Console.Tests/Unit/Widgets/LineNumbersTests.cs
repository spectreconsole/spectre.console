namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Widgets/LineNumbers")]
public sealed class LineNumbersTests
{
    [Fact]
    [Expectation("Render")]
    public Task Should_Render_LineNumbers()
    {
        // Given
        var console = new TestConsole().Width(60);
        var rows = new LineNumbers(new Markup("Hello\n"), new Table()
                .AddColumns("Foo", "Bar")
                .AddRow("Baz", "Qux"), new Markup("World"));

        // When
        console.Write(rows);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Nested")]
    public Task Should_Render_LineNumbers_Correctly_Inside_Other_Widget()
    {
        // Given
        var console = new TestConsole().Width(60);
        var table = new Table()
            .AddColumns("Foo", "Bar")
            .AddRow("HELLO WORLD")
            .AddRow(
                new LineNumbers(new Markup("HelloHelloHelloHelloHello\n"), new Markup("World")), new Text("Qux"));

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Empty")]
    public Task Should_Not_Throw_Exception_On_Empty_LineNumbers()
    {
        // Given
        var console = new TestConsole().Width(60);
        var table = new Table()
            .AddColumns("Foo", "Bar")
            .AddRow("HELLO WORLD")
            .AddRow(
                new LineNumbers(), new Text("Qux"));

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }
}