namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Async
    {
        [Fact]
        public async void Should_Execute_Command_Asynchronously()
        {
            // Given
            var app = new CommandAppTester();
            app.SetDefaultCommand<AsynchronousCommand>();
            app.Configure(config =>
            {
                config.PropagateExceptions();
            });

            // When
            var result = await app.RunAsync();

            // Then
            result.ExitCode.ShouldBe(0);
            result.Output.ShouldBe("Finished executing asynchronously");
        }

        [Fact]
        public async void Should_Handle_Exception_Asynchronously()
        {
            // Given
            var app = new CommandAppTester();
            app.SetDefaultCommand<AsynchronousCommand>();

            // When
            var result = await app.RunAsync(new[]
                        {
                        "--ThrowException",
                        "true",
                        });

            // Then
            result.ExitCode.ShouldBe(-1);
        }

        [Fact]
        public async void Should_Throw_Exception_Asynchronously()
        {
            // Given
            var app = new CommandAppTester();
            app.SetDefaultCommand<AsynchronousCommand>();
            app.Configure(config =>
            {
                config.PropagateExceptions();
            });

            // When
            var result = await Record.ExceptionAsync(async () =>
                    await app.RunAsync(new[]
                        {
                        "--ThrowException",
                        "true",
                        }));

            // Then
            result.ShouldBeOfType<Exception>().And(ex =>
            {
                ex.Message.ShouldBe("Throwing exception asynchronously");
            });
        }
    }
}