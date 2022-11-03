namespace Spectre.Console.Cli.Tests.Unit.AsyncCommandTests;

// A dummy AsyncCommand<T> impl
public sealed class AsyncTestCommand : AsyncCommand<AsyncTestCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--foo")]
        public int Foo { get; set; }
    }

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        return Task.FromResult(0);
    }

    /// <summary>
    /// Async commands are tested like other commands.
    /// </summary>
    [Fact]
    public void Test_For_An_Async_Command()
    {
        // Given
        var tester = new CommandAppTester();
        tester.Configure(c =>
        {
            c.AddCommand<AsyncTestCommand>("the_command");
        });

        // When
        var result = tester.Run("--foo 32");

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<Settings>().And(settings =>
        {
            settings.Foo.ShouldBe(32);
        });
    }
}