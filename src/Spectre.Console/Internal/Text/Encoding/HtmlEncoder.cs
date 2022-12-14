namespace Spectre.Console.Internal;

internal sealed class HtmlEncoder : IAnsiConsoleEncoder
{
    public string Encode(IAnsiConsole console, IEnumerable<IRenderable> renderables)
    {
        var context = RenderOptions.Create(console, new EncoderCapabilities(ColorSystem.TrueColor));
        var builder = new StringBuilder();

        builder.Append("<pre style=\"font-size:90%;font-family:consolas,'Courier New',monospace\">\n");

        foreach (var renderable in renderables)
        {
            var segments = renderable.Render(context, console.Profile.Width);
            foreach (var (_, first, _, segment) in segments.Enumerate())
            {
                if (segment.IsControlCode)
                {
                    continue;
                }

                if (segment.Text == "\n" && !first)
                {
                    builder.Append('\n');
                    continue;
                }

                var parts = segment.Text.Split(new[] { '\n' }, StringSplitOptions.None);
                foreach (var (_, _, last, line) in parts.Enumerate())
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    builder.Append("<span");
                    if (!segment.Style.Equals(Style.Plain))
                    {
                        builder.Append(" style=\"");
                        builder.Append(BuildCss(segment.Style));
                        builder.Append('"');
                    }

                    builder.Append('>');
                    builder.Append(line);
                    builder.Append("</span>");

                    if (parts.Length > 1 && !last)
                    {
                        builder.Append('\n');
                    }
                }
            }
        }

        builder.Append("</pre>");

        return builder.ToString().TrimEnd('\n');
    }

    private static string BuildCss(Style style)
    {
        var css = new List<string>();

        var foreground = style.Foreground;
        var background = style.Background;

        if ((style.Decoration & Decoration.Invert) != 0)
        {
            var temp = foreground;
            foreground = background;
            background = temp;
        }

        if ((style.Decoration & Decoration.Dim) != 0)
        {
            var blender = background;
            if (blender.Equals(Color.Default))
            {
                blender = Color.White;
            }

            foreground = foreground.Blend(blender, 0.5f);
        }

        if (!foreground.Equals(Color.Default))
        {
            css.Add($"color: #{foreground.ToHex()}");
        }

        if (!background.Equals(Color.Default))
        {
            css.Add($"background-color: #{background.ToHex()}");
        }

        if ((style.Decoration & Decoration.Bold) != 0)
        {
            css.Add("font-weight: bold");
        }

        if ((style.Decoration & Decoration.Bold) != 0)
        {
            css.Add("font-style: italic");
        }

        if ((style.Decoration & Decoration.Underline) != 0)
        {
            css.Add("text-decoration: underline");
        }

        if ((style.Decoration & Decoration.Strikethrough) != 0)
        {
            css.Add("text-decoration: line-through");
        }

        return string.Join(";", css);
    }
}