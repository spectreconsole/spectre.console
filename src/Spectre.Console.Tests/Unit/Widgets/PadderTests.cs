namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Widgets/Padder")]
public sealed class PadderTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void Should_Not_Throw_When_Panel_Gives_Less_Width_Than_The_Padding(int width)
    {
        // Given
        var console = new TestConsole().Width(width);
        var padder = new Padder(new Markup("Hello"), new Padding(2));

        // When
        var result = Record.Exception(() => console.Write(new Panel(padder)));

        // Then
        result.ShouldBeNull();
    }

    [Fact]
    [Expectation("Render")]
    public Task Should_Render_Padded_Object_Correctly()
    {
        // Given
        var console = new TestConsole().Width(60);
        var table = new Table();
        table.AddColumn("Foo");
        table.AddColumn("Bar");
        table.AddRow("Baz", "Qux");
        table.AddRow("Corgi", "Waldo");

        // When
        console.Write(new Padder(table).Padding(1, 2, 3, 4));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Expanded")]
    public Task Should_Render_Expanded_Padded_Object_Correctly()
    {
        // Given
        var console = new TestConsole().Width(60);
        var table = new Table();
        table.AddColumn("Foo");
        table.AddColumn("Bar");
        table.AddRow("Baz", "Qux");
        table.AddRow("Corgi", "Waldo");

        // When
        console.Write(new Padder(table)
            .Padding(1, 2, 3, 4)
            .Expand());

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Nested")]
    public Task Should_Render_Padded_Object_Correctly_When_Nested_Within_Other_Object()
    {
        // Given
        var console = new TestConsole().Width(60);
        var table = new Table();
        table.AddColumn("Foo");
        table.AddColumn("Bar", c => c.PadLeft(0).PadRight(0));
        table.AddRow("Baz", "Qux");
        table.AddRow(new Text("Corgi"), new Padder(new Panel("Waldo"))
            .Padding(2, 1));

        // When
        console.Write(new Padder(table)
            .Padding(1, 2, 3, 4)
            .Expand());

        // Then
        return Verifier.Verify(console.Output);
    }
}