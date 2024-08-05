namespace Spectre.Console.Json.Syntax;

/// <summary>
/// Represents a member in the JSON abstract syntax tree.
/// </summary>
public sealed class JsonMember : JsonSyntax
{
    /// <summary>
    /// Gets the member name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the member value.
    /// </summary>
    public JsonSyntax Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonMember"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    public JsonMember(string name, JsonSyntax value)
    {
        Name = name;
        Value = value;
    }

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context)
    {
        visitor.VisitMember(this, context);
    }
}
