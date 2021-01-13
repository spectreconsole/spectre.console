using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal sealed class FallbackBackend : IAnsiConsole
    {
        private readonly ColorSystem _system;
        private readonly FallbackCursor _cursor;
        private readonly ConsoleInput _input;
        private Style? _lastStyle;

        public Capabilities Capabilities { get; }
        public Encoding Encoding { get; }
        public RenderPipeline Pipeline { get; }
        public IAnsiConsoleCursor Cursor => _cursor;
        public IAnsiConsoleInput Input => _input;

        public int Width
        {
            get { return ConsoleHelper.GetSafeWidth(Constants.DefaultTerminalWidth); }
        }

        public int Height
        {
            get { return ConsoleHelper.GetSafeHeight(Constants.DefaultTerminalHeight); }
        }

        public FallbackBackend(TextWriter @out, Capabilities capabilities)
        {
            if (capabilities == null)
            {
                throw new ArgumentNullException(nameof(capabilities));
            }

            _system = capabilities.ColorSystem;
            _cursor = new FallbackCursor();
            _input = new ConsoleInput();

            if (@out != System.Console.Out)
            {
                System.Console.SetOut(@out ?? throw new ArgumentNullException(nameof(@out)));
            }

            Capabilities = capabilities;
            Encoding = System.Console.OutputEncoding;
            Pipeline = new RenderPipeline();
        }

        public void Clear(bool home)
        {
            var (x, y) = (System.Console.CursorLeft, System.Console.CursorTop);

            System.Console.Clear();

            if (!home)
            {
                // Set the cursor position
                System.Console.SetCursorPosition(x, y);
            }
        }

        public void Write(IEnumerable<Segment> segments)
        {
            foreach (var segment in segments)
            {
                if (segment.IsControlCode)
                {
                    continue;
                }

                if (_lastStyle?.Equals(segment.Style) != true)
                {
                    SetStyle(segment.Style);
                }

                System.Console.Write(segment.Text.NormalizeNewLines(native: true));
            }
        }

        private void SetStyle(Style style)
        {
            _lastStyle = style;

            System.Console.ResetColor();

            var background = Color.ToConsoleColor(style.Background);
            if (_system != ColorSystem.NoColors && (int)background != -1)
            {
                System.Console.BackgroundColor = background;
            }

            var foreground = Color.ToConsoleColor(style.Foreground);
            if (_system != ColorSystem.NoColors && (int)foreground != -1)
            {
                System.Console.ForegroundColor = foreground;
            }
        }
    }
}
