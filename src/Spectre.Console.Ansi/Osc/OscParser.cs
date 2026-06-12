namespace Spectre.Console.Ansi;

internal sealed class OscParser
{
    private const int MaxBufferSize = 2048;

    private char[] _buffer = new char[MaxBufferSize];
    private int _bufferIndex = 0;
    private char[] _everything = new char[MaxBufferSize];
    private int _index = 0;
    private bool _writeToBuffer = false;
    private State _state = State.Start;

    private enum State
    {
        Invalid = 0,
        Start = 1,
        Osc8 = 2,
    }

    public void Reset()
    {
        _buffer = new char[MaxBufferSize];
        _everything = new char[MaxBufferSize];
        _bufferIndex = 0;
        _index = 0;
        _writeToBuffer = false;
        _state = State.Start;
    }

    public void Next(char code)
    {
        // Keep track of everything so we can submit unknown OSC commands.
        // Not pretty, but it solves the problem. We should perhaps rethink this parser.
        if (_index < MaxBufferSize)
        {
            _everything[_index] = code;
            _index++;
        }

        if (_state == State.Invalid)
        {
            return;
        }

        if (_writeToBuffer)
        {
            if (_bufferIndex < MaxBufferSize)
            {
                _buffer[_bufferIndex] = code;
                _bufferIndex++;
            }

            return;
        }

        switch (_state)
        {
            case State.Invalid:
                break;
            case State.Start:
                switch (code)
                {
                    case '8':
                        _state = State.Osc8;
                        break;
                    default:
                        _state = State.Invalid;
                        break;
                }

                break;
            case State.Osc8:
                switch (code)
                {
                    case ';':
                        _writeToBuffer = true;
                        break;
                    default:
                        _state = State.Invalid;
                        break;
                }

                break;
        }
    }

    public OscCommand? End(char terminator)
    {
        if (_state == State.Osc8 && _bufferIndex > 0)
        {
            return OscHyperLinkParser.Parse(_buffer.AsSpan(0, _bufferIndex));
        }

        if (_state == State.Invalid && _index > 0)
        {
            return new OscCommand.Unknown(Data: new string(_everything, 0, _index));
        }

        return null;
    }
}