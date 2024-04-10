using System;
using System.ComponentModel;
using DemoAot.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DemoAot.Commands.Serve;

[Description("Launches a web server in the current working directory and serves all files in it.")]
public sealed class ServeCommand : Command<ServeCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("-p|--port <PORT>")]
        [DefaultValue(8080)]
        [Description("Port to use. Defaults to [grey]8080[/]. Use [grey]0[/] for a dynamic port.")]
        public int Port { get; set; }

        [CommandOption("-o|--open-browser [BROWSER]")]
        [Description("Open a web browser when the server starts. You can also specify which browser to use. If none is specified, the default one will be used.")]
        public FlagValue<string> OpenBrowser { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        if (settings.OpenBrowser.IsSet)
        {
            var browser = settings.OpenBrowser.Value;
            if (browser != null)
            {
                AnsiConsole.WriteLine($"Open in {browser}");
            }
            else
            {
                AnsiConsole.WriteLine($"Open in default browser.");
            }
        }

        SettingsDumper.Dump(settings);
        return 0;
    }
}
