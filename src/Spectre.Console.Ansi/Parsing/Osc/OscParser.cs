namespace Spectre.Console.Ansi;

[Experimental(Constants.ExperimentalParserId)]
internal sealed class OscParser
{
    // Upper bound on an OSC string. A longer string is dropped rather than dispatched cut off,
    // since a truncated URI or payload would still look valid.
    private const int MaxBufferSize = 2048;

    private readonly char[] _buffer = new char[MaxBufferSize];
    private int _length;
    private int _payloadStart;
    private bool _overflowed;
    private State _state = State.Start;

    private enum State
    {
        Invalid = 0,
        Start = 1,
        Osc8 = 2,
        HyperLink = 3,
    }

    public void Reset()
    {
        // Reuse the buffer; every read is bounded by _length, so stale data is never observed.
        _length = 0;
        _payloadStart = 0;
        _overflowed = false;
        _state = State.Start;
    }

    public void Next(char code)
    {
        if (_length == MaxBufferSize)
        {
            _overflowed = true;
            return;
        }

        // Accumulate the whole string so an unknown OSC command can be reported in full.
        _buffer[_length] = code;
        _length++;

        switch (_state)
        {
            case State.Start:
                _state = code == '8' ? State.Osc8 : State.Invalid;
                break;
            case State.Osc8:
                if (code == ';')
                {
                    // The OSC 8 payload (params and URI) starts just after the "8;" prefix.
                    _payloadStart = _length;
                    _state = State.HyperLink;
                }
                else
                {
                    _state = State.Invalid;
                }

                break;
        }
    }

    public OscCommand? End()
    {
        if (_overflowed)
        {
            return null;
        }

        switch (_state)
        {
            case State.HyperLink when _length > _payloadStart:
                return OscHyperLinkParser.Parse(_buffer.AsSpan(_payloadStart, _length - _payloadStart));
            case State.Invalid when _length > 0:
                return new OscCommand.Unknown(Data: new string(_buffer, 0, _length));
            default:
                return null;
        }
    }
}