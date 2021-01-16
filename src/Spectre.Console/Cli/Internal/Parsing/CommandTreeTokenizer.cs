using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Spectre.Console.Cli
{
    internal static class CommandTreeTokenizer
    {
        public enum Mode
        {
            Normal = 0,
            Remaining = 1,
        }

        // Consider removing this in favor for value tuples at some point.
        public sealed class CommandTreeTokenizerResult
        {
            public CommandTreeTokenStream Tokens { get; }
            public IReadOnlyList<string> Remaining { get; }

            public CommandTreeTokenizerResult(CommandTreeTokenStream tokens, IReadOnlyList<string> remaining)
            {
                Tokens = tokens;
                Remaining = remaining;
            }
        }

        public static CommandTreeTokenizerResult Tokenize(IEnumerable<string> args)
        {
            var tokens = new List<CommandTreeToken>();
            var position = 0;
            var previousReader = default(TextBuffer);
            var context = new CommandTreeTokenizerContext();

            foreach (var arg in args)
            {
                var start = position;
                var reader = new TextBuffer(previousReader, arg);

                // Parse the token.
                position = ParseToken(context, reader, position, start, tokens);
                context.FlushRemaining();

                previousReader = reader;
            }

            previousReader?.Dispose();

            return new CommandTreeTokenizerResult(
                new CommandTreeTokenStream(tokens),
                context.Remaining);
        }

        private static int ParseToken(CommandTreeTokenizerContext context, TextBuffer reader, int position, int start, List<CommandTreeToken> tokens)
        {
            while (reader.Peek() != -1)
            {
                if (reader.ReachedEnd)
                {
                    position += reader.Position - start;
                    break;
                }

                var character = reader.Peek();

                // Eat whitespace
                if (char.IsWhiteSpace(character))
                {
                    reader.Consume();
                    continue;
                }

                if (character == '-')
                {
                    // Option
                    tokens.AddRange(ScanOptions(context, reader));
                }
                else
                {
                    // Command or argument
                    tokens.Add(ScanString(context, reader));
                }

                // Flush remaining tokens
                context.FlushRemaining();
            }

            return position;
        }

        private static CommandTreeToken ScanString(
            CommandTreeTokenizerContext context,
            TextBuffer reader,
            char[]? stop = null)
        {
            if (reader.TryPeek(out var character))
            {
                // Is this a quoted string?
                if (character == '\"')
                {
                    return ScanQuotedString(context, reader);
                }
            }

            var position = reader.Position;
            var builder = new StringBuilder();
            while (!reader.ReachedEnd)
            {
                var current = reader.Peek();
                if (stop?.Contains(current) ?? false)
                {
                    break;
                }

                reader.Read(); // Consume
                context.AddRemaining(current);
                builder.Append(current);
            }

            var value = builder.ToString();
            return new CommandTreeToken(CommandTreeToken.Kind.String, position, value.Trim(), value);
        }

        private static CommandTreeToken ScanQuotedString(CommandTreeTokenizerContext context, TextBuffer reader)
        {
            var position = reader.Position;

            context.FlushRemaining();
            reader.Consume('\"');

            var builder = new StringBuilder();
            var terminated = false;
            while (!reader.ReachedEnd)
            {
                var character = reader.Peek();
                if (character == '\"')
                {
                    terminated = true;
                    reader.Read();
                    break;
                }

                builder.Append(reader.Read());
            }

            if (!terminated)
            {
                var unterminatedQuote = builder.ToString();
                var token = new CommandTreeToken(CommandTreeToken.Kind.String, position, unterminatedQuote, $"\"{unterminatedQuote}");
                throw CommandParseException.UnterminatedQuote(reader.Original, token);
            }

            var quotedString = builder.ToString();

            // Add to the context
            context.AddRemaining(quotedString);

            return new CommandTreeToken(
                CommandTreeToken.Kind.String,
                position, quotedString,
                quotedString);
        }

        private static IEnumerable<CommandTreeToken> ScanOptions(CommandTreeTokenizerContext context, TextBuffer reader)
        {
            var result = new List<CommandTreeToken>();

            var position = reader.Position;

            reader.Consume('-');
            context.AddRemaining('-');

            if (!reader.TryPeek(out var character))
            {
                var token = new CommandTreeToken(CommandTreeToken.Kind.ShortOption, position, "-", "-");
                throw CommandParseException.OptionHasNoName(reader.Original, token);
            }

            switch (character)
            {
                case '-':
                    var option = ScanLongOption(context, reader, position);
                    if (option != null)
                    {
                        result.Add(option);
                    }

                    break;
                default:
                    result.AddRange(ScanShortOptions(context, reader, position));
                    break;
            }

            if (reader.TryPeek(out character))
            {
                // Encountered a separator?
                if (character == '=' || character == ':')
                {
                    reader.Read(); // Consume
                    context.AddRemaining(character);

                    if (!reader.TryPeek(out _))
                    {
                        var token = new CommandTreeToken(CommandTreeToken.Kind.String, reader.Position, "=", "=");
                        throw CommandParseException.OptionValueWasExpected(reader.Original, token);
                    }

                    result.Add(ScanString(context, reader));
                }
            }

            return result;
        }

        private static IEnumerable<CommandTreeToken> ScanShortOptions(CommandTreeTokenizerContext context, TextBuffer reader, int position)
        {
            var result = new List<CommandTreeToken>();
            while (!reader.ReachedEnd)
            {
                var current = reader.Peek();
                if (char.IsWhiteSpace(current))
                {
                    break;
                }

                // Encountered a separator?
                if (current == '=' || current == ':')
                {
                    break;
                }

                if (char.IsLetter(current))
                {
                    context.AddRemaining(current);
                    reader.Read(); // Consume

                    var value = current.ToString(CultureInfo.InvariantCulture);
                    result.Add(result.Count == 0
                        ? new CommandTreeToken(CommandTreeToken.Kind.ShortOption, position, value, $"-{value}")
                        : new CommandTreeToken(CommandTreeToken.Kind.ShortOption, position + result.Count, value, value));
                }
                else
                {
                    // Create a token representing the short option.
                    var tokenPosition = position + 1 + result.Count;
                    var represntation = current.ToString(CultureInfo.InvariantCulture);
                    var token = new CommandTreeToken(CommandTreeToken.Kind.ShortOption, tokenPosition, represntation, represntation);
                    throw CommandParseException.InvalidShortOptionName(reader.Original, token);
                }
            }

            if (result.Count > 1)
            {
                foreach (var item in result)
                {
                    item.IsGrouped = true;
                }
            }

            return result;
        }

        private static CommandTreeToken ScanLongOption(CommandTreeTokenizerContext context, TextBuffer reader, int position)
        {
            reader.Consume('-');
            context.AddRemaining('-');

            if (reader.ReachedEnd)
            {
                // Rest of the arguments are remaining ones.
                context.Mode = Mode.Remaining;
                return new CommandTreeToken(CommandTreeToken.Kind.Remaining, position, "--", "--");
            }

            var name = ScanString(context, reader, new[] { '=', ':' });

            // Perform validation of the name.
            if (name.Value.Length == 0)
            {
                throw CommandParseException.LongOptionNameIsMissing(reader, position);
            }

            if (name.Value.Length == 1)
            {
                throw CommandParseException.LongOptionNameIsOneCharacter(reader, position, name.Value);
            }

            if (char.IsDigit(name.Value[0]))
            {
                throw CommandParseException.LongOptionNameStartWithDigit(reader, position, name.Value);
            }

            for (var index = 0; index < name.Value.Length; index++)
            {
                if (!char.IsLetterOrDigit(name.Value[index]) && name.Value[index] != '-' && name.Value[index] != '_')
                {
                    throw CommandParseException.LongOptionNameContainSymbol(reader, position + 2 + index, name.Value[index]);
                }
            }

            return new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name.Value, $"--{name.Value}");
        }
    }
}
