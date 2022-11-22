namespace Spectre.Console.Json.Syntax;

/// <summary>
/// Represents a null literal in the JSON abstract syntax tree.
/// </summary>
public sealed class JsonNull : JsonSyntax
{
    /// <summary>
    /// Gets the lexeme.
    /// </summary>
    public string Lexeme { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonNull"/> class.
    /// </summary>
    /// <param name="lexeme">The lexeme.</param>
    public JsonNull(string lexeme)
    {
        Lexeme = lexeme;
    }

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context)
    {
        visitor.VisitNull(this, context);
    }
}
