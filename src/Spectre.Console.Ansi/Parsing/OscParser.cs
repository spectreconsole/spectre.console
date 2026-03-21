namespace Spectre.Console.Ansi;

internal enum OscParserState
{
    Invalid = 0,
    Start = 1,
    Osc8 = 2,
}

internal sealed class OscParser
{
    private char[] _buffer = new char[2048];
    private int _index = 0;
    private bool _writeToBuffer = false;
    private OscParserState _state = OscParserState.Start;

    public void Reset()
    {
        _buffer = new char[2048];
        _index = 0;
        _writeToBuffer = false;
        _state = OscParserState.Start;
    }

    public void Next(char code)
    {
        if (_state == OscParserState.Invalid)
        {
            return;
        }

        if (_writeToBuffer)
        {
            _buffer[_index] = code;
            _index++;
            return;
        }

        switch (_state)
        {
            case OscParserState.Invalid:
                break;
            case OscParserState.Start:
                switch (code)
                {
                    case '8':
                        _state = OscParserState.Osc8;
                        break;
                    default:
                        _state = OscParserState.Invalid;
                        break;
                }

                break;
            case OscParserState.Osc8:
                switch (code)
                {
                    case ';':
                        _writeToBuffer = true;
                        break;
                    default:
                        _state = OscParserState.Invalid;
                        break;
                }

                break;
        }
    }

    public OscCommand? End(char terminator)
    {
        if (_state == OscParserState.Osc8)
        {
            return OscHyperLinkParser.Parse(_buffer.AsSpan(0, _index));
        }

        return null;
    }
}

file static class OscHyperLinkParser
{
    public static OscCommand? Parse(ReadOnlySpan<char> input)
    {
        string? id = null;

        // Find the final ';' which separates params from the URI.
        var lastSemicolon = input.LastIndexOf(';');
        if (lastSemicolon < 0)
        {
            return new OscCommand.HyperLinkStart(null, input.ToString());
        }

        var parameters = input[..lastSemicolon];
        var uri = input[(lastSemicolon + 1)..];

        // Parse key=value pairs from the params section.
        while (!parameters.IsEmpty)
        {
            var equalsIndex = parameters.IndexOf('=');
            if (equalsIndex < 0)
            {
                break;
            }

            var key = parameters[..equalsIndex].Trim();
            parameters = parameters[(equalsIndex + 1)..];

            var semicolonIndex = parameters.IndexOf(';');
            var value = semicolonIndex < 0
                ? parameters.Trim()
                : parameters[..semicolonIndex].Trim();

            if (key.SequenceEqual("id"))
            {
                id = value.IsEmpty ? null : value.ToString();
            }

            if (semicolonIndex < 0)
            {
                break;
            }

            parameters = parameters[(semicolonIndex + 1)..];
        }

        if (uri.Length == 0)
        {
            return new OscCommand.HyperLinkEnd();
        }

        return new OscCommand.HyperLinkStart(id, uri.ToString());
    }
}