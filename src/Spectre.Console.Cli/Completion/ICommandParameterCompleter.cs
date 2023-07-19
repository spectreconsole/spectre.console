using System.Linq.Expressions;

namespace Spectre.Console.Cli.Completion;

public interface ICommandParameterCompleter
{
    ICompletionResult GetSuggestions(ICommandParameterInfo parameter, string? prefix);
}

public interface ICompletionResult
{
    bool PreventDefault { get; }
    IEnumerable<string> Suggestions { get; }
}
