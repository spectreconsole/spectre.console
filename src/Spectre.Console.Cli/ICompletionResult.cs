namespace Spectre.Console.Cli.Completion;

/// <summary>
/// Represents a completion result.
/// </summary>
public interface ICompletionResult
{
    /// <summary>
    /// Gets a value indicating whether or not automatic completions should be disabled.
    /// </summary>
    bool PreventDefault { get; }

    /// <summary>
    /// Gets the suggestions.
    /// </summary>
    IEnumerable<string> Suggestions { get; }
}
