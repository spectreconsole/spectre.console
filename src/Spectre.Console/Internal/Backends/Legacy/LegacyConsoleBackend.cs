using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class LegacyConsoleBackend : IAnsiConsoleBackend
    {
        private readonly IAnsiConsole _console;
        private Style _lastStyle;

        public IAnsiConsoleCursor Cursor { get; }

        public LegacyConsoleBackend(IAnsiConsole console)
        {
            _console = console ?? throw new System.ArgumentNullException(nameof(console));
            _lastStyle = Style.Plain;

            Cursor = new LegacyConsoleCursor();
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

        public void Write(IRenderable renderable)
        {
            foreach (var segment in renderable.GetSegments(_console))
            {
                if (segment.IsControlCode)
                {
                    continue;
                }

                if (_lastStyle?.Equals(segment.Style) != true)
                {
                    SetStyle(segment.Style);
                }

                _console.Profile.Out.Writer.Write(segment.Text.NormalizeNewLines(native: true));
            }
        }

        private void SetStyle(Style style)
        {
            _lastStyle = style;

            System.Console.ResetColor();

            var background = Color.ToConsoleColor(style.Background);
            if (_console.Profile.Capabilities.ColorSystem != ColorSystem.NoColors && (int)background != -1)
            {
                System.Console.BackgroundColor = background;
            }

            var foreground = Color.ToConsoleColor(style.Foreground);
            if (_console.Profile.Capabilities.ColorSystem != ColorSystem.NoColors && (int)foreground != -1)
            {
                System.Console.ForegroundColor = foreground;
            }
        }
    }
}
