namespace Spectre.Console.Json.Syntax;

internal sealed class JsonNumber : JsonSyntax
{
    public string Lexeme { get; }

    public JsonNumber(string lexeme)
    {
        Lexeme = lexeme;
    }

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context)
    {
        visitor.VisitNumber(this, context);
    }
}
