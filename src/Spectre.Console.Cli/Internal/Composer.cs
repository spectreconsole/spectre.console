namespace Spectre.Console.Cli;

internal sealed class Composer : IRenderable
{
    private readonly StringBuilder _content;

    /// <summary>
    /// Whether to emit the markup styles, inline, when rendering the content.
    /// </summary>
    private readonly bool _renderMarkup = false;

    public Composer()
    {
        _content = new StringBuilder();
    }

    public Composer(bool renderMarkup)
        : this()
    {
        _renderMarkup = renderMarkup;
    }

    public Composer Text(string text)
    {
        _content.Append(text);
        return this;
    }

    public Composer Style(Style style, string text)
    {
        _content.Append('[').Append(style.ToMarkup()).Append(']');
        _content.Append(text.EscapeMarkup());
        _content.Append("[/]");

        return this;
    }

    public Composer Style(string style, string text)
    {
        _content.Append('[').Append(style).Append(']');
        _content.Append(text.EscapeMarkup());
        _content.Append("[/]");

        return this;
    }

    public Composer Style(string style, Action<Composer> action)
    {
        _content.Append('[').Append(style).Append(']');
        action(this);
        _content.Append("[/]");

        return this;
    }

    public Composer Space()
    {
        return Spaces(1);
    }

    public Composer Spaces(int count)
    {
        return Repeat(' ', count);
    }

    public Composer Tab()
    {
        return Tabs(1);
    }

    public Composer Tabs(int count)
    {
        return Spaces(count * 4);
    }

    public Composer Repeat(char character, int count)
    {
        _content.Append(new string(character, count));
        return this;
    }

    public Composer LineBreak()
    {
        return LineBreaks(1);
    }

    public Composer LineBreaks(int count)
    {
        for (var i = 0; i < count; i++)
        {
            _content.Append(Environment.NewLine);
        }

        return this;
    }

    public Composer Join(string separator, IEnumerable<Composer> composers)
    {
        if (composers != null)
        {
            foreach (var composer in composers)
            {
                if (_content.ToString().Length > 0)
                {
                    Text(separator);
                }

                Text(composer.ToString());
            }
        }

        return this;
    }

    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        if (_renderMarkup)
        {
            return ((IRenderable)new Paragraph(_content.ToString())).Measure(options, maxWidth);
        }
        else
        {
            return ((IRenderable)new Markup(_content.ToString())).Measure(options, maxWidth);
        }
    }

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        if (_renderMarkup)
        {
            return ((IRenderable)new Paragraph(_content.ToString())).Render(options, maxWidth);
        }
        else
        {
            return ((IRenderable)new Markup(_content.ToString())).Render(options, maxWidth);
        }
    }

    public override string ToString()
    {
        return _content.ToString();
    }
}