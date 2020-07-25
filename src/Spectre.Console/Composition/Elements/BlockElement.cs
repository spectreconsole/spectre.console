using System.Collections.Generic;

namespace Spectre.Console.Internal
{
    internal sealed class BlockElement : IRenderable
    {
        private readonly List<IRenderable> _elements;

        public IReadOnlyList<IRenderable> Elements => _elements;
        public int Width { get; private set; }

        public BlockElement()
        {
            _elements = new List<IRenderable>();
        }

        public void Append(IRenderable element)
        {
            if (element != null)
            {
                _elements.Add(element);
                Width += element.Width;
            }
        }

        public void Render(IAnsiConsole renderer)
        {
            foreach (var element in _elements)
            {
                element.Render(renderer);
            }
        }
    }
}
