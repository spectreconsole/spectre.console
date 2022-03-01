using System.Linq;
using Spectre.Console.Cli;

namespace Dynamic;

public static class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            foreach (var index in Enumerable.Range(1, 10))
            {
                config.AddCommand<MyCommand>($"c{index}")
                    .WithDescription($"Prints the number {index}")
                    .WithData(index);
            }
        });

        return app.Run(args);
    }
}
