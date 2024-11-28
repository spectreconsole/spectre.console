namespace Spectre.Console.Cli.Help;

/// <summary>
/// Represents an executable command.
/// </summary>
public interface ICommandInfo : ICommandContainer
{
    /// <summary>
    /// Gets the name of the command.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the description of the command.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Gets a value indicating whether the command is a branch.
    /// </summary>
    bool IsBranch { get; }

    /// <summary>
    /// Gets a value indicating whether the command is the default command within its container.
    /// </summary>
    bool IsDefaultCommand { get; }

    /// <summary>
    /// Gets a value indicating whether the command is hidden.
    /// </summary>
    bool IsHidden { get; }

    /// <summary>
    /// Gets the parameters associated with the command.
    /// </summary>
    IReadOnlyList<ICommandParameter> Parameters { get; }

    /// <summary>
    /// Gets the parent command, if any.
    /// </summary>
    ICommandInfo? Parent { get; }
}