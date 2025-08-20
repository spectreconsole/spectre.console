namespace Spectre.Console.Json;

internal sealed class JsonBuilderContext
{
    public Paragraph Paragraph { get; }
    public int Indentation { get; set; }
    public JsonTextStyles Styling { get; }
    public int IndentWidth { get; }

    public JsonBuilderContext(JsonTextStyles styling, int indentWidth)
    {
        if (indentWidth < 2 || indentWidth > 4)
        {
            throw new ArgumentOutOfRangeException(nameof(indentWidth), indentWidth, "The value must be between 2 and 4 spaces (inclusive).");
        }

        Paragraph = new Paragraph();
        Styling = styling;
        IndentWidth = indentWidth;
    }

    public void InsertIndentation()
    {
        Paragraph.Append(new string(' ', Indentation * IndentWidth));
    }
}

internal sealed class JsonBuilder : JsonSyntaxVisitor<JsonBuilderContext>
{
    public static JsonBuilder Shared { get; } = new JsonBuilder();

    public override void VisitObject(JsonObject syntax, JsonBuilderContext context)
    {
        context.Paragraph.Append("{", context.Styling.BracesStyle);
        context.Paragraph.Append("\n");
        context.Indentation++;

        foreach (var (_, _, last, property) in syntax.Members.Enumerate())
        {
            context.InsertIndentation();
            property.Accept(this, context);

            if (!last)
            {
                context.Paragraph.Append(",", context.Styling.CommaStyle);
            }

            context.Paragraph.Append("\n");
        }

        context.Indentation--;
        context.InsertIndentation();
        context.Paragraph.Append("}", context.Styling.BracesStyle);
    }

    public override void VisitArray(JsonArray syntax, JsonBuilderContext context)
    {
        context.Paragraph.Append("[", context.Styling.BracketsStyle);
        context.Paragraph.Append("\n");
        context.Indentation++;

        foreach (var (_, _, last, item) in syntax.Items.Enumerate())
        {
            context.InsertIndentation();
            item.Accept(this, context);

            if (!last)
            {
                context.Paragraph.Append(",", context.Styling.CommaStyle);
            }

            context.Paragraph.Append("\n");
        }

        context.Indentation--;
        context.InsertIndentation();
        context.Paragraph.Append("]", context.Styling.BracketsStyle);
    }

    public override void VisitMember(JsonMember syntax, JsonBuilderContext context)
    {
        context.Paragraph.Append(syntax.Name, context.Styling.MemberStyle);
        context.Paragraph.Append(":", context.Styling.ColonStyle);
        context.Paragraph.Append(" ");

        syntax.Value.Accept(this, context);
    }

    public override void VisitNumber(JsonNumber syntax, JsonBuilderContext context)
    {
        context.Paragraph.Append(syntax.Lexeme, context.Styling.NumberStyle);
    }

    public override void VisitString(JsonString syntax, JsonBuilderContext context)
    {
        context.Paragraph.Append(syntax.Lexeme, context.Styling.StringStyle);
    }

    public override void VisitBoolean(JsonBoolean syntax, JsonBuilderContext context)
    {
        context.Paragraph.Append(syntax.Lexeme, context.Styling.BooleanStyle);
    }

    public override void VisitNull(JsonNull syntax, JsonBuilderContext context)
    {
        context.Paragraph.Append(syntax.Lexeme, context.Styling.NullStyle);
    }
}