using System.Threading;

namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Async
    {
        [Fact]
        public async Task Should_Execute_Command_Asynchronously()
        {
            // Given
            CancellationTokenSource cts = new();

            var app = new CommandAppTester();
            app.SetDefaultCommand<AsynchronousCommand>();
            app.Configure(config =>
            {
                config.PropagateExceptions();
            });

            // When
            var result = await app.RunAsync(cts.Token);

            // Then
            result.ExitCode.ShouldBe(0);
            result.Output.ShouldBe("Finished executing asynchronously");
        }

        [Fact]
        public async Task Should_Handle_Exception_Asynchronously()
        {
            // Given
            CancellationTokenSource cts = new();

            var app = new CommandAppTester();
            app.SetDefaultCommand<AsynchronousCommand>();

            // When
            cts.Cancel();
            var result = await app.RunAsync(cts.Token);

            // Then
            result.ExitCode.ShouldBe(-1);
        }

        [Fact]
        public async Task Should_Throw_Exception_Asynchronously()
        {
            // Given
            CancellationTokenSource cts = new();

            var app = new CommandAppTester();
            app.SetDefaultCommand<AsynchronousCommand>();
            app.Configure(config =>
            {
                config.PropagateExceptions();
            });

            // When
            cts.Cancel();
            var result = await Record.ExceptionAsync(async () =>
                    await app.RunAsync(cts.Token));

            // Then
            result.ShouldBeOfType<Exception>().And(ex =>
            {
                ex.Message.ShouldBe("Throwing exception asynchronously");
            });
        }
    }
}