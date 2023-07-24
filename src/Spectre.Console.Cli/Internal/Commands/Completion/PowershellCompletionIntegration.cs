namespace Spectre.Console.Cli.Internal.Commands.Completion;

internal class PowershellCompletionIntegrationSettings : CommandSettings
{
    [CommandOption("--noInstall")]
    public bool NoInstall { get; set; }
}

internal class PowershellCompletionIntegration : Command<PowershellCompletionIntegrationSettings>
{
    public PowershellCompletionIntegration()
    {
    }

    public override int Execute(
        CommandContext context,
        PowershellCompletionIntegrationSettings settings)
    {
        var startArgs = GetSelfStartCommandFromCommandLineArgs();

        var startCommand = string.IsNullOrEmpty(startArgs.Runtime)
            ? "& \"" + startArgs.Command + "\""
            : "& \"" + startArgs.Runtime + "\" \"" + startArgs.Command + "\"";

        var replacements = new Dictionary<string, string>
        {
            ["[RUNCOMMAND]"] = startCommand,
            ["[APPNAME]"] = startArgs.CommandName,
            ["[APPNAME_LowerCase]"] = startArgs.CommandName.ToLowerInvariant(),
        };

        var sb = new StringBuilder();
        sb.AppendLine(GetResource("PowershellIntegration_Completion_and_alias", replacements));
        if (!settings.NoInstall)
        {
            sb.AppendLine(GetResource("PowershellIntegration_Install", replacements));
        }

        // Using Console.WriteLine instead of _writer.WriteLine because
        // Spectre console inserts line breaks which breaks the script
        System.Console.WriteLine(sb);

        return 0;
    }

    private static string GetResource(string resourceName, Dictionary<string, string> replacements)
    {
        var result = Properties.Resources.ResourceManager.GetString(resourceName) ?? throw new InvalidOperationException($"Could not find resource '{resourceName}'.");
        foreach (var replacement in replacements)
        {
            result = result.Replace(replacement.Key, replacement.Value);
        }

        return result;
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