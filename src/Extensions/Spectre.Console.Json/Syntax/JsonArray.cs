namespace Spectre.Console.Json.Syntax;

/// <summary>
/// Represents an array in the JSON abstract syntax tree.
/// </summary>
public sealed class JsonArray : JsonSyntax
{
    /// <summary>
    /// Gets the array items.
    /// </summary>
    public List<JsonSyntax> Items { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonArray"/> class.
    /// </summary>
    public JsonArray()
    {
        Items = new List<JsonSyntax>();
    }

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context)
    {
        visitor.VisitArray(this, context);
    }
}