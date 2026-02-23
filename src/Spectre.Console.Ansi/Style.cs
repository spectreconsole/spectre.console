namespace Spectre.Console;

/// <summary>
/// Represents color and text decoration.
/// </summary>
public readonly record struct Style
{
    /// <summary>
    /// Gets the foreground color.
    /// </summary>
    public Color Foreground { get; init; }

    /// <summary>
    /// Gets the background color.
    /// </summary>
    public Color Background { get; init; }

    /// <summary>
    /// Gets the text decoration.
    /// </summary>
    public Decoration Decoration { get; init; }

    /// <summary>
    /// Gets a <see cref="Style"/> with the
    /// default colors and without text decoration.
    /// </summary>
    public static Style Plain { get; } = new Style(null, null, null);

    /// <summary>
    /// Initializes a new instance of the <see cref="Style"/> class.
    /// </summary>
    public Style()
    {
        Foreground = Color.Default;
        Background = Color.Default;
        Decoration = Decoration.None;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Style"/> class.
    /// </summary>
    /// <param name="foreground">The foreground color.</param>
    /// <param name="background">The background color.</param>
    /// <param name="decoration">The text decoration.</param>
    public Style(Color? foreground = null, Color? background = null, Decoration? decoration = null)
    {
        Foreground = foreground ?? Color.Default;
        Background = background ?? Color.Default;
        Decoration = decoration ?? Decoration.None;
    }

    /// <summary>
    /// Combines this style with another one.
    /// </summary>
    /// <param name="other">The item to combine with this.</param>
    /// <returns>A new style representing a combination of this and the other one.</returns>
    public Style Combine(Style other)
    {
        var foreground = Foreground;
        if (!other.Foreground.IsDefault)
        {
            foreground = other.Foreground;
        }

        var background = Background;
        if (!other.Background.IsDefault)
        {
            background = other.Background;
        }

        return new Style(foreground, background, Decoration | other.Decoration);
    }

    /// <summary>
    /// Implicitly converts <see cref="string"/> into a <see cref="Style"/>.
    /// </summary>
    /// <param name="style">The style string.</param>
    public static implicit operator Style(string style)
    {
        return Parse(style);
    }

    /// <summary>
    /// Implicitly converts <see cref="Color"/> into a <see cref="Style"/> with a foreground color.
    /// </summary>
    /// <param name="color">The foreground color.</param>
    public static implicit operator Style(Color color)
    {
        return new Style(foreground: color);
    }

    /// <summary>
    /// Converts the string representation of a style to its <see cref="Style"/> equivalent.
    /// </summary>
    /// <param name="text">A string containing a style to parse.</param>
    /// <returns>A <see cref="Style"/> equivalent of the text contained in <paramref name="text"/>.</returns>
    public static Style Parse(string text)
    {
        var result = AnsiMarkupTagParser.Parse(text);
        return result.Style;
    }

    /// <summary>
    /// Converts the string representation of a style to its <see cref="Style"/> equivalent.
    /// A return value indicates whether the operation succeeded.
    /// </summary>
    /// <param name="text">A string containing a style to parse.</param>
    /// <param name="result">
    /// When this method returns, contains the <see cref="Style"/> equivalent of the text contained in <paramref name="text"/>,
    /// if the conversion succeeded, or <c>null</c> if the conversion failed.
    /// </param>
    /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
    public static bool TryParse(string text, out Style result)
    {
        if (AnsiMarkupTagParser.TryParse(text, out var parsed))
        {
            result = parsed.Value.Style;
            return true;
        }

        result = Plain;
        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = (int)2166136261;
            hash = (hash * 16777619) ^ Foreground.GetHashCode();
            hash = (hash * 16777619) ^ Background.GetHashCode();
            hash = (hash * 16777619) ^ Decoration.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Returns the markup representation of this style.
    /// </summary>
    /// <returns>The markup representation of this style.</returns>
    public string ToMarkup()
    {
        var builder = new List<string>();

        if (Decoration != Decoration.None)
        {
            var result = DecorationTable.GetMarkupNames(Decoration);
            if (result.Count != 0)
            {
                builder.AddRange(result);
            }
        }

        if (Foreground != Color.Default)
        {
            builder.Add(Foreground.ToMarkup());
        }

        if (Background != Color.Default)
        {
            if (builder.Count == 0)
            {
                builder.Add("default");
            }

            builder.Add("on " + Background.ToMarkup());
        }

        return string.Join(" ", builder);
    }
}

/// <summary>
/// Contains extension methods for <see cref="Style"/>.
/// </summary>
public static class StyleExtensions
{
    /// <summary>
    /// Creates a new style from the specified one with
    /// the specified foreground color.
    /// </summary>
    /// <param name="style">The style.</param>
    /// <param name="color">The foreground color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Style Foreground(this Style style, Color color)
    {
        return new Style(
            foreground: color,
            background: style.Background,
            decoration: style.Decoration);
    }

    /// <summary>
    /// Creates a new style from the specified one with
    /// the specified background color.
    /// </summary>
    /// <param name="style">The style.</param>
    /// <param name="color">The background color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Style Background(this Style style, Color color)
    {
        return new Style(
            foreground: style.Foreground,
            background: color,
            decoration: style.Decoration);
    }

    /// <summary>
    /// Creates a new style from the specified one with
    /// the specified text decoration.
    /// </summary>
    /// <param name="style">The style.</param>
    /// <param name="decoration">The text decoration.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Style Decoration(this Style style, Decoration decoration)
    {
        return new Style(
            foreground: style.Foreground,
            background: style.Background,
            decoration: decoration);
    }

    internal static Style Combine(this Style? style, IEnumerable<Style> source)
    {
        var current = style ?? Style.Plain;
        foreach (var item in source)
        {
            current = current.Combine(item);
        }

        return current;
    }
}