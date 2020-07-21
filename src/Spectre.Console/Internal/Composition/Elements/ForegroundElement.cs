using System;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Internal
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Not used (yet)")]
    internal sealed class ForegroundElement : IRenderable
    {
        private readonly Color _color;
        private readonly IRenderable _element;

        /// <inheritdoc/>
        public int Length => _element.Length;

        public ForegroundElement(Color color, IRenderable element)
        {
            _color = color;
            _element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <inheritdoc/>
        public void Render(IAnsiConsole renderer)
        {
            if (renderer is null)
            {
                throw new ArgumentNullException(nameof(renderer));
            }

            using (renderer.PushColor(_color, foreground: true))
            {
                _element.Render(renderer);
            }
        }
    }
}
