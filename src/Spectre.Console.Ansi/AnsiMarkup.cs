namespace Spectre.Console;

/// <summary>
/// Utility used for working with markup text.
/// </summary>
public sealed class AnsiMarkup
{
    private readonly AnsiWriter _writer;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiMarkup"/> class.
    /// </summary>
    /// <param name="writer">The ANSI writer to use for writing.</param>
    public AnsiMarkup(AnsiWriter writer)
    {
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
    }

    /// <summary>
    /// Outputs the specified markup.
    /// </summary>
    /// <param name="markup"></param>
    /// <param name="style"></param>
    public void Write(string markup, Style? style = null)
    {
        foreach (var segment in Parse(markup, style))
        {
            _writer.Write(segment.Text, segment.Style);
        }
    }

    /// <summary>
    /// Outputs the specified markup, followed by the current line terminator
    /// </summary>
    /// <param name="markup"></param>
    public void WriteLine(string markup)
    {
        Write(markup);
        _writer.WriteLine();
    }

    /// <summary>
    /// Parses the specified markup text into segments that can be processed.
    /// </summary>
    /// <param name="markup"></param>
    /// <param name="style"></param>
    /// <returns></returns>
    public static IEnumerable<AnsiMarkupSegment> Parse(string markup, Style? style = null)
    {
        ArgumentNullException.ThrowIfNull(markup);

        style ??= Style.Plain;

        using var tokenizer = new MarkupTokenizer(markup);

        var result = new List<AnsiMarkupSegment>();
        var stack = new Stack<Style>();

        while (tokenizer.MoveNext())
        {
            var token = tokenizer.Current;
            if (token == null)
            {
                break;
            }

            if (token.Kind == MarkupTokenKind.Open)
            {
                var parsedStyle = string.IsNullOrEmpty(token.Value) ? Style.Plain : StyleParser.Parse(token.Value);
                stack.Push(parsedStyle);
            }
            else if (token.Kind == MarkupTokenKind.Close)
            {
                if (stack.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"Encountered closing tag when none was expected near position {token.Position}.");
                }

                stack.Pop();
            }
            else if (token.Kind == MarkupTokenKind.Text)
            {
                var effectiveStyle = style.Combine(stack.Reverse());
                if (result.Count > 0 && result[^1].Style.Equals(effectiveStyle))
                {
                    // Merge segments
                    result[^1].Text += token.Value;
                }
                else
                {
                    result.Add(
                        new AnsiMarkupSegment(
                        token.Value, effectiveStyle));
                }
            }
            else
            {
                throw new InvalidOperationException("Encountered unknown markup token.");
            }
        }

        if (stack.Count > 0)
        {
            throw new InvalidOperationException("Unbalanced markup stack. Did you forget to close a tag?");
        }

        return result;
    }

    /// <summary>
    /// Escapes the specified text so that it won’t be interpreted as markup.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Escape(string? text)
    {
        if (text == null)
        {
            return string.Empty;
        }

        return text
            .ReplaceExact("[", "[[")
            .ReplaceExact("]", "]]");
    }

    /// <summary>
    /// Removes markup from the specified text.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Remove(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var result = new StringBuilder();

        using var tokenizer = new MarkupTokenizer(text);
        while (tokenizer.MoveNext() && tokenizer.Current != null)
        {
            if (tokenizer.Current.Kind == MarkupTokenKind.Text)
            {
                result.Append(tokenizer.Current.Value);
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Highlights the first text match in provided markup.
    /// </summary>
    /// <param name="markup">The markup text.</param>
    /// <param name="query">The search text to highlight.</param>
    /// <param name="style">The highlight style.</param>
    /// <returns>Highlighted markup text, using the specified style.</returns>
    public static string Highlight(string markup, string query, Style style)
    {
        return AnsiMarkupHighlighter.Highlight(markup, query, style);
    }
}

/// <summary>
/// Represents a markup segment.
/// </summary>
public sealed class AnsiMarkupSegment
{
    /// <summary>
    /// Gets the segment text.
    /// </summary>
    public string Text { get; internal set; }

    /// <summary>
    /// Gets the segment style.
    /// </summary>
    public Style Style { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiMarkupSegment"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="style">The style.</param>
    public AnsiMarkupSegment(string text, Style style)
    {
        Text = text;
        Style = style;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return !Style.Equals(Style.Plain)
            ? $"[{Style.ToMarkup()}]{Text}[/]"
            : Text;
    }
}

file enum MarkupTokenKind
{
    Text = 0,
    Open,
    Close,
}

file sealed class MarkupToken
{
    public MarkupTokenKind Kind { get; }
    public string Value { get; }
    public int Position { get; set; }

    public MarkupToken(MarkupTokenKind kind, string value, int position)
    {
        Kind = kind;
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Position = position;
    }
}

file sealed class MarkupTokenizer : IDisposable
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