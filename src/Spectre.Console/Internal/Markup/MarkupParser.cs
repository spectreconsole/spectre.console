using System;
using System.Collections.Generic;

namespace Spectre.Console.Internal
{
    internal static class MarkupParser
    {
        public static IRenderable Parse(string text)
        {
            using var tokenizer = new MarkupTokenizer(text);
            var root = new BlockElement();

            var stack = new Stack<BlockElement>();
            var current = root;

            while (true)
            {
                var token = tokenizer.GetNext();
                if (token == null)
                {
                    break;
                }

                if (token.Kind == MarkupTokenKind.Text)
                {
                    current.Append(new TextElement(token.Value));
                    continue;
                }
                else if (token.Kind == MarkupTokenKind.Open)
                {
                    var (style, foreground, background) = MarkupStyleParser.Parse(token.Value);
                    var content = new BlockElement();
                    current.Append(new StyleElement(style, foreground, background, content));

                    current = content;
                    stack.Push(current);

                    continue;
                }
                else if (token.Kind == MarkupTokenKind.Close)
                {
                    if (stack.Count == 0)
                    {
                        throw new InvalidOperationException($"Encountered closing tag when none was expected near position {token.Position}.");
                    }

                    stack.Pop();

                    if (stack.Count == 0)
                    {
                        current = root;
                    }
                    else
                    {
                        current = stack.Peek();
                    }

                    continue;
                }

                throw new InvalidOperationException("Encountered unkown markup token.");
            }

            if (stack.Count > 0)
            {
                throw new InvalidOperationException("Unbalanced markup stack. Did you forget to close a tag?");
            }

            return root;
        }
    }
}
