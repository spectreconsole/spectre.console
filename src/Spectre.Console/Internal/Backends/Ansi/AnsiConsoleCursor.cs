using System;
using static Spectre.Console.AnsiSequences;

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
                _backend.Write(new ControlCode(SM(DECTCEM)));
            }
            else
            {
                _backend.Write(new ControlCode(RM(DECTCEM)));
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
                    _backend.Write(new ControlCode(CUU(steps)));
                    break;
                case CursorDirection.Down:
                    _backend.Write(new ControlCode(CUD(steps)));
                    break;
                case CursorDirection.Right:
                    _backend.Write(new ControlCode(CUF(steps)));
                    break;
                case CursorDirection.Left:
                    _backend.Write(new ControlCode(CUB(steps)));
                    break;
            }
        }

        public void SetPosition(int column, int line)
        {
            _backend.Write(new ControlCode(CUP(line, column)));
        }
    }
}
