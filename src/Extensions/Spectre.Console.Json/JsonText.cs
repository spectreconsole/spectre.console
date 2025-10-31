namespace Spectre.Console.Json;

/// <summary>
/// A renderable piece of JSON text.
/// </summary>
public sealed class JsonText : JustInTimeRenderable
{
    private readonly string _json;
    private JsonSyntax? _syntax;
    private IJsonParser? _parser;
    private int _indentWidth;

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
    /// Gets or sets the width of indent used for rendering the JSON text.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The value must be between 2 and 4 spaces (inclusive).
    /// </exception>
    public int IndentWidth
    {
        get => _indentWidth;
        set
        {
            if (value < 2 || value > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(IndentWidth), value, "The value must be between 2 and 4 spaces (inclusive).");
            }

            _indentWidth = value;
        }
    }

    /// <summary>
    /// Gets or sets the JSON parser.
    /// </summary>
    public IJsonParser? Parser
    {
        get
        {
            return _parser;
        }
        set
        {
            _syntax = null;
            _parser = value;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonText"/> class.
    /// </summary>
    /// <param name="json">The JSON to render.</param>
    public JsonText(string json)
    {
        _json = json ?? throw new ArgumentNullException(nameof(json));
        _indentWidth = 3; // Use 3 spaces for indent by default.
    }

    /// <inheritdoc/>
    protected override IRenderable Build()
    {
        if (_syntax == null)
        {
            _syntax = (Parser ?? JsonParser.Shared).Parse(_json);
        }

        var jsonStyles = new JsonTextStyles
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
        };
        var context = new JsonBuilderContext(jsonStyles, IndentWidth);

        _syntax.Accept(JsonBuilder.Shared, context);
        return context.Paragraph;
    }
}
