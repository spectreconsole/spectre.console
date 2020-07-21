using System;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Internal
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Not used (yet)")]
    internal sealed class StyleElement : IRenderable
    {
        private readonly Styles _style;
        private readonly IRenderable _element;

        /// <inheritdoc/>
        public int Length => _element.Length;

        public StyleElement(Styles style, IRenderable element)
        {
            _style = style;
            _element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <inheritdoc/>
        public void Render(IAnsiConsole renderer)
        {
            if (renderer is null)
            {
                throw new ArgumentNullException(nameof(renderer));
            }

            using (renderer.PushStyle(_style))
            {
                _element.Render(renderer);
            }
        }
    }
}
