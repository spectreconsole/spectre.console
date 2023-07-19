namespace Spectre.Console.Cli.Completion;

public class CompletionResult : ICompletionResult
{
    public bool PreventDefault { get; }
    public IEnumerable<string> Suggestions { get; } = Enumerable.Empty<string>();

    internal bool IsGenerated { get; private set; }

    public CompletionResult()
    {
    }

    public CompletionResult(IEnumerable<string> suggestions, bool preventDefault = false)
    {
        Suggestions = suggestions ?? throw new ArgumentNullException(nameof(suggestions));
        PreventDefault = preventDefault;
    }

    public CompletionResult(ICompletionResult result)
    {
        Suggestions = result.Suggestions;
        PreventDefault = result.PreventDefault;
    }

    public CompletionResult WithSuggestions(IEnumerable<string> suggestions)
    {
        return new(suggestions, PreventDefault);
    }

    /// <summary>
    /// Disables completions, that are automatically generated
    /// </summary>
    /// <param name="preventDefault"></param>
    /// <returns></returns>
    public CompletionResult WithPreventDefault(bool preventDefault = true)
    {
        return new(Suggestions, preventDefault);
    }

    public CompletionResult WithGeneratedSuggestions()
    {
        return new(Suggestions, PreventDefault) { IsGenerated = true };
    }

    public static implicit operator CompletionResult(string[] suggestions)
    {
        return new(suggestions);
    }

    public static implicit operator CompletionResult(string suggestion)
    {
        return new(new[] { suggestion });
    }

    public static CompletionResult None()
    {
        return new();
    }

    public static CompletionResult Result(params string[] suggestions)
    {
        return new(suggestions);
    }
}
