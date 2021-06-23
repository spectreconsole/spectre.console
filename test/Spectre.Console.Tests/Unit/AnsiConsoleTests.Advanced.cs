using Shouldly;
using Spectre.Console.Advanced;
using Spectre.Console.Testing;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed partial class AnsiConsoleTests
    {
        public sealed class Advanced
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
                console.WriteAnsi("[101mHello[0m");

                // Then
                console.Output.NormalizeLineEndings()
                    .ShouldBe("[101mHello[0m");
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
                console.WriteAnsi("[101mHello[0m");

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
                result.ShouldBe("[38;5;11mHello [0m[38;5;12mWorld[0m[38;5;11m![0m");
            }
        }
    }
}
