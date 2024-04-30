using Spectre.Console;
using Spectre.Console.Cli;

namespace Help;

public sealed class DefaultCommand : Command
{
    private IAnsiConsole _console;

    public DefaultCommand(IAnsiConsole console)
    {
        _console = console;
    }

    public override int Execute(CommandContext context)
    {
        _console.WriteLine("Hello world");
        return 0;
    }
}