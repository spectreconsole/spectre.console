using System;
using System.Linq;
using static Spectre.Console.AnsiSequences;

namespace Spectre.Console
{
    internal sealed class AnsiBuilder
    {
        private readonly Profile _profile;
        private readonly AnsiLinkHasher _linkHasher;

        public AnsiBuilder(Profile profile)
        {
            _profile = profile ?? throw new ArgumentNullException(nameof(profile));
            _linkHasher = new AnsiLinkHasher();
        }

        public string GetAnsi(string text, Style style)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            var codes = AnsiDecorationBuilder.GetAnsiCodes(style.Decoration);

            // Got foreground?
            if (style.Foreground != Color.Default)
            {
                codes = codes.Concat(
                    AnsiColorBuilder.GetAnsiCodes(
                        _profile.ColorSystem,
                        style.Foreground,
                        true));
            }

            // Got background?
            if (style.Background != Color.Default)
            {
                codes = codes.Concat(
                    AnsiColorBuilder.GetAnsiCodes(
                        _profile.ColorSystem,
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

            if (style.Link != null && !_profile.Capabilities.Legacy)
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
