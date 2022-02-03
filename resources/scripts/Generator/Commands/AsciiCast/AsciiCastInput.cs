using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace Generator.Commands
{
    public sealed class AsciiCastInput : IAnsiConsoleInput
    {
        private readonly Queue<(ConsoleKeyInfo?, int)> _input;
        private readonly Random _random = new Random();

        public AsciiCastInput()
        {
            _input = new Queue<(ConsoleKeyInfo?, int)>();
        }

        public void PushText(string input, int keypressDelayMs)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            foreach (var character in input)
            {
                PushCharacter(character, keypressDelayMs);
            }
        }

        public void PushTextWithEnter(string input, int keypressDelayMs)
        {
            PushText(input, keypressDelayMs);
            PushKey(ConsoleKey.Enter, keypressDelayMs);
        }

        public void PushCharacter(char input, int keypressDelayMs)
        {
            var delay = keypressDelayMs + _random.Next((int)(keypressDelayMs * -.2), (int)(keypressDelayMs * .2));

            switch (input)
            {
                case '↑':
                    PushKey(ConsoleKey.UpArrow, keypressDelayMs);
                    break;
                case '↓':
                    PushKey(ConsoleKey.DownArrow, keypressDelayMs);
                    break;
                case '↲':
                    PushKey(ConsoleKey.Enter, keypressDelayMs);
                    break;
                case '¦':
                    _input.Enqueue((null, delay));
                    break;
                default:
                    var control = char.IsUpper(input);
                    _input.Enqueue((new ConsoleKeyInfo(input, (ConsoleKey)input, false, false, control), delay));
                    break;
            }
        }

        public void PushKey(ConsoleKey input, int keypressDelayMs)
        {
            var delay = keypressDelayMs + _random.Next((int)(keypressDelayMs * -.2), (int)(keypressDelayMs * .2));
            _input.Enqueue((new ConsoleKeyInfo((char)input, input, false, false, false), delay));
        }

        public bool IsKeyAvailable()
        {
            return _input.Count > 0;
        }

        public ConsoleKeyInfo? ReadKey(bool intercept)
        {
            if (_input.Count == 0)
            {
                throw new InvalidOperationException("No input available.");
            }

            var result = _input.Dequeue();

            Thread.Sleep(result.Item2);
            return result.Item1;
        }

        public Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken)
        {
            return Task.FromResult(ReadKey(intercept));
        }
    }
}
