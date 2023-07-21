using System.Resources;

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

        //_writer.WriteLine(resources);
        //return 1;

        StringBuilder sb = new();
        var startArgs = GetSelfStartCommandFromCommandLineArgs();

        var startCommand = string.IsNullOrEmpty(startArgs.Runtime)
            ? "& \"" + startArgs.Command + "\""
            : "& \"" + startArgs.Runtime + "\" \"" + startArgs.Command + "\"";

        _writer.WriteLine(resources.Replace("[RUNCOMMAND]", startCommand));

        //var completionCommand = startCommand + " cli complete --position $cursorPosition \"$wordToComplete\"";

        //sb
        //    .Append("Register-ArgumentCompleter -Native -CommandName ")
        //    .Append(startArgs.CommandName)
        //    .AppendLine(" -ScriptBlock {");

        //sb.AppendLine("    param($commandName, $wordToComplete, $cursorPosition)");
        //sb.Append("    $completions = ").AppendLine(completionCommand);
        //sb.AppendLine("    if ($completions) {");
        //sb.AppendLine("        foreach ($completion in $completions) {");
        //sb.AppendLine(
        //    "            [System.Management.Automation.CompletionResult]::new($completion, $completion, 'ParameterValue', $completion)"
        //);
        //sb.AppendLine("        }");
        //sb.AppendLine("    }");
        //sb.AppendLine("    else {");
        //sb.AppendLine("        $null");
        //sb.AppendLine("    }");
        //sb.AppendLine("}");

        //_writer.WriteLine(sb.ToString());

        // if (settings.Persist)
        // {
        //     // Add to profile
        //     string profilePath = Path.Combine(
        //         Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        //         "WindowsPowerShell",
        //         "profile.ps1"
        //     );
        // }

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
