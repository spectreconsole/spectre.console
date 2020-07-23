using System;

namespace Spectre.Console.Internal
{
    internal sealed class MarkupTextNode : IMarkupNode
    {
        public string Text { get; }

        public MarkupTextNode(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public void Render(IAnsiConsole renderer)
        {
            renderer.Write(Text);
        }
    }
}
