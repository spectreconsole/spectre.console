namespace Spectre.Console.Tests.Unit;

public sealed partial class AnsiConsoleTests
{
    public sealed class Ansi
    {
        [Fact]
        public void Should_Write_Ansi_Codes_To_Console_If_Supported()
        {
            // Given
            var console = new TestConsole()
                .SupportsAnsi(true)
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.WriteAnsi("\e[101mHello\e[0m");

            // Then
            console.Output.NormalizeLineEndings()
                .ShouldBe("\e[101mHello\e[0m");
        }

        [Fact]
        public void Should_Not_Write_Ansi_Codes_To_Console_If_Not_Supported()
        {
            // Given
            var console = new TestConsole()
                .SupportsAnsi(false)
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.WriteAnsi("\e[101mHello\e[0m");

            // Then
            console.Output.NormalizeLineEndings()
                .ShouldBeEmpty();
        }

        [Fact]
        public void Should_Return_Ansi_For_Renderable()
        {
            // Given
            var console = new TestConsole().Colors(ColorSystem.TrueColor);
            var markup = new Console.Markup("[yellow]Hello [blue]World[/]![/]");

            // When
            var result = console.ToAnsi(markup);

            // Then
            result.ShouldBe("\e[38;5;11mHello \e[0m\e[38;5;12mWorld\e[0m\e[38;5;11m!\e[0m");
        }
    }
}