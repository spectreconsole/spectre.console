namespace Spectre.Console.Tests.Unit;

public sealed class TextPathTests
{
    [Fact]
    public void Should_Render_Full_Path_If_Possible()
    {
        // Given
        var console = new TestConsole().Width(40);

        // When
        console.Write(new TextPath("C:/Foo/Bar/Baz.txt"));

        // Then
        console.Output.ShouldBe("C:/Foo/Bar/Baz.txt");
    }

    [Fact]
    public void Should_Pop_Segments_From_Left()
    {
        // Given
        var console = new TestConsole().Width(17);

        // When
        console.Write(new TextPath("C:/My documents/Bar/Baz.txt"));

        // Then
        console.Output.ShouldBe("C:/…/Bar/Baz.txt");
    }

    [Theory]
    [InlineData(8, "1234567890", "…4567890")]
    [InlineData(9, "1234567890", "…34567890")]
    public void Should_Use_Last_Segments_If_Less_Than_Three(int width, string input, string expected)
    {
        // Given
        var console = new TestConsole().Width(width);

        // When
        console.Write(new TextPath(input));

        // Then
        console.Output.ShouldBe(expected);
    }
}
