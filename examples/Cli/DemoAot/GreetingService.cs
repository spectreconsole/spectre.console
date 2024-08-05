using Spectre.Console;
using Spectre.Console.Cli;

namespace DemoAot;

public class InfoCommand(GreetingService greetingService) : Command<InfoCommand.Settings>
{
    public override int Execute(CommandContext context, Settings settings)
    {
        greetingService.Greet("World");
        return 0;
    }

    public class Settings : CommandSettings
    {
        [CommandOption("-v")]
        public bool Verbose {get;set;}
    }
}

public class GreetingService(IAnsiConsole ansiConsole)
{
    public void Greet(string name)
    {
        ansiConsole.WriteLine($"Hello {name}!");
    }
}