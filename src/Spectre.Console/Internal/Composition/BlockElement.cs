using System.Collections.Generic;

namespace Spectre.Console.Internal
{
    internal sealed class BlockElement : IConsoleElement
    {
        private readonly List<IConsoleElement> _elements;

        public IReadOnlyList<IConsoleElement> Elements => _elements;
        public int Width { get; private set; }

        public BlockElement()
        {
            _elements = new List<IConsoleElement>();
        }

        public void Append(IConsoleElement element)
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
