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
            return error == null;
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
                        if (part.StartsWith("#", StringComparison.OrdinalIgnoreCase))
                        {
                            color = ParseHexColor(part, out error);
                            if (!string.IsNullOrWhiteSpace(error))
                            {
                                return null;
                            }
                        }
                        else
                        {
                            error = !foreground
                                ? $"Could not find color '{part}'."
                                : $"Could not find color or style '{part}'.";

                            return null;
                        }
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

        private static Color? ParseHexColor(string hex, out string error)
        {
            error = null;

            hex = hex ?? string.Empty;
            hex = hex.Replace("#", string.Empty).Trim();

            try
            {
                if (!string.IsNullOrWhiteSpace(hex))
                {
                    if (hex.Length == 6)
                    {
                        return new Color(
                            (byte)Convert.ToUInt32(hex.Substring(0, 2), 16),
                            (byte)Convert.ToUInt32(hex.Substring(2, 2), 16),
                            (byte)Convert.ToUInt32(hex.Substring(4, 2), 16));
                    }
                    else if (hex.Length == 3)
                    {
                        return new Color(
                            (byte)Convert.ToUInt32(new string(hex[0], 2), 16),
                            (byte)Convert.ToUInt32(new string(hex[1], 2), 16),
                            (byte)Convert.ToUInt32(new string(hex[2], 2), 16));
                    }
                }
            }
            catch (Exception ex)
            {
                error = $"Invalid hex color '#{hex}'. {ex.Message}";
                return null;
            }

            error = $"Invalid hex color '#{hex}'.";
            return null;
        }
    }
}
