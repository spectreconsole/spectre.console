namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Async
    {
        [Fact]
        public async Task Should_Execute_Command_Asynchronously()
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
        public async Task Should_Handle_Exception_Asynchronously()
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
        public async Task Should_Throw_Exception_Asynchronously()
        {
            // Given
            var app = new CommandAppTester();
            app.SetDefaultCommand<AsynchronousCommand>();
            app.Configure(config =>
            {
                config.PropagateExceptions();
            });

            // When
            var exception = await Record.ExceptionAsync(async () =>
                    await app.RunAsync(new[]
                        {
                        "--ThrowException",
                        "true",
                        }));

            // Then
            exception.ShouldBeOfType<Exception>().And(ex =>
            {
                ex.Message.ShouldBe("Throwing exception asynchronously");
            });
        }

        [Fact]
        public async Task Should_Throw_OperationCanceledException_When_Propagated_And_Cancelled()
        {
            // Given
            var app = new CommandAppTester();
            app.SetDefaultCommand<AsynchronousCommand>();
            app.Configure(config =>
            {
                config.PropagateExceptions();
            });

            // When
            var exception = await Record.ExceptionAsync(async () =>
                await app.RunAsync(cancellationToken: new CancellationToken(canceled: true)));

            // Then
            exception.ShouldNotBeNull();
            exception.ShouldBeAssignableTo<OperationCanceledException>();
        }

        [Fact]
        public async Task Should_Return_Default_Exit_Code_When_Cancelled()
        {
            // Given
            var app = new CommandAppTester();
            app.SetDefaultCommand<AsynchronousCommand>();

            // When
            var result = await app.RunAsync(cancellationToken: new CancellationToken(canceled: true));

            // Then
            result.ExitCode.ShouldBe(130);
            result.Output.ShouldBeEmpty();
        }

        [Fact]
        public async Task Should_Return_Custom_Exit_Code_When_Cancelled()
        {
            // Given
            var app = new CommandAppTester();
            app.SetDefaultCommand<AsynchronousCommand>();
            app.Configure(config =>
            {
                config.CancellationExitCode(123);
            });

            // When
            var result = await app.RunAsync(cancellationToken: new CancellationToken(canceled: true));

            // Then
            result.ExitCode.ShouldBe(123);
            result.Output.ShouldBeEmpty();
        }
    }
}