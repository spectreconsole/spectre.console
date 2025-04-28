using Spectre.Console;
using Spectre.Console.Rendering;

namespace Spectre.Console.BlazorWasm
{
    /// <summary>
    /// An IAnsiConsoleBackend implementation for Blazor WebAssembly.
    /// Routes output to a Blazor component.
    /// </summary>
    public sealed class BlazorAnsiConsoleBackend : IAnsiConsoleBackend
    {
        private readonly IAnsiConsoleCursor _cursor;
        private readonly BlazorConsoleService _consoleService;

        public BlazorAnsiConsoleBackend(BlazorConsoleService consoleService)
        {
            _consoleService = consoleService;
            _cursor = new BlazorAnsiConsoleCursor();
        }

        public IAnsiConsoleCursor Cursor => _cursor;

        public void Clear(bool home)
        {
            _consoleService.Clear();
        }

        public void Write(IRenderable renderable)
        {
            // Set console width/height for Spectre.Console rendering (WASM polyfill)
            Spectre.Console.ConsolePolyfill.BufferWidth = 80;
            Spectre.Console.ConsolePolyfill.WindowHeight = 24;

            // Use Spectre.Console's rendering engine to render to a string
            var outputWriter = new StringWriter();

            var output = new FixedWidthAnsiConsoleOutput(outputWriter, 80, 24);

            var consoleSettings = new AnsiConsoleSettings
            {
                Out = output,
                ColorSystem = ColorSystemSupport.TrueColor,
                Interactive = InteractionSupport.No,
                Ansi = AnsiSupport.Yes
            };

            var console = AnsiConsole.Create(consoleSettings);
            console.Write(renderable);

            // Get the rendered output from the StringWriter
            var result = outputWriter.ToString();
            _consoleService.Write(result);
        }

        // Custom output to provide a fixed width/height
        private sealed class FixedWidthAnsiConsoleOutput : IAnsiConsoleOutput
        {
            private readonly TextWriter _writer;
            private readonly int _width;
            private readonly int _height;

            public FixedWidthAnsiConsoleOutput(TextWriter writer, int width, int height)
            {
                _writer = writer;
                _width = width;
                _height = height;
            }

            public TextWriter Writer => _writer;
            public int Width => _width;
            public int Height => _height;
            public bool IsTerminal => false;
            public bool IsOutputRedirected => false;
            public void SetEncoding(System.Text.Encoding encoding) { }
        }
    }

    // Stub for the Blazor-specific cursor
    internal sealed class BlazorAnsiConsoleCursor : IAnsiConsoleCursor
    {
        public int Top => 0;
        public int Left => 0;
        public bool IsEnabled => true;

        public void Hide() { }
        public void Show() { }
        public void Show(bool visible) { }
        public void SetPosition(int left, int top) { }
        public void Move(CursorDirection direction, int count) { }
    }
}
