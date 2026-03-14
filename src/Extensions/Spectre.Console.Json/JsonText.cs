namespace Spectre.Console.Json;

/// <summary>
/// A renderable piece of JSON text.
/// </summary>
public sealed class JsonText : JustInTimeRenderable
{
    private readonly string _json;
    private JsonSyntax? _syntax;

    /// <summary>
    /// Gets or sets the style used for braces.
    /// </summary>
    public Style? BracesStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for brackets.
    /// </summary>
    public Style? BracketsStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for member names.
    /// </summary>
    public Style? MemberStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for colons.
    /// </summary>
    public Style? ColonStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for commas.
    /// </summary>
    public Style? CommaStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for string literals.
    /// </summary>
    public Style? StringStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for number literals.
    /// </summary>
    public Style? NumberStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for boolean literals.
    /// </summary>
    public Style? BooleanStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for <c>null</c> literals.
    /// </summary>
    public Style? NullStyle { get; set; }

    /// <summary>
    /// Gets or sets the indentation.
    /// Defaults to three spaces.
    /// </summary>
    public string Indentation { get; set; } = "   ";

    /// <summary>
    /// Gets or sets the JSON parser.
    /// </summary>
    public IJsonParser? Parser
    {
        get;
        set
        {
            _syntax = null;
            field = value;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonText"/> class.
    /// </summary>
    /// <param name="json">The JSON to render.</param>
    public JsonText(string json)
    {
        _json = json ?? throw new ArgumentNullException(nameof(json));
    }

    /// <inheritdoc/>
    protected override IRenderable Build()
    {
        _syntax ??= (Parser ?? JsonParser.Shared).Parse(_json);

        var context = new JsonBuilderContext(
            new JsonTextStyles
            {
                BracesStyle = BracesStyle ?? Color.Grey,
                BracketsStyle = BracketsStyle ?? Color.Grey,
                MemberStyle = MemberStyle ?? Color.Blue,
                ColonStyle = ColonStyle ?? Color.Yellow,
                CommaStyle = CommaStyle ?? Color.Grey,
                StringStyle = StringStyle ?? Color.Red,
                NumberStyle = NumberStyle ?? Color.Green,
                BooleanStyle = BooleanStyle ?? Color.Green,
                NullStyle = NullStyle ?? Color.Grey,
            }, Indentation);

        _syntax.Accept(JsonBuilder.Shared, context);
        return context.Paragraph;
    }
}

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

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
        ArgumentNullException.ThrowIfNull(text);

        text.NullStyle = new Style(color);
        return text;
    }

    /// <summary>
    /// Sets the indentation.
    /// </summary>
    /// <param name="text">The JSON text instance.</param>
    /// <param name="indentation">The indentation.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static JsonText Indentation(this JsonText text, string indentation = "   ")
    {
        ArgumentNullException.ThrowIfNull(text);

        text.Indentation = indentation;
        return text;
    }
}