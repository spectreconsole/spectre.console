using Spectre.Console;
using Spectre.Console.Cli;

namespace Delegates;

public static partial class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.AddDelegate("foo", Foo)
                .WithDescription("Foos the bars");

            config.AddDelegate<BarSettings>("bar", Bar)
                .WithDescription("Bars the foos"); ;
        });

        return app.Run(args);
    }

    private static int Foo(CommandContext context)
    {
        AnsiConsole.WriteLine("Foo");
        return 0;
    }

    private static int Bar(CommandContext context, BarSettings settings)
    {
        for (var index = 0; index < settings.Count; index++)
        {
            AnsiConsole.WriteLine("Bar");
        }

        return 0;
    }
}
