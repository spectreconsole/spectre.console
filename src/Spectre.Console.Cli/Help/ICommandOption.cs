namespace Spectre.Console.Cli.Help;

/// <summary>
/// Represents a command option.
/// </summary>
public interface ICommandOption : ICommandParameter
{
    /// <summary>
    /// Gets the long names of the option.
    /// </summary>
    IReadOnlyList<string> LongNames { get; }

    /// <summary>
    /// Gets the short names of the option.
    /// </summary>
    IReadOnlyList<string> ShortNames { get; }

    /// <summary>
    /// Gets the value name of the option, if applicable.
    /// </summary>
    string? ValueName { get; }

    /// <summary>
    /// Gets a value indicating whether the option value is optional.
    /// </summary>
    bool ValueIsOptional { get; }
}