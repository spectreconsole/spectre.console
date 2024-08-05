namespace Spectre.Console.Json;

/// <summary>
/// Contains extension methods for <see cref="JsonText"/>.
/// </summary>
public static class JsonTextExtensions
{
    /// <summary>
    /// Sets the style used for braces.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="style">The style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText BracesStyle(this JsonText text, Style? style)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.BracesStyle = style;
        return text;
    }

    /// <summary>
    /// Sets the style used for brackets.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="style">The style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText BracketStyle(this JsonText text, Style? style)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.BracketsStyle = style;
        return text;
    }

    /// <summary>
    /// Sets the style used for member names.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="style">The style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText MemberStyle(this JsonText text, Style? style)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.MemberStyle = style;
        return text;
    }

    /// <summary>
    /// Sets the style used for colons.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="style">The style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText ColonStyle(this JsonText text, Style? style)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.ColonStyle = style;
        return text;
    }

    /// <summary>
    /// Sets the style used for commas.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="style">The style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText CommaStyle(this JsonText text, Style? style)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.CommaStyle = style;
        return text;
    }

    /// <summary>
    /// Sets the style used for string literals.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="style">The style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText StringStyle(this JsonText text, Style? style)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.StringStyle = style;
        return text;
    }

    /// <summary>
    /// Sets the style used for number literals.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="style">The style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText NumberStyle(this JsonText text, Style? style)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.NumberStyle = style;
        return text;
    }

    /// <summary>
    /// Sets the style used for boolean literals.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="style">The style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText BooleanStyle(this JsonText text, Style? style)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.BooleanStyle = style;
        return text;
    }

    /// <summary>
    /// Sets the style used for <c>null</c> literals.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="style">The style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText NullStyle(this JsonText text, Style? style)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.NullStyle = style;
        return text;
    }

    /// <summary>
    /// Sets the color used for braces.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="color">The color to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText BracesColor(this JsonText text, Color color)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.BracesStyle = new Style(color);
        return text;
    }

    /// <summary>
    /// Sets the color used for brackets.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="color">The color to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText BracketColor(this JsonText text, Color color)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.BracketsStyle = new Style(color);
        return text;
    }

    /// <summary>
    /// Sets the color used for member names.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="color">The color to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText MemberColor(this JsonText text, Color color)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.MemberStyle = new Style(color);
        return text;
    }

    /// <summary>
    /// Sets the color used for colons.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="color">The color to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText ColonColor(this JsonText text, Color color)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.ColonStyle = new Style(color);
        return text;
    }

    /// <summary>
    /// Sets the color used for commas.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="color">The color to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText CommaColor(this JsonText text, Color color)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.CommaStyle = new Style(color);
        return text;
    }

    /// <summary>
    /// Sets the color used for string literals.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="color">The color to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText StringColor(this JsonText text, Color color)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.StringStyle = new Style(color);
        return text;
    }

    /// <summary>
    /// Sets the color used for number literals.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="color">The color to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText NumberColor(this JsonText text, Color color)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.NumberStyle = new Style(color);
        return text;
    }

    /// <summary>
    /// Sets the color used for boolean literals.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="color">The color to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText BooleanColor(this JsonText text, Color color)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.BooleanStyle = new Style(color);
        return text;
    }

    /// <summary>
    /// Sets the color used for <c>null</c> literals.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="color">The color to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText NullColor(this JsonText text, Color color)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.NullStyle = new Style(color);
        return text;
    }
}
