namespace Spectre.Console.Cli.Internal.Commands.Completion;

internal class PowershellCompletionIntegrationSettings : CommandSettings
{
    // [CommandOption("--addToPath")]
    // public bool AddToPath { get; set; }

    [CommandOption("--persist")]
    public bool Persist { get; set; }
}

internal class PowershellCompletionIntegration : Command<PowershellCompletionIntegrationSettings>
{
    private IAnsiConsole _writer;

    public PowershellCompletionIntegration(IConfiguration configuration)
    {
        _writer = configuration.Settings.Console.GetConsole();
    }

    public override int Execute(
        CommandContext context,
        PowershellCompletionIntegrationSettings settings
    )
    {
        var resources = Properties.Resources.ResourceManager.GetString("PowershellIntegration_Fully_Integrated");

        StringBuilder sb = new();
        var startArgs = GetSelfStartCommandFromCommandLineArgs();

        var startCommand = string.IsNullOrEmpty(startArgs.Runtime)
            ? "& \"" + startArgs.Command + "\""
            : "& \"" + startArgs.Runtime + "\" \"" + startArgs.Command + "\"";

        var result = resources
            .Replace("[RUNCOMMAND]", startCommand)
            .Replace("[APPNAME]", startArgs.CommandName)
            .Replace("[APPNAME_LowerCase]", startArgs.CommandName.ToLowerInvariant());

        // Using Console.WriteLine instead of _writer.WriteLine because
        // Spectre console inserts line breaks which breaks the script
        System.Console.WriteLine(result);

        return 0;
    }

    private static StartArgs GetSelfStartCommandFromCommandLineArgs()
    {
        var args = Environment.GetCommandLineArgs();
        var command = args[0];
        if (command.EndsWith(".dll"))
        {
            return new StartArgs("dotnet", command);
        }

        var commandIsDotnet = Path.GetFileNameWithoutExtension(command)
            .Equals("dotnet", StringComparison.OrdinalIgnoreCase);

        if (commandIsDotnet)
        {
            return new StartArgs(command, args[1]);
        }

        return new StartArgs(string.Empty, args[1]);
    }

    // private record StartArgs(string Runtime, string Command)
    private class StartArgs
    {
        public string Runtime { get; }
        public string Command { get; }
        public string CommandName => Path.GetFileNameWithoutExtension(Command);

        public StartArgs(string runtime, string command)
        {
            Runtime = runtime;
            Command = command;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Runtime))
            {
                return Command;
            }

            return string.Join(" ", Runtime, Command);
        }
    }
}