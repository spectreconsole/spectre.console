namespace Spectre.Console.Cli.Help;

/// <summary>
/// Represents a command argument.
/// </summary>
public interface ICommandArgument : ICommandParameter
{
    /// <summary>
    /// Gets the value of the argument.
    /// </summary>
    string Value { get; }

    /// <summary>
    /// Gets the position of the argument.
    /// </summary>
    int Position { get; }
}