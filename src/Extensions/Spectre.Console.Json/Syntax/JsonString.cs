namespace Spectre.Console.Json.Syntax;

/// <summary>
/// Represents a string literal in the JSON abstract syntax tree.
/// </summary>
public sealed class JsonString : JsonSyntax
{
    /// <summary>
    /// Gets the lexeme.
    /// </summary>
    public string Lexeme { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonString"/> class.
    /// </summary>
    /// <param name="lexeme">The lexeme.</param>
    public JsonString(string lexeme)
    {
        Lexeme = lexeme;
    }

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context)
    {
        visitor.VisitString(this, context);
    }
}
