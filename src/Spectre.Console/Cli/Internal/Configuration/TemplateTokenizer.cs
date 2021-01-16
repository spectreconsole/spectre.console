using System.Collections.Generic;
using System.Text;

namespace Spectre.Console.Cli
{
    internal static class TemplateTokenizer
    {
        public static IReadOnlyList<TemplateToken> Tokenize(string template)
        {
            using var buffer = new TextBuffer(template);
            var result = new List<TemplateToken>();

            while (!buffer.ReachedEnd)
            {
                EatWhitespace(buffer);

                if (!buffer.TryPeek(out var character))
                {
                    break;
                }

                if (character == '-')
                {
                    result.Add(ReadOption(buffer));
                }
                else if (character == '|')
                {
                    buffer.Consume('|');
                }
                else if (character == '<')
                {
                    result.Add(ReadValue(buffer, true));
                }
                else if (character == '[')
                {
                    result.Add(ReadValue(buffer, false));
                }
                else
                {
                    throw CommandTemplateException.UnexpectedCharacter(buffer.Original, buffer.Position, character);
                }
            }

            return result;
        }

        private static void EatWhitespace(TextBuffer buffer)
        {
            while (!buffer.ReachedEnd)
            {
                var character = buffer.Peek();
                if (!char.IsWhiteSpace(character))
                {
                    break;
                }

                buffer.Read();
            }
        }

        private static TemplateToken ReadOption(TextBuffer buffer)
        {
            var position = buffer.Position;

            buffer.Consume('-');
            if (buffer.IsNext('-'))
            {
                buffer.Consume('-');
                var longValue = ReadOptionName(buffer);
                return new TemplateToken(TemplateToken.Kind.LongName, position, longValue, $"--{longValue}");
            }

            var shortValue = ReadOptionName(buffer);
            return new TemplateToken(TemplateToken.Kind.ShortName, position, shortValue, $"-{shortValue}");
        }

        private static string ReadOptionName(TextBuffer buffer)
        {
            var builder = new StringBuilder();
            while (!buffer.ReachedEnd)
            {
                var character = buffer.Peek();
                if (char.IsWhiteSpace(character) || character == '|')
                {
                    break;
                }

                builder.Append(buffer.Read());
            }

            return builder.ToString();
        }

        private static TemplateToken ReadValue(TextBuffer buffer, bool required)
        {
            var start = required ? '<' : '[';
            var end = required ? '>' : ']';

            var position = buffer.Position;
            var kind = required ? TemplateToken.Kind.RequiredValue : TemplateToken.Kind.OptionalValue;

            // Consume start of value character (< or [).
            buffer.Consume(start);

            var builder = new StringBuilder();
            while (!buffer.ReachedEnd)
            {
                var character = buffer.Peek();
                if (character == end)
                {
                    break;
                }

                buffer.Read();
                builder.Append(character);
            }

            if (buffer.ReachedEnd)
            {
                var name = builder.ToString();
                var token = new TemplateToken(kind, position, name, $"{start}{name}");
                throw CommandTemplateException.UnterminatedValueName(buffer.Original, token);
            }

            // Consume end of value character (> or ]).
            buffer.Consume(end);

            // Get the value (the text within the brackets).
            var value = builder.ToString();

            // Create a token and return it.
            return new TemplateToken(kind, position, value, required ? $"<{value}>" : $"[{value}]");
        }
    }
}