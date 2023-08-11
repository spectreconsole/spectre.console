using Spectre.Console;
using Spectre.Console.Cli;

namespace Help;

public sealed class DefaultCommand : Command
{
    public DefaultCommand()
    {
    }

    public override int Execute(CommandContext context)
    {
        AnsiConsole.WriteLine("Hello world");
        return 0;
    }
}