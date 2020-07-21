using System;
using System.IO;

namespace Spectre.Console.Tests
{
    public sealed class AnsiConsoleFixture : IDisposable
    {
        private readonly StringWriter _writer;

        public IAnsiConsole Console { get; }

        public string Output => _writer.ToString();

        public AnsiConsoleFixture(ColorSystem system)
        {
            _writer = new StringWriter();

            Console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Yes,
                ColorSystem = (ColorSystemSupport)system,
                Out = _writer,
            });
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }
    }
}
