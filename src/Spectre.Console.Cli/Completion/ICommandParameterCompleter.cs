namespace Spectre.Console.Cli.Completion;

/// <summary>
/// Represents a command parameter completer.
/// </summary>
public interface ICommandCompletable
{
    /// <summary>
    /// Gets the suggestions for the specified parameter.
    /// </summary>
    /// <param name="parameter">Information on which parameter to get suggestions for.</param>
    /// <param name="prefix">The prefix.</param>
    /// <returns>The suggestions for the specified parameter.</returns>
    CompletionResult GetSuggestions(ICommandParameterInfo parameter, string? prefix);
}
