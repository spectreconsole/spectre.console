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
    /// <param name="markup">The markup to write.</param>
    /// <param name="style">The base style to use during parsing.</param>
    public void Write(string markup, Style? style = null)
    {
        foreach (var segment in Parse(markup, style))
        {
            _writer.Write(segment.Text, segment.Style, segment.Link);
        }
    }

    /// <summary>
    /// Outputs the specified markup, followed by the current line terminator
    /// </summary>
    /// <param name="markup">The markup to write.</param>
    public void WriteLine(string markup)
    {
        Write(markup);
        _writer.WriteLine();
    }

    /// <summary>
    /// Parses the specified markup text into segments that can be processed.
    /// </summary>
    /// <param name="markup">The markup to parse.</param>
    /// <param name="style">The base style to use when parsing.</param>
    /// <returns>One or more segments that represents the parsed markup.</returns>
    public static IEnumerable<AnsiMarkupSegment> Parse(string markup, Style? style = null)
    {
        ArgumentNullException.ThrowIfNull(markup);

        style ??= Style.Plain;

        using var tokenizer = new MarkupTokenizer(markup);

        var result = new List<AnsiMarkupSegment>();
        var stack = new Stack<Style>();
        var link = default(Link?);

        while (tokenizer.MoveNext())
        {
            var token = tokenizer.Current;

            if (token.Kind == MarkupTokenKind.Open)
            {
                var parsed = AnsiMarkupTagParser.Parse(token.Value);
                link ??= parsed.Link;
                stack.Push(style.Value);
                style = style.Value.Combine(parsed.Style);
            }
            else if (token.Kind == MarkupTokenKind.Close)
            {
                if (stack.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"Encountered closing tag when none was expected near position {token.Position}.");
                }

                style = stack.Pop();
            }
            else if (token.Kind == MarkupTokenKind.Text)
            {
                if (result.Count > 0 && result[^1].Style.Equals(style))
                {
                    // Merge segments
                    result[^1].Text += token.Value;
                }
                else
                {
                    result.Add(
                        new AnsiMarkupSegment(
                        token.Value, style.Value, link));
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
    /// <param name="markup">The markup to escape.</param>
    /// <returns>The escaped markup.</returns>
    public static string Escape(string? markup)
    {
        if (markup == null)
        {
            return string.Empty;
        }

        return markup
            .ReplaceExact("[", "[[")
            .ReplaceExact("]", "]]");
    }

    /// <summary>
    /// Removes markup from the specified text.
    /// </summary>
    /// <param name="markup">The markup to clean.</param>
    /// <returns>The provided text without markup.</returns>
    public static string Remove(string? markup)
    {
        if (string.IsNullOrWhiteSpace(markup))
        {
            return string.Empty;
        }

        var result = new StringBuilder();

        using var tokenizer = new MarkupTokenizer(markup);
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
    /// Gets the segment link.
    /// </summary>
    public Link? Link { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiMarkupSegment"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="style">The style.</param>
    /// <param name="link">The link.</param>
    public AnsiMarkupSegment(string text, Style style, Link? link)
    {
        Text = text;
        Style = style;
        Link = link;
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

    public MarkupToken Current { get => field!; private set; }

    public MarkupTokenizer(string text)
    {
        _reader = new StringBuffer(text);
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

        Current = _reader.Peek() == '['
            ? ReadMarkup()
            : ReadText();

        return true;
    }

    private MarkupToken ReadText()
    {
        var position = _reader.Position;
        var builder = new StringBuilder();

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
                _reader.Read();
                if (_reader.Peek() != ']')
                {
                    throw new InvalidOperationException(
                        $"Encountered unescaped ']' token at position {_reader.Position}");
                }
            }

            builder.Append(_reader.Read());
        }

        return new MarkupToken(MarkupTokenKind.Text, builder.ToString(), position);
    }

    private MarkupToken ReadMarkup()
    {
        var position = _reader.Position;

        // Read initial opening bracket
        _reader.Read();

        if (_reader.Eof)
        {
            ThrowMalformed(_reader.Position);
        }

        switch (_reader.Peek())
        {
            case '[':
                // No markup but instead escaped markup in text.
                _reader.Read();
                return new MarkupToken(MarkupTokenKind.Text, "[", position);
            case '/':
                // Markup closed.
                _reader.Read();

                if (_reader.Eof)
                {
                    ThrowMalformed(_reader.Position);
                }

                if (_reader.Read() != ']')
                {
                    ThrowMalformed(_reader.Position - 1);
                }

                return new MarkupToken(MarkupTokenKind.Close, string.Empty, position);
        }

        // Read the "content" of the markup until we find the end-of-markup
        var builder = new StringBuilder();
        var currentStylePartCanContainMarkup = false;
        while (!_reader.Eof)
        {
            var current = _reader.Read();

            if (current == ']')
            {
                if (!currentStylePartCanContainMarkup || _reader.Peek() != ']')
                {
                    // Not parsing a link or not escaped. Markup closed
                    break;
                }

                _reader.Read();
            }

            if (current == '[')
            {
                if (!currentStylePartCanContainMarkup || _reader.Peek() != '[')
                {
                    ThrowMalformed(_reader.Position - 1);
                }

                _reader.Read();
            }

            builder.Append(current);

            const string LinkPrefix = "link=";
            if (current == ' ')
            {
                currentStylePartCanContainMarkup = false;
            }
            // Only check if we're not already parsing a link & we added the last character of the prefix
            else if (!currentStylePartCanContainMarkup && current == LinkPrefix[^1] && builder.Length >= LinkPrefix.Length)
            {
                currentStylePartCanContainMarkup =
                    (builder.Length == LinkPrefix.Length || builder[^(LinkPrefix.Length + 1)] == ' ')
                    && builder.ToString(builder.Length - LinkPrefix.Length, LinkPrefix.Length)
                        .Equals(LinkPrefix, StringComparison.OrdinalIgnoreCase);
            }
        }

        if (_reader.Eof)
        {
            ThrowMalformed(_reader.Position);
        }

        return new MarkupToken(MarkupTokenKind.Open, builder.ToString(), position);
    }

    [DoesNotReturn]
    private static void ThrowMalformed(int pos) =>
        throw new InvalidOperationException($"Encountered malformed markup tag at position {pos}.");
}
