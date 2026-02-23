namespace Spectre.Console.Ansi.Tests;

public sealed class AnsiWriterTests
{
    [Fact]
    public void Should_Write_Expected_Ansi()
    {
        // Given
        var fixture = new AnsiFixture();
        fixture.Capabilities.Ansi = true;
        fixture.Capabilities.Links = true;

        // When
        fixture.Writer
            .BeginLink("https://spectreconsole.net", linkId: 123)
            .Decoration(Decoration.Bold | Decoration.Italic)
            .Foreground(Color.Yellow)
            .Write("Spectre Console")
            .ResetStyle()
            .EndLink();

        // Then
        fixture.Output.ShouldBe("\e]8;id=123;https://spectreconsole.net\e\\\e[1;3m\e[38;5;11mSpectre Console\e[0m\e]8;;\e\\");
    }

    [Fact]
    public void Should_Not_Write_Link_If_Not_Supported()
    {
        // Given
        var fixture = new AnsiFixture();
        fixture.Capabilities.Ansi = true;
        fixture.Capabilities.Links = false;

        // When
        fixture.Writer
            .BeginLink("https://spectreconsole.net", linkId: 123)
            .Decoration(Decoration.Bold | Decoration.Italic)
            .Foreground(Color.Yellow)
            .Write("Spectre Console")
            .ResetStyle()
            .EndLink();

        // Then
        fixture.Output.ShouldBe("\e[1;3m\e[38;5;11mSpectre Console\e[0m");
    }

    [Fact]
    public void Should_Not_Write_Ansi_If_Not_Supported()
    {
        // Given
        var fixture = new AnsiFixture();
        fixture.Capabilities.Ansi = false;

        // When
        fixture.Writer
            .BeginLink("https://spectreconsole.net", linkId: 123)
            .Decoration(Decoration.Bold | Decoration.Italic)
            .Foreground(Color.Yellow)
            .Write("Spectre Console")
            .ResetStyle()
            .EndLink();

        // Then
        fixture.Output.ShouldBe("Spectre Console");
    }
}
