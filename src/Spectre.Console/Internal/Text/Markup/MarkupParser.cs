namespace Spectre.Console;

internal static class MarkupParser
{
    public static Paragraph Parse(string text, Style? style = null)
    {
        ArgumentNullException.ThrowIfNull(text);

        style ??= Style.Plain;

        var result = new Paragraph();
        using var tokenizer = new MarkupTokenizer(text);

        var stack = new Stack<Style>();

        while (tokenizer.MoveNext())
        {
            var token = tokenizer.Current;
            if (token == null)
            {
                break;
            }

            if (token.Kind == MarkupTokenKind.Open)
            {
                var parsedStyle = string.IsNullOrEmpty(token.Value) ? Style.Plain : StyleParser.Parse(token.Value);
                stack.Push(parsedStyle);
            }
            else if (token.Kind == MarkupTokenKind.Close)
            {
                if (stack.Count == 0)
                {
                    throw new InvalidOperationException($"Encountered closing tag when none was expected near position {token.Position}.");
                }

                stack.Pop();
            }
            else if (token.Kind == MarkupTokenKind.Text)
            {
                // Get the effective style.
                var effectiveStyle = style.Combine(stack.Reverse());
                result.Append(Emoji.Replace(token.Value), effectiveStyle);
            }
            else
            {
                throw new InvalidOperationException("Encountered unknown markup token.");
            }
        }

        if (stack.Count > 0)
        {
            throw new InvalidOperationException("Unbalanced markup stack. Did you forget to close a tag?");
        }

        return result;
    }
}