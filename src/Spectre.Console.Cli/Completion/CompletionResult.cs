namespace Spectre.Console.Cli.Completion;

/// <summary>
/// Represents a completion result item.
/// </summary>
public class CompletionResultItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompletionResultItem"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="description">The description.</param>
    public CompletionResultItem(string value, string? description = null)
    {
        this.Value = value;
        this.Description = description;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; internal set; }

    /// <summary>
    /// Gets the description.
    /// </summary>
    public string? Description { get; internal set; }

    /// <summary>
    /// Deconstructs the <see cref="CompletionResultItem"/> into its components.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="description">The description.</param>
    public void Deconstruct(out string value, out string? description)
    {
        value = this.Value;
        description = this.Description;
    }

    /// <summary>
    /// Implicitly converts a string to a <see cref="CompletionResultItem"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    public static implicit operator CompletionResultItem(string value) => new(value);
}

/// <summary>
/// Represents a completion result.
/// </summary>
public class CompletionResult : ICompletionResult
{
    /// <summary>
    /// Gets a value indicating whether or not automatic completions should be disabled.
    /// </summary>
    public bool PreventDefault { get; internal set; }

    internal bool PreventAll { get; set; }

    /// <summary>
    /// Gets the suggestions.
    /// </summary>
    public IEnumerable<CompletionResultItem> Suggestions { get; } =
        Enumerable.Empty<CompletionResultItem>();

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
        Suggestions =
            suggestions?.Select(x => new CompletionResultItem(x))
            ?? throw new ArgumentNullException(nameof(suggestions));
        PreventDefault = preventDefault;
    }

    /// <summary>
    ///  Initializes a new instance of the <see cref="CompletionResult"/> class.
    /// </summary>
    /// <param name="suggestions">The suggestions.</param>
    /// <param name="preventDefault">Whether or not to disable auto completions.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="suggestions"/> are <c>null</c>.</exception>
    public CompletionResult(
        IEnumerable<CompletionResultItem> suggestions,
        bool preventDefault = false)
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
        PreventAll = (result as CompletionResult)?.PreventAll ?? false;
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
    /// Creates a new <see cref="CompletionResult"/> with the specified suggestions.
    /// </summary>
    /// <param name="suggestions">The suggestions.</param>
    public static implicit operator CompletionResult(CompletionResultItem[] suggestions)
    {
        return new(suggestions);
    }

    /// <summary>
    /// Creates a new <see cref="CompletionResult"/> with the specified suggestion.
    /// </summary>
    /// <param name="suggestion">The suggestion.</param>
    public static implicit operator CompletionResult(CompletionResultItem suggestion)
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
