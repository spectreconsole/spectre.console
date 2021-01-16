using System;
using System.Collections.Generic;
using System.IO;
using Spectre.Console.Rendering;

namespace Spectre.Console.Testing
{
    public sealed class FakeAnsiConsole : IDisposable, IAnsiConsole
    {
        private readonly StringWriter _writer;
        private readonly IAnsiConsole _console;

        public string Output => _writer.ToString();

        public Profile Profile => _console.Profile;
        public IAnsiConsoleCursor Cursor => _console.Cursor;
        public FakeConsoleInput Input { get; }
        public RenderPipeline Pipeline => _console.Pipeline;

        IAnsiConsoleInput IAnsiConsole.Input => Input;

        public FakeAnsiConsole(
            ColorSystem system,
            AnsiSupport ansi = AnsiSupport.Yes,
            int width = 80)
        {
            _writer = new StringWriter();

            var factory = AnsiConsoleFactory.NoEnrichers();
            _console = factory.Create(new AnsiConsoleSettings
            {
                Ansi = ansi,
                ColorSystem = (ColorSystemSupport)system,
                Out = _writer,
            });

            _console.Profile.Width = width;

            Input = new FakeConsoleInput();
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }

        public void Clear(bool home)
        {
            _console.Clear(home);
        }

        public void Write(IEnumerable<Segment> segments)
        {
            if (segments is null)
            {
                return;
            }

            foreach (var segment in segments)
            {
                _console.Write(segment);
            }
        }
    }
}
