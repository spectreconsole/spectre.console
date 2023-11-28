namespace Spectre.Console.Cli.Help;

/// <summary>
/// Represents a command parameter.
/// </summary>
public interface ICommandParameter
{
    /// <summary>
    /// Gets a value indicating whether the parameter is a flag.
    /// </summary>
    bool IsFlag { get; }

    /// <summary>
    /// Gets a value indicating whether the parameter is required.
    /// </summary>
    bool Required { get; }

    /// <summary>
    /// Gets the description of the parameter.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Gets the default value of the parameter, if specified.
    /// </summary>
    DefaultValueAttribute? DefaultValue { get; }

    /// <summary>
    /// Gets a value indicating whether the parameter is hidden.
    /// </summary>
    bool IsHidden { get; }
}