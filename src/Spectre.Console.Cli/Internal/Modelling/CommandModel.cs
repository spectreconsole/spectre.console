namespace Spectre.Console.Cli;

internal sealed class CommandModel : ICommandContainer, ICommandModel
{
    public string? ApplicationName { get; }
    public ParsingMode ParsingMode { get; }
    public IList<CommandInfo> Commands { get; }
    public IList<string[]> Examples { get; }

    public CommandInfo? DefaultCommand => Commands.FirstOrDefault(c => c.IsDefaultCommand);

    string ICommandModel.ApplicationName => GetApplicationName();
    IList<ICommandInfo> Help.ICommandContainer.Commands => Commands.Cast<ICommandInfo>().ToList();
    ICommandInfo? Help.ICommandContainer.DefaultCommand => DefaultCommand;

    public CommandModel(
        CommandAppSettings settings,
        IEnumerable<CommandInfo> commands,
        IEnumerable<string[]> examples)
    {
        ApplicationName = settings.ApplicationName;
        ParsingMode = settings.ParsingMode;
        Commands = new List<CommandInfo>(commands ?? Array.Empty<CommandInfo>());
        Examples = new List<string[]>(examples ?? Array.Empty<string[]>());
    }

    public string GetApplicationName()
    {
        return
            ApplicationName ??
            Path.GetFileName(GetApplicationFile()) ?? // null is propagated by GetFileName
            "?";
    }

    private static string? GetApplicationFile()
    {
        var location = Assembly.GetEntryAssembly()?.Location;
        if (string.IsNullOrWhiteSpace(location))
        {
            // this is special case for single file executable
            // (Assembly.Location returns empty string)
            return Process.GetCurrentProcess().MainModule?.FileName;
        }

        return location;
    }
}