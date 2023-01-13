namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command context.
/// </summary>
public sealed class CommandContext : IHelpProvider
{
    /// <summary>
    /// Gets the remaining arguments.
    /// </summary>
    /// <value>
    /// The remaining arguments.
    /// </value>
    public IRemainingArguments Remaining { get; }

    /// <summary>
    /// Gets the name of the command.
    /// </summary>
    /// <value>
    /// The name of the command.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Gets the data that was passed to the command during registration (if any).
    /// </summary>
    /// <value>
    /// The command data.
    /// </value>
    public object? Data { get; }

    /// <inheritdoc/>
    public IEnumerable<IRenderable> Help { get; }

    /// <summary>
    /// Gets the application version.
    /// </summary>
    public string? ApplicationVersion { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandContext"/> class.
    /// </summary>
    /// <param name="remaining">The remaining arguments.</param>
    /// <param name="name">The command name.</param>
    /// <param name="data">The command data.</param>
    public CommandContext(IRemainingArguments remaining, string name, object? data)
    {
        Remaining = remaining ?? throw new System.ArgumentNullException(nameof(remaining));
        Name = name ?? throw new System.ArgumentNullException(nameof(name));
        Data = data;
        Help = Array.Empty<IRenderable>();
        ApplicationVersion = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandContext"/> class.
    /// </summary>
    /// <param name="remaining">The remaining arguments.</param>
    /// <param name="name">The command name.</param>
    /// <param name="data">The command data.</param>
    /// <param name="help">The help text for the command.</param>
    /// <param name="applicationVersion">The application version.</param>
    public CommandContext(IRemainingArguments remaining, string name, object? data, IEnumerable<IRenderable> help, string applicationVersion)
    {
        Remaining = remaining ?? throw new System.ArgumentNullException(nameof(remaining));
        Name = name ?? throw new System.ArgumentNullException(nameof(name));
        Data = data;
        Help = help;
        ApplicationVersion = applicationVersion;
    }
}