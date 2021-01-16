using System;
using System.IO;

namespace Spectre.Console.Cli
{
    internal sealed class TextBuffer : IDisposable
    {
        // There is some kind of bug
        private readonly StringReader _reader;

        public bool ReachedEnd => _reader.Peek() == -1;
        public string Original { get; }
        public int Position { get; private set; }

        public TextBuffer(string text)
        {
            _reader = new StringReader(text);
            Original = text;
            Position = 0;
        }

        public TextBuffer(TextBuffer? buffer, string text)
        {
            _reader = new StringReader(text);
            Original = buffer != null ? buffer.Original + " " + text : text;
            Position = buffer?.Position + 1 ?? 0;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public char Peek()
        {
            return (char)_reader.Peek();
        }

        public bool TryPeek(out char character)
        {
            var value = _reader.Peek();
            if (value == -1)
            {
                character = '\0';
                return false;
            }

            character = (char)value;
            return true;
        }

        public void Consume()
        {
            EnsureNotAtEnd();
            Read();
        }

        public void Consume(char character)
        {
            EnsureNotAtEnd();
            if (Read() != character)
            {
                throw new InvalidOperationException($"Expected '{character}' token.");
            }
        }

        public bool IsNext(char character)
        {
            if (TryPeek(out var result))
            {
                return result == character;
            }

            return false;
        }

        public char Read()
        {
            EnsureNotAtEnd();
            var result = (char)_reader.Read();
            Position++;
            return result;
        }

        private void EnsureNotAtEnd()
        {
            if (ReachedEnd)
            {
                throw new InvalidOperationException("Can't read past the end of the buffer.");
            }
        }
    }
}
