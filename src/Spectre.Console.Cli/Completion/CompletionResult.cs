namespace Spectre.Console.Cli.Completion;

/// <summary>
/// Represents a completion result.
/// </summary>
public class CompletionResult : ICompletionResult
{
    /// <summary>
    /// Gets a value indicating whether or not automatic completions should be disabled.
    /// </summary>
    public bool PreventDefault { get; }

    /// <summary>
    /// Gets the suggestions.
    /// </summary>
    public IEnumerable<string> Suggestions { get; } = Enumerable.Empty<string>();

    /// <summary>
    /// Gets a value indicating whether or not the suggestions were automatically generated.
    /// </summary>
    internal bool IsGenerated { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompletionResult"/> class.
    /// </summary>
    public CompletionResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompletionResult"/> class.
    /// </summary>
    /// <param name="suggestions">The suggestions.</param>
    /// <param name="preventDefault">Whether or not to disable auto completions.</param>
    public CompletionResult(IEnumerable<string> suggestions, bool preventDefault = false)
    {
        Suggestions = suggestions ?? throw new ArgumentNullException(nameof(suggestions));
        PreventDefault = preventDefault;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompletionResult"/> class.
    /// </summary>
    /// <param name="result">The suggestions.</param>
    public CompletionResult(ICompletionResult result)
    {
        Suggestions = result.Suggestions;
        PreventDefault = result.PreventDefault;
    }

    /// <summary>
    /// Creates a new <see cref="CompletionResult"/> with the specified suggestions.
    /// </summary>
    /// <param name="suggestions">The suggestions.</param>
    /// <returns>A copy of this instance so that multiple calls can be chained.</returns>
    public CompletionResult WithSuggestions(IEnumerable<string> suggestions)
    {
        return new(suggestions, PreventDefault);
    }

    /// <summary>
    /// Disables completions, that are automatically generated.
    /// </summary>
    /// <returns>A copy of this instance so that multiple calls can be chained.</returns>
    /// <param name="preventDefault">Whether or not to disable auto completions.</param>
    public CompletionResult WithPreventDefault(bool preventDefault = true)
    {
        return new(Suggestions, preventDefault);
    }

    /// <summary>
    /// Marks the suggestions as automatically generated.
    /// </summary>
    internal CompletionResult WithGeneratedSuggestions()
    {
        return new(Suggestions, PreventDefault) { IsGenerated = true };
    }

    /// <summary>
    /// Creates a new <see cref="CompletionResult"/> with the specified suggestions.
    /// </summary>
    /// <param name="suggestions">The suggestions.</param>
    public static implicit operator CompletionResult(string[] suggestions)
    {
        return new(suggestions);
    }

    /// <summary>
    /// Creates a new <see cref="CompletionResult"/> with the specified suggestion.
    /// </summary>
    /// <param name="suggestion">The suggestion.</param>
    public static implicit operator CompletionResult(string suggestion)
    {
        return new(new[] { suggestion });
    }

    /// <summary>
    /// Creates a new <see cref="CompletionResult"/> with no suggestions.
    /// </summary>
    /// <returns>A new <see cref="CompletionResult"/> with no suggestions.</returns>
    public static CompletionResult None()
    {
        return new();
    }

    /// <summary>
    /// Creates a new <see cref="CompletionResult"/> with the specified suggestions.
    /// </summary>
    /// <param name="suggestions">The suggestions.</param>
    /// <returns>A new <see cref="CompletionResult"/> with the specified suggestions.</returns>
    public static CompletionResult Result(params string[] suggestions)
    {
        return new(suggestions);
    }
}