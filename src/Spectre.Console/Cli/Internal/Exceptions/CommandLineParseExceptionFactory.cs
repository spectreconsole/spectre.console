using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli
{
    internal static class CommandLineParseExceptionFactory
    {
        internal static CommandParseException Create(string arguments, CommandTreeToken token, string message, string details)
        {
            return new CommandParseException(message, CreatePrettyMessage(arguments, token, message, details));
        }

        internal static CommandParseException Create(IEnumerable<string> arguments, CommandTreeToken token, string message, string details)
        {
            return new CommandParseException(message, CreatePrettyMessage(string.Join(" ", arguments), token, message, details));
        }

        private static IRenderable CreatePrettyMessage(string arguments, CommandTreeToken token, string message, string details)
        {
            var composer = new Composer();

            var position = token?.Position ?? 0;
            var value = token?.Representation ?? arguments;

            // Header
            composer.LineBreak();
            composer.Style("red", "Error:");
            composer.Space().Text(message.EscapeMarkup());
            composer.LineBreak();

            // Template
            composer.LineBreak();
            composer.Spaces(7).Text(arguments.EscapeMarkup());

            // Error
            composer.LineBreak();
            composer.Spaces(7).Spaces(position);

            composer.Style("red", error =>
            {
                error.Repeat('^', value.Length);
                error.Space();
                error.Text(details.TrimEnd('.').EscapeMarkup());
                error.LineBreak();
            });

            composer.LineBreak();

            return composer;
        }
    }
}
