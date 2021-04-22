using Shouldly;
using Spectre.Console.Advanced;
using Spectre.Console.Testing;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public partial class AnsiConsoleTests
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
        }
    }
}
