using System;
using System.Text;
using Spectre.Console.Rendering;
using static Spectre.Console.AnsiSequences;

namespace Spectre.Console
{
    internal sealed class AnsiConsoleBackend : IAnsiConsoleBackend
    {
        private readonly IAnsiConsole _console;

        public IAnsiConsoleCursor Cursor { get; }

        public AnsiConsoleBackend(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            Cursor = new AnsiConsoleCursor(this);
        }

        public void Clear(bool home)
        {
            Write(new ControlCode(ED(2)));
            Write(new ControlCode(ED(3)));

            if (home)
            {
                Write(new ControlCode(CUP(1, 1)));
            }
        }

        public void Write(IRenderable renderable)
        {
            var result = AnsiBuilder.Build(_console, renderable);
            if (result?.Length > 0)
            {
                _console.Profile.Out.Writer.Write(result);
                _console.Profile.Out.Writer.Flush();
            }
        }
    }
}
