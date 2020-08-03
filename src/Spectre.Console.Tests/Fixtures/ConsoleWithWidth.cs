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

        public Decoration Decoration { get => _console.Decoration; set => _console.Decoration = value; }
        public Color Foreground { get => _console.Foreground; set => _console.Foreground = value; }
        public Color Background { get => _console.Background; set => _console.Background = value; }

        public ConsoleWithWidth(IAnsiConsole console, int width)
        {
            _console = console;
            Width = width;
        }

        public void Write(string text)
        {
            _console.Write(text);
        }
    }
}
