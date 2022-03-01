using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Logging.Commands;

public class HelloCommand : Command<HelloCommand.Settings>
{
    private ILogger<HelloCommand> _logger;
    private IAnsiConsole _console;

    public HelloCommand(IAnsiConsole console, ILogger<HelloCommand> logger)
    {
        _console = console;
        _logger = logger;
        _logger.LogDebug("{0} initialized", nameof(HelloCommand));
    }

    public class Settings : LogCommandSettings
    {
        [CommandArgument(0, "[Name]")]
        public string Name { get; set; }
    }


    public override int Execute(CommandContext context, Settings settings)
    {
        _logger.LogInformation("Starting my command");
        AnsiConsole.MarkupLine($"Hello, [blue]{settings.Name}[/]");
        _logger.LogInformation("Completed my command");

        return 0;
    }
}
