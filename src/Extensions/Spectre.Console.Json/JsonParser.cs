namespace Spectre.Console.Json;

internal sealed class JsonParser : IJsonParser
{
    public static JsonParser Shared { get; } = new JsonParser();

    public JsonSyntax Parse(string json)
    {
        try
        {
            var tokens = JsonTokenizer.Tokenize(json);
            var reader = new JsonTokenReader(tokens);
            return ParseElement(reader);
        }
        catch
        {
            throw new InvalidOperationException("Invalid JSON");
        }
    }

    private static JsonSyntax ParseElement(JsonTokenReader reader)
    {
        return ParseValue(reader);
    }

    private static List<JsonSyntax> ParseElements(JsonTokenReader reader)
    {
        var members = new List<JsonSyntax>();

        while (!reader.Eof)
        {
            members.Add(ParseElement(reader));

            if (reader.Peek()?.Type != JsonTokenType.Comma)
            {
                break;
            }

            reader.Consume(JsonTokenType.Comma);
        }

        return members;
    }

    private static JsonSyntax ParseValue(JsonTokenReader reader)
    {
        var current = reader.Peek();
        if (current == null)
        {
            throw new InvalidOperationException("Could not parse value (EOF)");
        }

        if (current.Type == JsonTokenType.LeftBrace)
        {
            return ParseObject(reader);
        }

        if (current.Type == JsonTokenType.LeftBracket)
        {
            return ParseArray(reader);
        }

        if (current.Type == JsonTokenType.Number)
        {
            reader.Consume(JsonTokenType.Number);
            return new JsonNumber(current.Lexeme);
        }

        if (current.Type == JsonTokenType.String)
        {
            reader.Consume(JsonTokenType.String);
            return new JsonString(current.Lexeme);
        }

        if (current.Type == JsonTokenType.Boolean)
        {
            reader.Consume(JsonTokenType.Boolean);
            return new JsonBoolean(current.Lexeme);
        }

        if (current.Type == JsonTokenType.Null)
        {
            reader.Consume(JsonTokenType.Null);
            return new JsonNull(current.Lexeme);
        }

        throw new InvalidOperationException($"Unknown value token: {current.Type}");
    }

    private static JsonSyntax ParseObject(JsonTokenReader reader)
    {
        reader.Consume(JsonTokenType.LeftBrace);

        var result = new JsonObject();

        if (reader.Peek()?.Type != JsonTokenType.RightBrace)
        {
            result.Members.AddRange(ParseMembers(reader));
        }

        reader.Consume(JsonTokenType.RightBrace);
        return result;
    }

    private static JsonSyntax ParseArray(JsonTokenReader reader)
    {
        reader.Consume(JsonTokenType.LeftBracket);

        var result = new JsonArray();

        if (reader.Peek()?.Type != JsonTokenType.RightBracket)
        {
            result.Items.AddRange(ParseElements(reader));
        }

        reader.Consume(JsonTokenType.RightBracket);
        return result;
    }

    private static List<JsonMember> ParseMembers(JsonTokenReader reader)
    {
        var members = new List<JsonMember>();

        while (!reader.Eof)
        {
            members.Add(ParseMember(reader));

            if (reader.Peek()?.Type != JsonTokenType.Comma)
            {
                break;
            }

            reader.Consume(JsonTokenType.Comma);
        }

        return members;
    }

    private static JsonMember ParseMember(JsonTokenReader reader)
    {
        var name = reader.Consume(JsonTokenType.String);
        reader.Consume(JsonTokenType.Colon);
        var value = ParseElement(reader);
        return new JsonMember(name.Lexeme, value);
    }
}
