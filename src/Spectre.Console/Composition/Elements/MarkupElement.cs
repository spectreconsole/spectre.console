namespace Spectre.Console.Internal
{
    internal sealed class MarkupElement : IRenderable
    {
        private readonly IRenderable _inner;

        public int Width => _inner.Width;

        public MarkupElement(string markup)
        {
            _inner = MarkupParser.Parse(markup);
        }

        public void Render(IAnsiConsole renderer)
        {
            _inner.Render(renderer);
        }
    }
}
