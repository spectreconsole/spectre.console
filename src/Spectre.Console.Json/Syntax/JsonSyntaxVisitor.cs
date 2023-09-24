namespace Spectre.Console.Json.Syntax;

internal abstract class JsonSyntaxVisitor<T>
{
    public abstract void VisitObject(JsonObject syntax, T context);
    public abstract void VisitArray(JsonArray syntax, T context);
    public abstract void VisitMember(JsonMember syntax, T context);
    public abstract void VisitNumber(JsonNumber syntax, T context);
    public abstract void VisitString(JsonString syntax, T context);
    public abstract void VisitBoolean(JsonBoolean syntax, T context);
    public abstract void VisitNull(JsonNull syntax, T context);
}
