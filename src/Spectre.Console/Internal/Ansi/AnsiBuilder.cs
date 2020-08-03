using System.Linq;

namespace Spectre.Console.Internal
{
    internal static class AnsiBuilder
    {
        public static string GetAnsi(
            ColorSystem system,
            string text,
            Decoration decoration,
            Color foreground,
            Color background)
        {
            var codes = AnsiDecorationBuilder.GetAnsiCodes(decoration);

            // Got foreground?
            if (foreground != Color.Default)
            {
                codes = codes.Concat(AnsiColorBuilder.GetAnsiCodes(system, foreground, foreground: true));
            }

            // Got background?
            if (background != Color.Default)
            {
                codes = codes.Concat(AnsiColorBuilder.GetAnsiCodes(system, background, foreground: false));
            }

            var result = codes.ToArray();
            if (result.Length == 0)
            {
                return text;
            }

            var lol = string.Concat(
                "\u001b[",
                string.Join(";", result),
                "m",
                text,
                "\u001b[0m");

            return lol;
        }
    }
}
