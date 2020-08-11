using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console.Internal
{
    internal static class MarkupParser
    {
        public static Text Parse(string text, Style? style = null)
        {
            style ??= Style.Plain;

            var result = new Text(string.Empty);
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
                    var parsedStyle = StyleParser.Parse(token.Value);
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
                    // Get the effecive style.
                    var effectiveStyle = style.Combine(stack.Reverse());
                    result.Append(token.Value, effectiveStyle);
                }
                else
                {
                    throw new InvalidOperationException("Encountered unkown markup token.");
                }
            }

            if (stack.Count > 0)
            {
                throw new InvalidOperationException("Unbalanced markup stack. Did you forget to close a tag?");
            }

            return result;
        }
    }
}
