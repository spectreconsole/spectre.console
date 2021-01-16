using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class LegacyConsoleBackend : IAnsiConsoleBackend
    {
        private readonly Profile _profile;
        private Style _lastStyle;

        public IAnsiConsoleCursor Cursor { get; }

        public LegacyConsoleBackend(Profile profile)
        {
            _profile = profile ?? throw new System.ArgumentNullException(nameof(profile));
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

        public void Render(IEnumerable<Segment> segments)
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

                _profile.Out.Write(segment.Text.NormalizeNewLines(native: true));
            }
        }

        private void SetStyle(Style style)
        {
            _lastStyle = style;

            System.Console.ResetColor();

            var background = Color.ToConsoleColor(style.Background);
            if (_profile.ColorSystem != ColorSystem.NoColors && (int)background != -1)
            {
                System.Console.BackgroundColor = background;
            }

            var foreground = Color.ToConsoleColor(style.Foreground);
            if (_profile.ColorSystem != ColorSystem.NoColors && (int)foreground != -1)
            {
                System.Console.ForegroundColor = foreground;
            }
        }
    }
}
