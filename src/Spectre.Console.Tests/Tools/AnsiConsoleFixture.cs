using System;
using System.IO;
using System.Text;
using Spectre.Console.Rendering;
using Spectre.Console.Tests.Tools;

namespace Spectre.Console.Tests
{
    public sealed class TestableAnsiConsole : IDisposable, IAnsiConsole
    {
        private readonly StringWriter _writer;
        private readonly IAnsiConsole _console;

        public string Output => _writer.ToString();

        public Capabilities Capabilities => _console.Capabilities;
        public Encoding Encoding => _console.Encoding;
        public int Width { get; }
        public int Height => _console.Height;
        public IAnsiConsoleCursor Cursor => _console.Cursor;

        public TestableAnsiConsole(ColorSystem system, AnsiSupport ansi = AnsiSupport.Yes, int width = 80)
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
