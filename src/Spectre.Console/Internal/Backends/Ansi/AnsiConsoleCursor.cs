using System;

namespace Spectre.Console
{
    internal sealed class AnsiConsoleCursor : IAnsiConsoleCursor
    {
        private readonly AnsiConsoleBackend _backend;

        public AnsiConsoleCursor(AnsiConsoleBackend backend)
        {
            _backend = backend ?? throw new ArgumentNullException(nameof(backend));
        }

        public void Show(bool show)
        {
            if (show)
            {
                _backend.Write(new ControlSequence("\u001b[?25h"));
            }
            else
            {
                _backend.Write(new ControlSequence("\u001b[?25l"));
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
                    _backend.Write(new ControlSequence($"\u001b[{steps}A"));
                    break;
                case CursorDirection.Down:
                    _backend.Write(new ControlSequence($"\u001b[{steps}B"));
                    break;
                case CursorDirection.Right:
                    _backend.Write(new ControlSequence($"\u001b[{steps}C"));
                    break;
                case CursorDirection.Left:
                    _backend.Write(new ControlSequence($"\u001b[{steps}D"));
                    break;
            }
        }

        public void SetPosition(int column, int line)
        {
            _backend.Write(new ControlSequence($"\u001b[{line};{column}H"));
        }
    }
}
