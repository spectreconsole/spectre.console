namespace Spectre.Console.Json.Syntax;

/// <summary>
/// Represents a boolean literal in the JSON abstract syntax tree.
/// </summary>
public sealed class JsonBoolean : JsonSyntax
{
    /// <summary>
    /// Gets the lexeme.
    /// </summary>
    public string Lexeme { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonBoolean"/> class.
    /// </summary>
    /// <param name="lexeme">The lexeme.</param>
    public JsonBoolean(string lexeme)
    {
        Lexeme = lexeme;
    }

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context)
    {
        visitor.VisitBoolean(this, context);
    }
}
