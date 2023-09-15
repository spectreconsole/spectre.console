namespace Spectre.Console.Cli.Completion;

[System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
public sealed class CompletionSuggestionsAttribute : Attribute
{
    public string[] Suggestions { get; }

    public CompletionSuggestionsAttribute(params string[] suggestions)
    {
        Suggestions = suggestions;
    }
}