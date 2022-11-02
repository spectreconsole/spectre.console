namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class AsyncCommandNullTests
{
    [Fact]
    public void No_Commands_Case()
    {
        var app = new CommandAppTester();

        app.Configure(config => {
            config.EmptyCommand();
        });

        // When
        var result = app.RunAsync(new[]
        {
            "0"
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<EmptySettings>();
    }
}
