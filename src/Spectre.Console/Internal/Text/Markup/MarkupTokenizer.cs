using System;
using System.Text;

namespace Spectre.Console
{
    internal sealed class MarkupTokenizer : IDisposable
    {
        private readonly StringBuffer _reader;

        public MarkupToken? Current { get; private set; }

        public MarkupTokenizer(string text)
        {
            _reader = new StringBuffer(text ?? throw new ArgumentNullException(nameof(text)));
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public bool MoveNext()
        {
            if (_reader.Eof)
            {
                return false;
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
                    Current = new MarkupToken(MarkupTokenKind.Text, "[", position);
                    return true;
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
                    Current = new MarkupToken(MarkupTokenKind.Close, string.Empty, position);
                    return true;
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
                Current = new MarkupToken(MarkupTokenKind.Open, builder.ToString(), position);
                return true;
            }
            else
            {
                var position = _reader.Position;
                var builder = new StringBuilder();

                var encounteredClosing = false;
                while (!_reader.Eof)
                {
                    current = _reader.Peek();
                    if (current == '[')
                    {
                        break;
                    }
                    else if (current == ']')
                    {
                        if (encounteredClosing)
                        {
                            _reader.Read();
                            encounteredClosing = false;
                            continue;
                        }

                        encounteredClosing = true;
                    }
                    else
                    {
                        if (encounteredClosing)
                        {
                            throw new InvalidOperationException(
                                $"Encountered unescaped ']' token at position {_reader.Position}");
                        }
                    }

                    builder.Append(_reader.Read());
                }

                if (encounteredClosing)
                {
                    throw new InvalidOperationException($"Encountered unescaped ']' token at position {_reader.Position}");
                }

                Current = new MarkupToken(MarkupTokenKind.Text, builder.ToString(), position);
                return true;
            }
        }
    }
}
