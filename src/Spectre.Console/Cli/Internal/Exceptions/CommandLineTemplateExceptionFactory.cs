using Spectre.Console.Rendering;

namespace Spectre.Console.Cli
{
    internal static class CommandLineTemplateExceptionFactory
    {
        internal static CommandTemplateException Create(string template, TemplateToken? token, string message, string details)
        {
            return new CommandTemplateException(message, template, CreatePrettyMessage(template, token, message, details));
        }

        private static IRenderable CreatePrettyMessage(string template, TemplateToken? token, string message, string details)
        {
            var composer = new Composer();

            var position = token?.Position ?? 0;
            var value = token?.Representation ?? template;

            // Header
            composer.LineBreak();
            composer.Style("red", "Error:");
            composer.Space().Text("An error occured when parsing template.");
            composer.LineBreak();
            composer.Spaces(7).Style("yellow", message.EscapeMarkup());
            composer.LineBreak();

            if (string.IsNullOrWhiteSpace(template))
            {
                // Error
                composer.LineBreak();
                composer.Style("red", message.EscapeMarkup());
                composer.LineBreak();
            }
            else
            {
                // Template
                composer.LineBreak();
                composer.Spaces(7).Text(template.EscapeMarkup());

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
            }

            composer.LineBreak();

            return composer;
        }
    }
}
