using System;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal sealed class AnsiCursor : IAnsiConsoleCursor
    {
        private readonly AnsiBackend _renderer;

        public AnsiCursor(AnsiBackend renderer)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public void Show(bool show)
        {
            if (show)
            {
                _renderer.Write(Segment.Control("\u001b[?25h"));
            }
            else
            {
                _renderer.Write(Segment.Control("\u001b[?25l"));
            }
        }

        public void Move(CursorDirection direction, int steps)
        {
            if (steps == 0)
            {
                return;
            }

            switch (direction)
            {
                case CursorDirection.Up:
                    _renderer.Write(Segment.Control($"\u001b[{steps}A"));
                    break;
                case CursorDirection.Down:
                    _renderer.Write(Segment.Control($"\u001b[{steps}B"));
                    break;
                case CursorDirection.Right:
                    _renderer.Write(Segment.Control($"\u001b[{steps}C"));
                    break;
                case CursorDirection.Left:
                    _renderer.Write(Segment.Control($"\u001b[{steps}D"));
                    break;
            }
        }

        public void SetPosition(int column, int line)
        {
            _renderer.Write(Segment.Control($"\u001b[{line};{column}H"));
        }
    }
}