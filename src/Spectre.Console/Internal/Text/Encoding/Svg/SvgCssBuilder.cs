namespace Spectre.Console.Internal;

internal static class SvgCssBuilder
{
    public static string BuildCss(SvgTheme theme, Style style)
    {
        var css = new List<string>();

        var foreground = theme.GetColor(style.Foreground);
        var background = theme.GetColor(style.Background);

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
