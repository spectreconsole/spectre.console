using System.Threading.Tasks;
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
                .WithDescription("Bars the foos");

            config.AddAsyncDelegate("fooAsync", FooAsync)
                .WithDescription("Foos the bars asynchronously");

            config.AddAsyncDelegate<BarSettings>("barAsync", BarAsync)
                .WithDescription("Bars the foos asynchronously");
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

    private static Task<int> FooAsync(CommandContext context)
    {
        AnsiConsole.WriteLine("Foo");
        return Task.FromResult(0);
    }

    private static Task<int> BarAsync(CommandContext context, BarSettings settings)
    {
        for (var index = 0; index < settings.Count; index++)
        {
            AnsiConsole.WriteLine("Bar");
        }

        return Task.FromResult(0);
    }
}
