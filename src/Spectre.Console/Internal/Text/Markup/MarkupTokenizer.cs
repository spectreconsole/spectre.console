namespace Spectre.Console;

internal sealed class MarkupTokenizer : IDisposable
{
    private readonly StringBuffer _reader;

    public MarkupToken? Current { get; private set; }

    public MarkupTokenizer(string text)
    {
        _reader = new StringBuffer(text ?? throw new ArgumentNullException(nameof(text)));
    }

    public void Dispose()
    {
        _reader.Dispose();
    }

    public bool MoveNext()
    {
        if (_reader.Eof)
        {
            return false;
        }

        var current = _reader.Peek();
        return current == '[' ? ReadMarkup() : ReadText();
    }

    private bool ReadText()
    {
        var position = _reader.Position;
        var builder = new StringBuilder();

        var encounteredClosing = false;
        while (!_reader.Eof)
        {
            var current = _reader.Peek();
            if (current == '[')
            {
                // markup encountered. Stop processing.
                break;
            }

            // If we find a closing tag (']') there must be two of them.
            if (current == ']')
            {
                if (encounteredClosing)
                {
                    _reader.Read();
                    encounteredClosing = false;
                    continue;
                }

                encounteredClosing = true;
            }
            else
            {
                if (encounteredClosing)
                {
                    throw new InvalidOperationException(
                        $"Encountered unescaped ']' token at position {_reader.Position}");
                }
            }

            builder.Append(_reader.Read());
        }

        if (encounteredClosing)
        {
            throw new InvalidOperationException($"Encountered unescaped ']' token at position {_reader.Position}");
        }

        Current = new MarkupToken(MarkupTokenKind.Text, builder.ToString(), position);
        return true;
    }

    private bool ReadMarkup()
    {
        var position = _reader.Position;

        _reader.Read();

        if (_reader.Eof)
        {
            throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position}.");
        }

        var current = _reader.Peek();
        switch (current)
        {
            case '[':
                // No markup but instead escaped markup in text.
                _reader.Read();
                Current = new MarkupToken(MarkupTokenKind.Text, "[", position);
                return true;
            case '/':
                // Markup closed.
                _reader.Read();

                if (_reader.Eof)
                {
                    throw new InvalidOperationException(
                        $"Encountered malformed markup tag at position {_reader.Position}.");
                }

                current = _reader.Peek();
                if (current != ']')
                {
                    throw new InvalidOperationException(
                        $"Encountered malformed markup tag at position {_reader.Position}.");
                }

                _reader.Read();
                Current = new MarkupToken(MarkupTokenKind.Close, string.Empty, position);
                return true;
        }

        // Read the "content" of the markup until we find the end-of-markup
        var builder = new StringBuilder();
        var encounteredOpening = false;
        var encounteredClosing = false;
        while (!_reader.Eof)
        {
            var currentStylePartCanContainMarkup =
                builder.ToString()
                .Split(' ')
                .Last()
                .StartsWith("link=", StringComparison.OrdinalIgnoreCase);
            current = _reader.Peek();

            if (currentStylePartCanContainMarkup)
            {
                switch (current)
                {
                    case ']' when !encounteredOpening:
                        if (encounteredClosing)
                        {
                            builder.Append(_reader.Read());
                            encounteredClosing = false;
                            continue;
                        }

                        _reader.Read();
                        encounteredClosing = true;
                        continue;

                    case '[' when !encounteredClosing:
                        if (encounteredOpening)
                        {
                            builder.Append(_reader.Read());
                            encounteredOpening = false;
                            continue;
                        }

                        _reader.Read();
                        encounteredOpening = true;
                        continue;
                }
            }
            else
            {
                switch (current)
                {
                    case ']':
                        _reader.Read();
                        encounteredClosing = true;
                        break;
                    case '[':
                        _reader.Read();
                        encounteredOpening = true;
                        break;
                }
            }

            if (encounteredClosing)
            {
                break;
            }

            if (encounteredOpening)
            {
                throw new InvalidOperationException(
                    $"Encountered malformed markup tag at position {_reader.Position - 1}.");
            }

            builder.Append(_reader.Read());
        }

        if (_reader.Eof)
        {
            throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position}.");
        }

        Current = new MarkupToken(MarkupTokenKind.Open, builder.ToString(), position);
        return true;
    }
}