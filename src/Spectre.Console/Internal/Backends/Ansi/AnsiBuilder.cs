using System;
using System.Linq;
using System.Text;
using Spectre.Console.Rendering;
using static Spectre.Console.AnsiSequences;

namespace Spectre.Console
{
    internal static class AnsiBuilder
    {
        private static readonly AnsiLinkHasher _linkHasher;

        static AnsiBuilder()
        {
            _linkHasher = new AnsiLinkHasher();
        }

        public static string Build(IAnsiConsole console, IRenderable renderable)
        {
            var builder = new StringBuilder();
            foreach (var segment in renderable.GetSegments(console))
            {
                if (segment.IsControlCode)
                {
                    builder.Append(segment.Text);
                    continue;
                }

                var parts = segment.Text.NormalizeNewLines().Split(new[] { '\n' });
                foreach (var (_, _, last, part) in parts.Enumerate())
                {
                    if (!string.IsNullOrEmpty(part))
                    {
                        builder.Append(Build(console.Profile, part, segment.Style));
                    }

                    if (!last)
                    {
                        builder.Append(Environment.NewLine);
                    }
                }
            }

            return builder.ToString();
        }

        private static string Build(Profile profile, string text, Style style)
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }
            else if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            else if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            var codes = AnsiDecorationBuilder.GetAnsiCodes(style.Decoration);

            // Got foreground?
            if (style.Foreground != Color.Default)
            {
                codes = codes.Concat(
                    AnsiColorBuilder.GetAnsiCodes(
                        profile.Capabilities.ColorSystem,
                        style.Foreground,
                        true));
            }

            // Got background?
            if (style.Background != Color.Default)
            {
                codes = codes.Concat(
                    AnsiColorBuilder.GetAnsiCodes(
                        profile.Capabilities.ColorSystem,
                        style.Background,
                        false));
            }

            var result = codes.ToArray();
            if (result.Length == 0 && style.Link == null)
            {
                return text;
            }

            var ansi = result.Length > 0
                ? $"{SGR(result)}{text}{SGR(0)}"
                : text;

            if (style.Link != null && !profile.Capabilities.Legacy)
            {
                var link = style.Link;

                // Empty links means we should take the URL from the text.
                if (link.Equals(Constants.EmptyLink, StringComparison.Ordinal))
                {
                    link = text;
                }

                var linkId = _linkHasher.GenerateId(link, text);
                ansi = $"{ESC}]8;id={linkId};{link}{ESC}\\{ansi}{ESC}]8;;{ESC}\\";
            }

            return ansi;
        }
    }
}
