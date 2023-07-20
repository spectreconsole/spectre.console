namespace Spectre.Console.Cli.Completion;

[System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
internal sealed class CompletionSuggestionsAttribute : Attribute
{
    public string[] Suggestions { get; }

    public CompletionSuggestionsAttribute(params string[] suggestions)
    {
        Suggestions = suggestions;
    }
}