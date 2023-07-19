namespace Spectre.Console.Cli.Completion;

/// <summary>
/// Represents a command parameter completer.
/// </summary>
public interface ICommandParameterCompleter
{
    /// <summary>
    /// Gets the suggestions for the specified parameter.
    /// </summary>
    /// <param name="parameter">Information on which parameter to get suggestions for.</param>
    /// <param name="prefix">The prefix.</param>
    /// <returns>The suggestions for the specified parameter.</returns>
    ICompletionResult GetSuggestions(ICommandParameterInfo parameter, string? prefix);
}
