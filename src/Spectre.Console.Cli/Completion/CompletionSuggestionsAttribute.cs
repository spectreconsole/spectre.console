namespace Spectre.Console.Cli.Completion;

/// <summary>
/// An attribute that provides completion suggestions for command line arguments or options.
/// </summary>
/// <remarks>
/// The completion suggestions are used to provide hints to the user about possible values they can use.
/// The attribute can be applied to properties which represent command line arguments or options.
/// </remarks>
[System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
public sealed class CompletionSuggestionsAttribute : Attribute
{
    /// <summary>
    /// Gets the array of suggested completion values.
    /// </summary>
    public string[] Suggestions { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompletionSuggestionsAttribute"/> class with the provided suggestions.
    /// </summary>
    /// <param name="suggestions">An array of suggested completion values.</param>
    public CompletionSuggestionsAttribute(params string[] suggestions)
    {
        Suggestions = suggestions;
    }
}
