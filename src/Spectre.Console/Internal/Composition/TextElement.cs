using System;

namespace Spectre.Console.Internal
{
    internal sealed class TextElement : IConsoleElement
    {
        public string Text { get; }

        public int Width => Text.Length;

        public TextElement(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public void Render(IAnsiConsole renderer)
        {
            renderer.Write(Text);
        }
    }
}
