namespace Spectre.Console.Tests.Data;

public sealed class AsynchronousCommand : AsyncCommand<AsynchronousCommandSettings>
{
    private readonly IAnsiConsole _console;

    public AsynchronousCommand(IAnsiConsole console)
    {
        _console = console;
    }

    public async override Task<int> ExecuteAsync(CommandContext context, AsynchronousCommandSettings settings)
    {
        // Simulate a long running asynchronous task
        await Task.Delay(200);

        if (settings.ThrowException)
        {
            throw new Exception($"Throwing exception asynchronously");
        }
        else
        {
            _console.WriteLine($"Finished executing asynchronously");
        }

        return 0;
    }
}
