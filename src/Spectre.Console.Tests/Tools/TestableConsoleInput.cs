using System;
using System.Collections.Generic;

namespace Spectre.Console.Tests
{
    public sealed class TestableConsoleInput : IAnsiConsoleInput
    {
        private readonly Queue<ConsoleKeyInfo> _input;

        public TestableConsoleInput()
        {
            _input = new Queue<ConsoleKeyInfo>();
        }

        public void PushText(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            foreach (var character in input)
            {
                PushCharacter(character);
            }
        }

        public void PushTextWithEnter(string input)
        {
            PushText(input);
            PushKey(ConsoleKey.Enter);
        }

        public void PushCharacter(char character)
        {
            var control = char.IsUpper(character);
            _input.Enqueue(new ConsoleKeyInfo(character, (ConsoleKey)character, false, false, control));
        }

        public void PushKey(ConsoleKey key)
        {
            _input.Enqueue(new ConsoleKeyInfo((char)key, key, false, false, false));
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            if (_input.Count == 0)
            {
                throw new InvalidOperationException("No input available.");
            }

            return _input.Dequeue();
        }
    }
}
