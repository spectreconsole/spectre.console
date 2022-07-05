using System.Threading;

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
    /// Gets the cancellation token for async commands.
    /// </summary>
    /// <value>The token.</value>
    public CancellationToken? Token { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandContext"/> class.
    /// </summary>
    /// <param name="remaining">The remaining arguments.</param>
    /// <param name="name">The command name.</param>
    /// <param name="data">The command data.</param>
    /// <param name="token">The cancellation token.</param>
    public CommandContext(IRemainingArguments remaining, string name, object? data, CancellationToken? token)
    {
        Remaining = remaining ?? throw new System.ArgumentNullException(nameof(remaining));
        Name = name ?? throw new System.ArgumentNullException(nameof(name));
        Data = data;
        Token = token;
    }
}