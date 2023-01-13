namespace Spectre.Console.Tests.Data;

/// <summary>
/// Represents a command that renders a custom bannder, and then help text and the application version.
/// </summary>
/// <remarks>
/// A good example of the behaviour that some spectre.console users would like from their application default command.
/// </remarks>
public sealed class DefaultCommand : Command<EmptyCommandSettings>
{
    private readonly IAnsiConsole _console;

    public DefaultCommand(IAnsiConsole console)
    {
        _console = console;
    }

    public override int Execute(CommandContext context, EmptyCommandSettings settings)
    {
        // Fancy application banner.
        _console.WriteLine("----------------------------------");
        _console.WriteLine("---      DEFAULT COMMAND       ---");
        _console.WriteLine("----------------------------------");
        _console.WriteLine();

        // Command help.
        _console.SafeRender(context.Help);
        _console.WriteLine();

        // Application version.
        _console.MarkupLine("Version {0}", context.ApplicationVersion);

        return 0;
    }
}