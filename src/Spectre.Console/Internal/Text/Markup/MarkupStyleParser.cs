using System;

namespace Spectre.Console.Internal
{
    internal static class MarkupStyleParser
    {
        public static (Styles? Style, Color? Foreground, Color? Background) Parse(string text)
        {
            var effectiveStyle = (Styles?)null;
            var effectiveForeground = (Color?)null;
            var effectiveBackground = (Color?)null;

            var parts = text.Split(new[] { ' ' });
            var foreground = true;
            foreach (var part in parts)
            {
                if (part.Equals("on", StringComparison.OrdinalIgnoreCase))
                {
                    foreground = false;
                    continue;
                }

                var style = StyleTable.GetStyle(part);
                if (style != null)
                {
                    if (effectiveStyle == null)
                    {
                        effectiveStyle = Styles.None;
                    }

                    effectiveStyle |= style.Value;
                }
                else
                {
                    var color = ColorTable.GetColor(part);
                    if (color == null)
                    {
                        throw new InvalidOperationException("Could not find color..");
                    }

                    if (foreground)
                    {
                        if (effectiveForeground != null)
                        {
                            throw new InvalidOperationException("A foreground has already been set.");
                        }

                        effectiveForeground = color;
                    }
                    else
                    {
                        if (effectiveBackground != null)
                        {
                            throw new InvalidOperationException("A background has already been set.");
                        }

                        effectiveBackground = color;
                    }
                }
            }

            return (effectiveStyle, effectiveForeground, effectiveBackground);
        }
    }
}
