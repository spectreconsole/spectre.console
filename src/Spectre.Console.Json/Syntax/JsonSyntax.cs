namespace Spectre.Console.Json.Syntax;

/// <summary>
/// Represents a syntax node in the JSON abstract syntax tree.
/// </summary>
public abstract class JsonSyntax
{
    internal abstract void Accept<T>(JsonSyntaxVisitor<T> visitor, T context);
}
