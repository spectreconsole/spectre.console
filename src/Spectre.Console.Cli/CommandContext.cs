namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command context.
/// </summary>
public sealed class CommandContext
{
    /// <summary>
    /// Gets the remaining arguments.
    /// </summary>
    /// <value>
    /// The remaining arguments.
    /// </value>
    public IRemainingArguments Remaining { get; }

    /// <summary>
    /// Gets all the arguments that were passed to the application.
    /// </summary>
    public IReadOnlyList<string> Arguments { get; }

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

    /// <summary>
    /// Gets the type resolver of the current command app.
    /// </summary>
    /// <value>
    /// The type resolver.
    /// </value>
    public ITypeResolver TypeResolver { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandContext"/> class.
    /// </summary>
    /// <param name="arguments">All arguments that were passed to the application.</param>
    /// <param name="remaining">The remaining arguments.</param>
    /// <param name="name">The command name.</param>
    /// <param name="data">The command data.</param>
    /// <param name="typeResolver">The type resolver.</param>
    public CommandContext(
        IEnumerable<string> arguments,
        IRemainingArguments remaining,
        string name,
        object? data,
        ITypeResolver typeResolver)
    {
        Arguments = arguments.ToSafeReadOnlyList();
        Remaining = remaining ?? throw new System.ArgumentNullException(nameof(remaining));
        Name = name ?? throw new System.ArgumentNullException(nameof(name));
        Data = data;
        TypeResolver = typeResolver ?? throw new System.ArgumentNullException(nameof(typeResolver));
    }
}