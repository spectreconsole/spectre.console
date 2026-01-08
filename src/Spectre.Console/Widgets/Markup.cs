namespace Spectre.Console;

/// <summary>
/// A renderable piece of markup text.
/// </summary>
[SuppressMessage("Naming", "CA1724:Type names should not match namespaces")]
public sealed class Markup : Renderable, IHasJustification, IOverflowable
{
    private readonly Paragraph _paragraph;

    /// <inheritdoc/>
    public Justify? Justification
    {
        get => _paragraph.Justification;
        set => _paragraph.Justification = value;
    }

    /// <inheritdoc/>
    public Overflow? Overflow
    {
        get => _paragraph.Overflow;
        set => _paragraph.Overflow = value;
    }

    /// <summary>
    /// Gets the character count.
    /// </summary>
    public int Length => _paragraph.Length;

    /// <summary>
    /// Gets the number of lines.
    /// </summary>
    public int Lines => _paragraph.Lines;

    /// <summary>
    /// Initializes a new instance of the <see cref="Markup"/> class.
    /// </summary>
    /// <param name="text">The markup text.</param>
    /// <param name="style">The style of the text.</param>
    public Markup(string text, Style? style = null)
    {
        _paragraph = MarkupParser.Parse(text, style);
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        return ((IRenderable)_paragraph).Measure(options, maxWidth);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        return ((IRenderable)_paragraph).Render(options, maxWidth);
    }

    /// <summary>
    /// Returns a new instance of a Markup widget from an interpolated string.
    /// </summary>
    /// <param name="value">The interpolated string value to write.</param>
    /// <param name="style">The style of the text.</param>
    /// <returns>A new markup instance.</returns>
    public static Markup FromInterpolated(FormattableString value, Style? style = null)
    {
        return FromInterpolated(CultureInfo.CurrentCulture, value, style);
    }

    /// <summary>
    /// Returns a new instance of a Markup widget from an interpolated string.
    /// </summary>
    /// <param name="provider">The format provider to use.</param>
    /// <param name="value">The interpolated string value to write.</param>
    /// <param name="style">The style of the text.</param>
    /// <returns>A new markup instance.</returns>
    public static Markup FromInterpolated(IFormatProvider provider, FormattableString value, Style? style = null)
    {
        return new Markup(EscapeInterpolated(provider, value), style);
    }

    /// <summary>
    /// Escapes text so that it won’t be interpreted as markup.
    /// </summary>
    /// <param name="text">The text to escape.</param>
    /// <returns>A string that is safe to use in markup.</returns>
    public static string Escape(string text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        return text.EscapeMarkup();
    }

    /// <summary>
    /// Removes markup from the specified string.
    /// </summary>
    /// <param name="text">The text to remove markup from.</param>
    /// <returns>A string that does not have any markup.</returns>
    public static string Remove(string text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        return text.RemoveMarkup();
    }

    internal static string EscapeInterpolated(IFormatProvider provider, FormattableString value)
    {
        object?[] args = value.GetArguments().Select(arg => arg is string s ? s.EscapeMarkup() : arg).ToArray();
        return string.Format(provider, value.Format, args);
    }
}