using System;
using System.Collections.Generic;

namespace Spectre.Console.Internal
{
    internal static class MarkupParser
    {
        public static Text Parse(string text, Appearance appearance = null)
        {
            appearance ??= Appearance.Plain;

            var result = new Text(string.Empty);
            using var tokenizer = new MarkupTokenizer(text);

            var stack = new Stack<Appearance>();

            while (tokenizer.MoveNext())
            {
                var token = tokenizer.Current;

                if (token.Kind == MarkupTokenKind.Open)
                {
                    var (style, foreground, background) = MarkupStyleParser.Parse(token.Value);
                    stack.Push(new Appearance(foreground, background, style));
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
                    var style = appearance.Combine(stack);
                    result.Append(token.Value, style);
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
