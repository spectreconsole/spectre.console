using System;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Generator.Commands
{
    public sealed class AsciiCastConsole : IAnsiConsole
    {
        private readonly IAnsiConsole _console;
        private readonly AsciiCastInput _input;

        public Profile Profile => _console.Profile;

        public IAnsiConsoleCursor Cursor => _console.Cursor;

        IAnsiConsoleInput IAnsiConsole.Input => _input;

        public AsciiCastInput Input => _input;

        public IExclusivityMode ExclusivityMode => _console.ExclusivityMode;

        public RenderPipeline Pipeline => _console.Pipeline;

        public AsciiCastConsole(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _input = new AsciiCastInput();
        }

        public void Clear(bool home)
        {
            _console.Clear(home);
        }

        public void Write(IRenderable renderable)
        {
            _console.Write(renderable);
        }
    }
}
