namespace Spectre.Console.Internal
{
    internal sealed class RepeatingElement : IConsoleElement
    {
        private readonly int _repetitions;
        private readonly IConsoleElement _element;

        public int Width => _element.Width * _repetitions;

        public RepeatingElement(int repetitions, IConsoleElement element)
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
