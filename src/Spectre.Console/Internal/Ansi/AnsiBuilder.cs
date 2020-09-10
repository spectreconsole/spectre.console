using System;
using System.Linq;

namespace Spectre.Console.Internal
{
    internal static class AnsiBuilder
    {
        public static string GetAnsi(
            Capabilities capabilities,
            string text,
            Decoration decoration,
            Color foreground,
            Color background,
            string? link)
        {
            var codes = AnsiDecorationBuilder.GetAnsiCodes(decoration);

            // Got foreground?
            if (foreground != Color.Default)
            {
                codes = codes.Concat(
                    AnsiColorBuilder.GetAnsiCodes(
                        capabilities.ColorSystem,
                        foreground,
                        true));
            }

            // Got background?
            if (background != Color.Default)
            {
                codes = codes.Concat(
                    AnsiColorBuilder.GetAnsiCodes(
                        capabilities.ColorSystem,
                        background,
                        false));
            }

            var result = codes.ToArray();
            if (result.Length == 0 && link == null)
            {
                return text;
            }

            var ansiCodes = string.Join(";", result);
            var ansi = result.Length > 0
                ? $"\u001b[{ansiCodes}m{text}\u001b[0m"
                : text;

            if (link != null && !capabilities.LegacyConsole)
            {
                // Empty links means we should take the URL from the text.
                if (link.Equals(Constants.EmptyLink, StringComparison.Ordinal))
                {
                    link = text;
                }

                var linkId = Math.Abs(link.GetDeterministicHashCode());
                ansi = $"\u001b]8;id={linkId};{link}\u001b\\{ansi}\u001b]8;;\u001b\\";
            }

            return ansi;
        }
    }
}
