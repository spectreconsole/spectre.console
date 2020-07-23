using System.Collections.Generic;

namespace Spectre.Console.Internal
{
    internal sealed class MarkupBlockNode : IMarkupNode
    {
        private readonly List<IMarkupNode> _elements;

        public MarkupBlockNode()
        {
            _elements = new List<IMarkupNode>();
        }

        public void Append(IMarkupNode element)
        {
            if (element != null)
            {
                _elements.Add(element);
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
