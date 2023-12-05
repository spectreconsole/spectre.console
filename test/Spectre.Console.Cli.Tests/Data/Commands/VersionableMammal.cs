using System;

namespace Spectre.Console.Tests.Data;

[Description("The versionable mammal")]
public class VersionableMammalCommand : AnimalCommand<VersionableMammalSettings>
{
    private readonly IAnsiConsole _console;

    public VersionableMammalCommand(IAnsiConsole console)
    {
        _console = console;
    }

    public override int Execute(CommandContext context, VersionableMammalSettings settings)
    {
        _console.WriteLine("Versionable ran!");
        return 0;
    }
}