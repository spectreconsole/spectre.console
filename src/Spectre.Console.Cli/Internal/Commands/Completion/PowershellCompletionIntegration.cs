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
        StringBuilder outputBuilder = new();
        // if (settings.AddToPath)
        // {
        //     outputBuilder.AppendLine(
        //         "$env:path = $env:path -replace \"" + Environment.CurrentDirectory + "\", \"\""
        //     );
        //     outputBuilder.AppendLine("$env:path = $env:path -replace \";;\" , \";\"");
        //     outputBuilder.AppendLine(
        //         "$env:path = $env:path + \";" + Environment.CurrentDirectory + "\""
        //     );
        // }

        var startArgs = GetSelfStartCommandFromCommandLineArgs();

        // if (!string.IsNullOrEmpty(startArgs.Runtime))
        // {
        //     var startCommand = "& \"" + startArgs.Runtime + "\" \"" + startArgs.Command + "\"";
        // }
        // else {
        //     var startCommand = "& \"" + startArgs.Command + "\"";
        //  }

        var startCommand = string.IsNullOrEmpty(startArgs.Runtime)
            ? "& \"" + startArgs.Command + "\""
            : "& \"" + startArgs.Runtime + "\" \"" + startArgs.Command + "\"";

        var completionCommand = startCommand + " cli complete --position $cursorPosition \"$wordToComplete\"";

        outputBuilder
            .Append("Register-ArgumentCompleter -Native -CommandName ")
            .Append(startArgs.CommandName)
            .AppendLine(" -ScriptBlock {");

        outputBuilder.AppendLine("    param($commandName, $wordToComplete, $cursorPosition)");
        outputBuilder
        .Append("    $completions = ")
        .AppendLine(completionCommand);
        outputBuilder.AppendLine("    if ($completions) {");
        outputBuilder.AppendLine("        foreach ($completion in $completions) {");
        outputBuilder.AppendLine(
            "            [System.Management.Automation.CompletionResult]::new($completion, $completion, 'ParameterValue', $completion)"
        );
        outputBuilder.AppendLine("        }");
        outputBuilder.AppendLine("    }");
        outputBuilder.AppendLine("    else {");
        outputBuilder.AppendLine("        $null");
        outputBuilder.AppendLine("    }");
        outputBuilder.AppendLine("}");

        _writer.WriteLine(outputBuilder.ToString());

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
