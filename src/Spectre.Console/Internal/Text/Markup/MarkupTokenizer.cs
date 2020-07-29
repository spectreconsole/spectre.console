using System;
using System.Text;

namespace Spectre.Console.Internal
{
    internal sealed class MarkupTokenizer : IDisposable
    {
        private readonly StringBuffer _reader;

        public MarkupTokenizer(string text)
        {
            _reader = new StringBuffer(text ?? throw new ArgumentNullException(nameof(text)));
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public MarkupToken GetNext()
        {
            if (_reader.Eof)
            {
                return null;
            }

            var current = _reader.Peek();
            if (current == '[')
            {
                var position = _reader.Position;

                _reader.Read();

                if (_reader.Eof)
                {
                    throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position}.");
                }

                current = _reader.Peek();
                if (current == '[')
                {
                    _reader.Read();
                    return new MarkupToken(MarkupTokenKind.Text, "[", position);
                }

                if (current == '/')
                {
                    _reader.Read();

                    if (_reader.Eof)
                    {
                        throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position}.");
                    }

                    current = _reader.Peek();
                    if (current != ']')
                    {
                        throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position}.");
                    }

                    _reader.Read();
                    return new MarkupToken(MarkupTokenKind.Close, string.Empty, position);
                }

                var builder = new StringBuilder();
                while (!_reader.Eof)
                {
                    current = _reader.Peek();
                    if (current == ']')
                    {
                        break;
                    }

                    builder.Append(_reader.Read());
                }

                if (_reader.Eof)
                {
                    throw new InvalidOperationException($"Encountered malformed markup tag at position {_reader.Position}.");
                }

                _reader.Read();
                return new MarkupToken(MarkupTokenKind.Open, builder.ToString(), position);
            }
            else
            {
                var position = _reader.Position;
                var builder = new StringBuilder();
                while (!_reader.Eof)
                {
                    current = _reader.Peek();
                    if (current == '[')
                    {
                        break;
                    }

                    builder.Append(_reader.Read());
                }

                return new MarkupToken(MarkupTokenKind.Text, builder.ToString(), position);
            }
        }
    }
}
