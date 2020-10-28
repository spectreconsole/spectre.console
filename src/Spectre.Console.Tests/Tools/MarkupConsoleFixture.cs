using System;
using System.IO;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Tests.Tools
{
    public sealed class MarkupConsoleFixture : IDisposable, IAnsiConsole
    {
        private readonly StringWriter _writer;
        private readonly IAnsiConsole _console;

        public string Output => _writer.ToString().TrimEnd('\n');

        public Capabilities Capabilities => _console.Capabilities;
        public Encoding Encoding => _console.Encoding;
        public IAnsiConsoleCursor Cursor => _console.Cursor;
        public int Width { get; }
        public int Height => _console.Height;

        public MarkupConsoleFixture(ColorSystem system, AnsiSupport ansi = AnsiSupport.Yes, int width = 80)
        {
            _writer = new StringWriter();
            _console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Ansi = ansi,
                ColorSystem = (ColorSystemSupport)system,
                Out = _writer,
                LinkIdentityGenerator = new TestLinkIdentityGenerator(),
            });

            Width = width;
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }

        public void Clear(bool home)
        {
            _console.Clear(home);
        }

        public void Write(Segment segment)
        {
            _console.Write(segment);
        }
    }
}
