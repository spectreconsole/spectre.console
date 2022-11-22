namespace Spectre.Console.Json;

internal sealed class JsonToken
{
    public JsonTokenType Type { get; }
    public string Lexeme { get; }

    public JsonToken(JsonTokenType type, string lexeme)
    {
        Type = type;
        Lexeme = lexeme ?? throw new ArgumentNullException(nameof(lexeme));
    }
}
