namespace Spectre.Console;

internal ref struct WhiteSpaceSegmentEnumerator
{
    private ReadOnlySpan<char> _remaining;
    private ReadOnlySpan<char> _current;

    public WhiteSpaceSegmentEnumerator(ReadOnlySpan<char> buffer)
    {
        _remaining = buffer;
        _current = default;
    }

    public ReadOnlySpan<char> Current => _current;
    public WhiteSpaceSegmentEnumerator GetEnumerator() => this;

    public bool MoveNext()
    {
        if (_remaining.IsEmpty)
        {
            _current = default;
            return false;
        }

        var i = 0;
        var isWhiteSpace = false;
        for (; i < _remaining.Length; i++)
        {
            if (i == 0)
            {
                isWhiteSpace = char.IsWhiteSpace(_remaining[i]);
            }
            else if (char.IsWhiteSpace(_remaining[i]) != isWhiteSpace)
            {
                break;
            }
        }

        _current = _remaining[..i];
        _remaining = _remaining[i..];
        return true;
    }
}