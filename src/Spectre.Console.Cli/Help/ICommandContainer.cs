namespace Spectre.Console.Cli.Help;

/// <summary>
/// Represents a command container.
/// </summary>
public interface ICommandContainer
{
    /// <summary>
    /// Gets all the examples for the container.
    /// </summary>
    IList<string[]> Examples { get; }

    /// <summary>
    /// Gets all commands in the container.
    /// </summary>
    IList<ICommandInfo> Commands { get; }

    /// <summary>
    /// Gets the default command for the container.
    /// </summary>
    /// <remarks>
    /// Returns null if a default command has not been set.
    /// </remarks>
    ICommandInfo? DefaultCommand { get; }
}
