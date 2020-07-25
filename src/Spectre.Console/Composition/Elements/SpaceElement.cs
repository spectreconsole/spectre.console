using Spectre.Console.Internal;

namespace Spectre.Console.Internal
{
    internal sealed class SpaceElement : IRenderable
    {
        private readonly RepeatingElement _element;

        public int Width => _element.Width;

        public SpaceElement(int count = 1)
        {
            _element = new RepeatingElement(count, new TextElement(" "));
        }

        public void Render(IAnsiConsole renderer)
        {
            _element.Render(renderer);
        }
    }
}
