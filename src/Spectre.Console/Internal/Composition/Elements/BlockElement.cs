using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Internal
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Not used (yet)")]
    internal sealed class BlockElement : IRenderable
    {
        private readonly List<IRenderable> _elements;

        /// <inheritdoc/>
        public int Length { get; private set; }

        public IReadOnlyList<IRenderable> Elements => _elements;

        public BlockElement()
        {
            _elements = new List<IRenderable>();
        }

        public BlockElement Append(IRenderable element)
        {
            if (element != null)
            {
                _elements.Add(element);
                Length += element.Length;
            }

            return this;
        }

        /// <inheritdoc/>
        public void Render(IAnsiConsole renderer)
        {
            foreach (var element in _elements)
            {
                element.Render(renderer);
            }
        }
    }
}
