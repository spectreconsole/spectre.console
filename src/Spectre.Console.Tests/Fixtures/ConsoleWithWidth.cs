using System.Text;

namespace Spectre.Console.Tests
{
    public sealed class ConsoleWithWidth : IAnsiConsole
    {
        private readonly IAnsiConsole _console;

        public Capabilities Capabilities => _console.Capabilities;

        public int Width { get; }
        public int Height => _console.Height;

        public Encoding Encoding => _console.Encoding;

        public ConsoleWithWidth(IAnsiConsole console, int width)
        {
            _console = console;
            Width = width;
        }

        public void Write(string text, Style style)
        {
            _console.Write(text, style);
        }
    }
}
