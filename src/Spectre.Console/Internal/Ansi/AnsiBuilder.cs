using System;
using System.Linq;

namespace Spectre.Console.Internal
{
    internal sealed class AnsiBuilder
    {
        private readonly Capabilities _capabilities;
        private readonly ILinkIdentityGenerator _linkHasher;

        public AnsiBuilder(Capabilities capabilities, ILinkIdentityGenerator? linkHasher)
        {
            _capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
            _linkHasher = linkHasher ?? new LinkIdentityGenerator();
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
                        _capabilities.ColorSystem,
                        style.Foreground,
                        true));
            }

            // Got background?
            if (style.Background != Color.Default)
            {
                codes = codes.Concat(
                    AnsiColorBuilder.GetAnsiCodes(
                        _capabilities.ColorSystem,
                        style.Background,
                        false));
            }

            var result = codes.ToArray();
            if (result.Length == 0 && style.Link == null)
            {
                return text;
            }

            var ansiCodes = string.Join(";", result);
            var ansi = result.Length > 0
                ? $"\u001b[{ansiCodes}m{text}\u001b[0m"
                : text;

            if (style.Link != null && !_capabilities.LegacyConsole)
            {
                var link = style.Link;

                // Empty links means we should take the URL from the text.
                if (link.Equals(Constants.EmptyLink, StringComparison.Ordinal))
                {
                    link = text;
                }

                var linkId = _linkHasher.GenerateId(link, text);
                ansi = $"\u001b]8;id={linkId};{link}\u001b\\{ansi}\u001b]8;;\u001b\\";
            }

            return ansi;
        }
    }
}
