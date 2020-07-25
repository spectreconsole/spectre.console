using Spectre.Console.Internal;

namespace Spectre.Console.Internal
{
    internal sealed class TabElement : IRenderable
    {
        private readonly RepeatingElement _element;

        public int Width => _element.Width;

        public TabElement(int count = 1)
        {
            _element = new RepeatingElement(count * 4, new TextElement(" "));
        }

        public void Render(IAnsiConsole renderer)
        {
            _element.Render(renderer);
        }
    }
}
