using System.Globalization;

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
            if (segment.IsControlCode)
            {
                _writer.Write(segment.Text);
            }
            else
            {
                _writer.Write(segment.Text, segment.Style, segment.Link);
            }
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
        var styleStack = new Stack<Style>();
        var linkStack = new Stack<Link?>();
        var link = default(Link?);
        var skipNextSelfClosingMarkup = false;
        var writeNextAsRaw = false;

        while (tokenizer.MoveNext())
        {
            var token = tokenizer.Current;

            if (token.Kind == MarkupTokenKind.Open)
            {
                if (writeNextAsRaw)
                {
                    result.Add(new AnsiMarkupSegment($"[{token.Value}]", style.Value, null));
                    writeNextAsRaw = false;
                    continue;
                }

                if (token.Value.Equals("escnext", StringComparison.OrdinalIgnoreCase))
                {
                    if (skipNextSelfClosingMarkup)
                    {
                        result.Add(new AnsiMarkupSegment("[escnext]", style.Value, null));
                        skipNextSelfClosingMarkup = false;
                        continue;
                    }
                    skipNextSelfClosingMarkup = true;
                    continue;
                }

                if (token.Value.Equals("wnext", StringComparison.OrdinalIgnoreCase))
                {
                    writeNextAsRaw = true;
                    continue;
                }

                if (skipNextSelfClosingMarkup)
                {
                    skipNextSelfClosingMarkup = false;
                    continue;
                }

                if (token.Value.StartsWith("clear:", StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(AnsiMarkupSegment.Control(ParseClearDirective(token.Value)));
                    continue;
                }

                if (token.Value.StartsWith("move:", StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(AnsiMarkupSegment.Control(ParseMoveDirective(token.Value)));
                    continue;
                }

                var parsed = AnsiMarkupTagParser.Parse(token.Value);
                linkStack.Push(link);
                link = parsed.Link ?? link;
                styleStack.Push(style.Value);
                style = style.Value.Combine(parsed.Style);
            }
            else if (token.Kind == MarkupTokenKind.Close)
            {
                if (styleStack.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"Encountered closing tag when none was expected near position {token.Position}.");
                }

                style = styleStack.Pop();
                link = linkStack.Pop();
            }
            else if (token.Kind == MarkupTokenKind.Text)
            {
                if (result.Count > 0 && !result[^1].IsControlCode && result[^1].Style.Equals(style) && Equals(result[^1].Link, link))
                {
                    // Merge segments with same style and link
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

        if (styleStack.Count > 0)
        {
            throw new InvalidOperationException("Unbalanced markup stack. Did you forget to close a tag?");
        }

        return result;
    }

    private static string ParseClearDirective(string text)
    {
        if (!text.StartsWith("clear:", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Could not parse clear directive '{text}'.");
        }

        var value = text[6..].Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Could not parse clear directive '{text}'.");
        }

        var normalized = value.ToLowerInvariant();
        return normalized switch
        {
            "to eol" or "eol" => "\x1B[K",
            "line" or "entire line" or "entireline" => "\x1B[2K",
            "to eos" or "eos" => "\x1B[0J",
            "screen" or "entire screen" or "entirescreen" or "full screen" => "\x1B[H\x1B[2J",
            _ => ParseClearRowDirective(value),
        };
    }

    private static string ParseClearRowDirective(string value)
    {
        if (value.Length > 0 && value[0] == '#')
        {
            var rowText = value[1..].Trim();
            if (int.TryParse(rowText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var row) && row > 0)
            {
                return $"\x1B[{row};1H";
            }
        }

        throw new InvalidOperationException($"Could not parse clear directive 'clear:{value}'.");
    }

    private static string ParseMoveDirective(string text)
    {
        if (!text.StartsWith("move:", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Could not parse move directive '{text}'.");
        }

        var value = text[5..].Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Could not parse move directive '{text}'.");
        }

        var normalized = value.ToLowerInvariant();
        return normalized switch
        {
            "home" or "origin" => "\x1B[H",
            _ when normalized.StartsWith("to ") => ParseMoveToDirective(normalized[3..].Trim()),
            _ when normalized.StartsWith("up ") => ParseMoveRelativeDirective(normalized[3..].Trim(), 'A'),
            _ when normalized.StartsWith("down ") => ParseMoveRelativeDirective(normalized[5..].Trim(), 'B'),
            _ when normalized.StartsWith("left ") => ParseMoveRelativeDirective(normalized[5..].Trim(), 'D'),
            _ when normalized.StartsWith("right ") => ParseMoveRelativeDirective(normalized[6..].Trim(), 'C'),
            _ when normalized.StartsWith("forward ") => ParseMoveRelativeDirective(normalized[8..].Trim(), 'C'),
            _ when normalized.StartsWith("backward ") => ParseMoveRelativeDirective(normalized[9..].Trim(), 'D'),
            _ when normalized.Contains(';') => ParseMoveToDirective(normalized),
            _ when int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var row) && row > 0 => $"\x1B[{row};1H",
            _ => throw new InvalidOperationException($"Could not parse move directive 'move:{value}'."),
        };
    }

    private static string ParseMoveToDirective(string value)
    {
        if (value.Contains(';'))
        {
            var parts = value.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2 &&
                int.TryParse(parts[0].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var row) && row > 0 &&
                int.TryParse(parts[1].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var column) && column > 0)
            {
                return $"\x1B[{row};{column}H";
            }
        }

        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var singleRow) && singleRow > 0)
        {
            return $"\x1B[{singleRow};1H";
        }

        throw new InvalidOperationException($"Could not parse move directive 'move:{value}'.");
    }

    private static string ParseMoveRelativeDirective(string value, char direction)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return $"\x1B[1{direction}";
        }

        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var steps) && steps >= 0)
        {
            return $"\x1B[{steps}{direction}";
        }

        throw new InvalidOperationException($"Could not parse move directive 'move:{value}'.");
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
    /// Gets a value indicating whether the segment contains control codes.
    /// </summary>
    public bool IsControlCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiMarkupSegment"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="style">The style.</param>
    /// <param name="link">The link.</param>
    /// <param name="isControlCode">Whether this segment represents control codes.</param>
    public AnsiMarkupSegment(string text, Style style, Link? link, bool isControlCode = false)
    {
        Text = text;
        Style = style;
        Link = link;
        IsControlCode = isControlCode;
    }

    /// <summary>
    /// Creates a new segment for raw control codes.
    /// </summary>
    /// <param name="control">The control code text.</param>
    /// <returns>The control segment.</returns>
    public static AnsiMarkupSegment Control(string control)
    {
        return new AnsiMarkupSegment(control, Style.Plain, null, true);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var escaped = AnsiMarkup.Escape(Text);
        return !Style.Equals(Style.Plain)
            ? $"[{Style.ToMarkup()}]{escaped}[/]"
            : escaped;
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
        var closed = false;
        while (!_reader.Eof)
        {
            var current = _reader.Read();

            if (current == ']')
            {
                if (!currentStylePartCanContainMarkup || _reader.Peek() != ']')
                {
                    // Not parsing a link or not escaped. Markup closed
                    closed = true;
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

        if (!closed)
        {
            ThrowMalformed(_reader.Position);
        }

        return new MarkupToken(MarkupTokenKind.Open, builder.ToString(), position);
    }

    [DoesNotReturn]
    private static void ThrowMalformed(int pos) =>
        throw new InvalidOperationException($"Encountered malformed markup tag at position {pos}.");
}
