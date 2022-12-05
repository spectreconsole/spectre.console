namespace Spectre.Console.Cli;

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
            if (string.IsNullOrEmpty(arg))
            {
                // Null strings in the args array are still represented as tokens
                tokens.Add(new CommandTreeToken(CommandTreeToken.Kind.String, position, string.Empty, string.Empty));
                continue;
            }

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
        if (!reader.ReachedEnd && reader.Peek() == '-')
        {
            // Option
            tokens.AddRange(ScanOptions(context, reader));
        }
        else
        {
            // Command or argument
            while (reader.Peek() != -1)
            {
                if (reader.ReachedEnd)
                {
                    position += reader.Position - start;
                    break;
                }

                tokens.Add(ScanString(context, reader));

                // Flush remaining tokens
                context.FlushRemaining();
            }
        }

        return position;
    }

    private static CommandTreeToken ScanString(
        CommandTreeTokenizerContext context,
        TextBuffer reader,
        char[]? stop = null)
    {
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
        return new CommandTreeToken(CommandTreeToken.Kind.String, position, value, value);
    }

    private static IEnumerable<CommandTreeToken> ScanOptions(CommandTreeTokenizerContext context, TextBuffer reader)
    {
        var result = new List<CommandTreeToken>();

        var position = reader.Position;

        reader.Consume('-');
        context.AddRemaining('-');

        if (!reader.TryPeek(out var character) || character == ' ')
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

                var tokenValue = ScanString(context, reader);
                tokenValue.HadSeparator = true;
                result.Add(tokenValue);
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
            else if (result.Count == 0 && char.IsDigit(current))
            {
                // We require short options to be named with letters. Short options that start with a number
                // ("-1", "-2ab", "-3..7") may actually mean values (either for options or arguments) and will
                // be tokenized as strings. This block handles parsing those cases, but we only allow this
                // when the digit is the first character in the token (i.e. "-a1" is always an error), hence the
                // result.Count == 0 check above.
                string value = string.Empty;

                while (!reader.ReachedEnd)
                {
                    char c = reader.Peek();

                    if (char.IsWhiteSpace(c))
                    {
                        break;
                    }

                    value += c.ToString(CultureInfo.InvariantCulture);
                    reader.Read();
                }

                value = "-" + value; // Prefix with the minus sign that we originally thought to mean a short option
                result.Add(new CommandTreeToken(CommandTreeToken.Kind.String, position, value, value));
            }
            else
            {
                // Create a token representing the short option.
                var representation = current.ToString(CultureInfo.InvariantCulture);
                var tokenPosition = position + 1 + result.Count;
                var token = new CommandTreeToken(CommandTreeToken.Kind.ShortOption, tokenPosition, representation, representation);

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
        if (name.Value == " ")
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