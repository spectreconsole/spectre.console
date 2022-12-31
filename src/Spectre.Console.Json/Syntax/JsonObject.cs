namespace Spectre.Console.Json.Syntax;

/// <summary>
/// Represents an object in the JSON abstract syntax tree.
/// </summary>
public sealed class JsonObject : JsonSyntax
{
    /// <summary>
    /// Gets the object's members.
    /// </summary>
    public List<JsonMember> Members { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonObject"/> class.
    /// </summary>
    public JsonObject()
    {
        Members = new List<JsonMember>();
    }

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context)
    {
        visitor.VisitObject(this, context);
    }
}
