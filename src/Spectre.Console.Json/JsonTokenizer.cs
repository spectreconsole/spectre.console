using System.Text;

namespace Spectre.Console.Json;

internal static class JsonTokenizer
{
    private static readonly Dictionary<char, JsonTokenType> _typeLookup;
    private static readonly Dictionary<string, JsonTokenType> _keywords;
    private static readonly HashSet<char> _allowedEscapedChars;

    static JsonTokenizer()
    {
        _typeLookup = new Dictionary<char, JsonTokenType>
        {
            { '{', JsonTokenType.LeftBrace },
            { '}', JsonTokenType.RightBrace },
            { '[', JsonTokenType.LeftBracket },
            { ']', JsonTokenType.RightBracket },
            { ':', JsonTokenType.Colon },
            { ',', JsonTokenType.Comma },
        };

        _keywords = new Dictionary<string, JsonTokenType>
        {
            { "true", JsonTokenType.Boolean },
            { "false", JsonTokenType.Boolean },
            { "null", JsonTokenType.Null },
        };

        _allowedEscapedChars = new HashSet<char>
        {
            '\"', '\\', '/', 'b', 'f', 'n', 'r', 't', 'u',
        };
    }

    public static List<JsonToken> Tokenize(string text)
    {
        var result = new List<JsonToken>();
        var buffer = new StringBuffer(text);

        while (!buffer.Eof)
        {
            var current = buffer.Peek();

            if (_typeLookup.TryGetValue(current, out var tokenType))
            {
                buffer.Read(); // Consume
                result.Add(new JsonToken(tokenType, current.ToString()));
                continue;
            }
            else if (current == '\"')
            {
                result.Add(ReadString(buffer));
            }
            else if (current == '-' || current.IsDigit())
            {
                result.Add(ReadNumber(buffer));
            }
            else if (current is ' ' or '\n' or '\r' or '\t')
            {
                buffer.Read(); // Consume
            }
            else if (char.IsLetter(current))
            {
                var accumulator = new StringBuilder();
                while (!buffer.Eof)
                {
                    current = buffer.Peek();
                    if (!char.IsLetter(current))
                    {
                        break;
                    }

                    buffer.Read(); // Consume
                    accumulator.Append(current);
                }

                if (!_keywords.TryGetValue(accumulator.ToString(), out var keyword))
                {
                    throw new InvalidOperationException($"Encountered invalid keyword '{keyword}'");
                }

                result.Add(new JsonToken(keyword, accumulator.ToString()));
            }
            else
            {
                throw new InvalidOperationException("Invalid token");
            }
        }

        return result;
    }

    private static JsonToken ReadString(StringBuffer buffer)
    {
        var accumulator = new StringBuilder();
        accumulator.Append(buffer.Expect('\"'));

        while (!buffer.Eof)
        {
            var current = buffer.Peek();
            if (current == '\"')
            {
                break;
            }
            else if (current == '\\')
            {
                buffer.Expect('\\');

                if (buffer.Eof)
                {
                    break;
                }

                current = buffer.Read();
                if (!_allowedEscapedChars.Contains(current))
                {
                    throw new InvalidOperationException("Invalid escape encountered");
                }

                accumulator.Append('\\').Append(current);
            }
            else
            {
                accumulator.Append(current);
                buffer.Read();
            }
        }

        if (buffer.Eof)
        {
            throw new InvalidOperationException("Unterminated string literal");
        }

        accumulator.Append(buffer.Expect('\"'));
        return new JsonToken(JsonTokenType.String, accumulator.ToString());
    }

    private static JsonToken ReadNumber(StringBuffer buffer)
    {
        var accumulator = new StringBuilder();

        // Minus?
        if (buffer.Peek() == '-')
        {
            buffer.Read();
            accumulator.Append("-");
        }

        // Digits
        var current = buffer.Peek();
        if (current.IsDigit(min: 1))
        {
            ReadDigits(buffer, accumulator, min: 1);
        }
        else if (current == '0')
        {
            accumulator.Append(buffer.Expect('0'));
        }
        else
        {
            throw new InvalidOperationException("Invalid number");
        }

        // Fractions
        current = buffer.Peek();
        if (current == '.')
        {
            accumulator.Append(buffer.Expect('.'));
            ReadDigits(buffer, accumulator);
        }

        // Exponent
        current = buffer.Peek();
        if (current is 'e' or 'E')
        {
            accumulator.Append(buffer.Read());

            current = buffer.Peek();
            if (current is '+' or '-')
            {
                accumulator.Append(buffer.Read());
            }

            ReadDigits(buffer, accumulator);
        }

        return new JsonToken(JsonTokenType.Number, accumulator.ToString());
    }

    private static void ReadDigits(StringBuffer buffer, StringBuilder accumulator, int min = 0)
    {
        while (!buffer.Eof)
        {
            var current = buffer.Peek();
            if (!current.IsDigit(min))
            {
                break;
            }

            buffer.Read(); // Consume
            accumulator.Append(current);
        }
    }
}
