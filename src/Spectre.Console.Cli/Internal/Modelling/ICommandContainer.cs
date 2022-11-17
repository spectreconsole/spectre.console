namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command container.
/// </summary>
internal interface ICommandContainer
{
    /// <summary>
    /// Gets all commands in the container.
    /// </summary>
    IList<CommandInfo> Commands { get; }

    /// <summary>
    /// Gets the default command for the container.
    /// </summary>
    /// <remarks>
    /// Returns null if a default command has not been set.
    /// </remarks>
    CommandInfo? DefaultCommand { get; }
}