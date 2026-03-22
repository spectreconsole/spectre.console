namespace Spectre.Console.Ansi;

internal sealed class OscParser
{
    private char[] _buffer = new char[2048];
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
        _buffer = new char[2048];
        _index = 0;
        _writeToBuffer = false;
        _state = State.Start;
    }

    public void Next(char code)
    {
        if (_state == State.Invalid)
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
        if (_state == State.Osc8)
        {
            return OscHyperLinkParser.Parse(_buffer.AsSpan(0, _index));
        }

        return null;
    }
}