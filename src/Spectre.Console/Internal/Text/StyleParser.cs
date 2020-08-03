using System;

namespace Spectre.Console.Internal
{
    internal static class StyleParser
    {
        public static Style Parse(string text)
        {
            var style = Parse(text, out var error);
            if (error != null)
            {
                throw new InvalidOperationException(error);
            }

            return style;
        }

        public static bool TryParse(string text, out Style style)
        {
            style = Parse(text, out var error);
            if (error != null)
            {
                return false;
            }

            return true;
        }

        private static Style Parse(string text, out string error)
        {
            var effectiveDecoration = (Decoration?)null;
            var effectiveForeground = (Color?)null;
            var effectiveBackground = (Color?)null;

            var parts = text.Split(new[] { ' ' });
            var foreground = true;
            foreach (var part in parts)
            {
                if (part.Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (part.Equals("on", StringComparison.OrdinalIgnoreCase))
                {
                    foreground = false;
                    continue;
                }

                var decoration = DecorationTable.GetDecoration(part);
                if (decoration != null)
                {
                    if (effectiveDecoration == null)
                    {
                        effectiveDecoration = Decoration.None;
                    }

                    effectiveDecoration |= decoration.Value;
                }
                else
                {
                    var color = ColorTable.GetColor(part);
                    if (color == null)
                    {
                        if (!foreground)
                        {
                            error = $"Could not find color '{part}'.";
                        }
                        else
                        {
                            error = $"Could not find color or style '{part}'.";
                        }

                        return null;
                    }

                    if (foreground)
                    {
                        if (effectiveForeground != null)
                        {
                            error = "A foreground color has already been set.";
                            return null;
                        }

                        effectiveForeground = color;
                    }
                    else
                    {
                        if (effectiveBackground != null)
                        {
                            error = "A background color has already been set.";
                            return null;
                        }

                        effectiveBackground = color;
                    }
                }
            }

            error = null;
            return new Style(effectiveForeground, effectiveBackground, effectiveDecoration);
        }
    }
}
