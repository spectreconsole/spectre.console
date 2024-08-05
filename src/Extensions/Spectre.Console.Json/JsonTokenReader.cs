namespace Spectre.Console.Json;

internal sealed class JsonTokenReader
{
    private readonly List<JsonToken> _reader;
    private readonly int _length;

    public int Position { get; private set; }
    public bool Eof => Position >= _length;

    public JsonTokenReader(List<JsonToken> tokens)
    {
        _reader = tokens;
        _length = tokens.Count;

        Position = 0;
    }

    public JsonToken Consume(JsonTokenType type)
    {
        var read = Read();
        if (read == null)
        {
            throw new InvalidOperationException("Could not read token");
        }

        if (read.Type != type)
        {
            throw new InvalidOperationException($"Expected '{type}' token, but found '{read.Type}'");
        }

        return read;
    }

    public JsonToken? Peek()
    {
        if (Eof)
        {
            return null;
        }

        return _reader[Position];
    }

    public JsonToken? Read()
    {
        if (Eof)
        {
            return null;
        }

        Position++;
        return _reader[Position - 1];
    }
}
