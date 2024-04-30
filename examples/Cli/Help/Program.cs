using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

namespace Help;

public static class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp<DefaultCommand>();

        app.Configure(config =>
        {
            // Register the custom help provider
            config.SetHelpProvider(new CustomHelpProvider(config.Settings));

            // Render an unstyled help text for maximum accessibility
            config.Settings.HelpProviderStyles = null;
        });

        return app.Run(args);
    }
}
