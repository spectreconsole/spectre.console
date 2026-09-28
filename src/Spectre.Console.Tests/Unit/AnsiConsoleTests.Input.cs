using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spectre.Console.Tests.Unit;

public partial class AnsiConsoleTests
{
    public sealed class Input
    {
        [Fact]
        public void Should_Use_Settings_In_When_Specified()
        {
            // Given
            var input = new TestableInput();

            // When
            var console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Out = new AnsiConsoleOutput(new StringWriter()),
                In = input,
            });

            // Then
            console.Input.ShouldBeSameAs(input);
        }

        [Fact]
        public void Should_Fall_Back_To_Default_Input_When_Settings_In_Is_Null()
        {
            // Given, When
            var console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Out = new AnsiConsoleOutput(new StringWriter()),
            });

            // Then
            console.Input.ShouldNotBeSameAs(null);
            console.Input.ShouldNotBeOfType<TestableInput>();
        }

        private sealed class TestableInput : IAnsiConsoleInput
        {
            public bool IsKeyAvailable()
            {
                return true;
            }

            public ConsoleKeyInfo? ReadKey(bool intercept)
            {
                return null;
            }

            public Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken)
            {
                return Task.FromResult<ConsoleKeyInfo?>(null);
            }
        }
    }
}
