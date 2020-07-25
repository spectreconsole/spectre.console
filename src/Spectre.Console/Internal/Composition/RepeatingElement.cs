namespace Spectre.Console.Internal
{
    internal sealed class RepeatingElement : IRenderable
    {
        private readonly int _repetitions;
        private readonly IRenderable _element;

        public int Width => _element.Width * _repetitions;

        public RepeatingElement(int repetitions, IRenderable element)
        {
            _repetitions = repetitions;
            _element = element;
        }

        public void Render(IAnsiConsole renderer)
        {
            for (var index = 0; index < _repetitions; index++)
            {
                _element.Render(renderer);
            }
        }
    }
}
