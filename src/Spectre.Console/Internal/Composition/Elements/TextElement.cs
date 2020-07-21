using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Internal
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Not used (yet)")]
    internal sealed class TextElement : IRenderable
    {
        private readonly string _text;

        /// <inheritdoc/>
        public int Length => _text.Length;

        public TextElement(string text)
        {
            _text = text ?? throw new System.ArgumentNullException(nameof(text));
        }

        /// <inheritdoc/>
        public void Render(IAnsiConsole renderer)
        {
            renderer.Write(_text);
        }
    }
}
