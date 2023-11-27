namespace Spectre.Console.Cli;

internal sealed class CommandModel : ICommandContainer, ICommandModel
{
    public string? ApplicationName { get; }
    public ParsingMode ParsingMode { get; }
    public IList<CommandInfo> Commands { get; }
    public IList<string[]> Examples { get; }

    public CommandInfo? DefaultCommand => Commands.FirstOrDefault(c => c.IsDefaultCommand);

    string ICommandModel.ApplicationName => GetApplicationName(ApplicationName);
    IReadOnlyList<ICommandInfo> Help.ICommandContainer.Commands => Commands.Cast<ICommandInfo>().ToList();
    ICommandInfo? Help.ICommandContainer.DefaultCommand => DefaultCommand;
    IReadOnlyList<string[]> Help.ICommandContainer.Examples => (IReadOnlyList<string[]>)Examples;

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

    /// <summary>
    /// Gets the name of the application.
    /// If the provided <paramref name="applicationName"/> is not null or empty,
    /// it is returned. Otherwise the name of the current application
    /// is determined based on the executable file's name.
    /// </summary>
    /// <param name="applicationName">The optional name of the application.</param>
    /// <returns>
    /// The name of the application, or a default value of "?" if no valid application name can be determined.
    /// </returns>
    private static string GetApplicationName(string? applicationName)
    {
        return
            applicationName ??
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