namespace Spectre.Console.Tests.Unit;

public sealed class TextPathTests
{
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
        console.Output.TrimEnd()
            .ShouldBe(expected);
    }

    [Theory]
    [InlineData("C:/Foo/Bar/Baz.txt", "C:/Foo/Bar/Baz.txt")]
    [InlineData("/Foo/Bar/Baz.txt", "/Foo/Bar/Baz.txt")]
    [InlineData("Foo/Bar/Baz.txt", "Foo/Bar/Baz.txt")]
    public void Should_Render_Full_Path_If_Possible(string input, string expected)
    {
        // Given
        var console = new TestConsole().Width(40);

        // When
        console.Write(new TextPath(input));

        // Then
        console.Output.TrimEnd()
            .ShouldBe(expected);
    }

    [Theory]
    [InlineData(17, "C:/My documents/Bar/Baz.txt", "C:/…/Bar/Baz.txt")]
    [InlineData(15, "/My documents/Bar/Baz.txt", "/…/Bar/Baz.txt")]
    [InlineData(14, "My documents/Bar/Baz.txt", "…/Bar/Baz.txt")]
    public void Should_Pop_Segments_From_Left(int width, string input, string expected)
    {
        // Given
        var console = new TestConsole().Width(width);

        // When
        console.Write(new TextPath(input));

        // Then
        console.Output.TrimEnd()
            .ShouldBe(expected);
    }

    [Theory]
    [InlineData("C:/My documents/Bar/Baz.txt")]
    [InlineData("/My documents/Bar/Baz.txt")]
    [InlineData("My documents/Bar/Baz.txt")]
    [InlineData("Bar/Baz.txt")]
    [InlineData("Baz.txt")]
    public void Should_Insert_Line_Break_At_End_Of_Path(string input)
    {
        // Given
        var console = new TestConsole().Width(80);

        // When
        console.Write(new TextPath(input));

        // Then
        console.Output.ShouldEndWith("\n");
    }

    [Fact]
    public void Should_Right_Align_Correctly()
    {
        // Given
        var console = new TestConsole().Width(40);

        // When
        console.Write(new TextPath("C:/My documents/Bar/Baz.txt").RightJustified());

        // Then
        console.Output.TrimEnd('\n')
            .ShouldBe("             C:/My documents/Bar/Baz.txt");
    }

    [Fact]
    public void Should_Center_Align_Correctly()
    {
        // Given
        var console = new TestConsole().Width(40);

        // When
        console.Write(new TextPath("C:/My documents/Bar/Baz.txt").Centered());

        // Then
        console.Output.TrimEnd('\n')
            .ShouldBe("      C:/My documents/Bar/Baz.txt       ");
    }
}
