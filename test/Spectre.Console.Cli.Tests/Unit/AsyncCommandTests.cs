namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class AsyncCommandTests
{
    [Fact]
    public void Single_Command_Case()
    {
        var app = new CommandAppTester();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddBranch<CommandContents>("", nothing =>
            {
                nothing.AddCommand<NullCommand>("");
            });
        });

        // When
        var result = app.RunAsync(new[]
        {
            "animal", "dog", "12", "--good-boy",
            "--name", "Rufus",
        });
    }
}