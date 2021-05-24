using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using Spectre.Console;

namespace Generator.Commands
{
    public class AsciiCastOut : IAnsiConsoleOutput
    {
        private sealed class AsciiCastWriter : TextWriter
        {
            private readonly TextWriter _wrappedTextWriter;
            private readonly StringBuilder _builder = new StringBuilder();
            private int? _firstTick;

            public AsciiCastWriter(TextWriter wrappedTextWriter)
            {
                _wrappedTextWriter = wrappedTextWriter;
            }

            public override void Write(string value)
            {
                if (value == null)
                {
                    return;
                }

                Append(value);
                _wrappedTextWriter.Write(value);
                base.Write(value);
            }

            public override Encoding Encoding => _wrappedTextWriter.Encoding;

            private void Append(string value)
            {
                var tick = 0m;
                if (_firstTick.HasValue)
                {
                    tick = Environment.TickCount - _firstTick.Value;
                }
                else
                {
                    _firstTick = Environment.TickCount;
                }

                tick /= 1000m;

                _builder.Append('[')
                    .AppendFormat(CultureInfo.InvariantCulture, "{0}", tick)
                    .Append(", \"o\", \"").Append(JsonEncodedText.Encode(value)).AppendLine("\"]");
            }

            public string GetJsonAndClearBuffer()
            {
                var json = _builder.ToString();

                // reset the buffer and also reset the first tick count
                _builder.Clear();
                _firstTick = null;
                return json;
            }
        }

        private readonly IAnsiConsoleOutput _wrappedAnsiConsole;
        private readonly AsciiCastWriter _asciiCastWriter;

        public AsciiCastOut(IAnsiConsoleOutput wrappedAnsiConsole)
        {
            _wrappedAnsiConsole = wrappedAnsiConsole ?? throw new ArgumentNullException(nameof(wrappedAnsiConsole));
            _asciiCastWriter = new AsciiCastWriter(_wrappedAnsiConsole.Writer);
        }

        public TextWriter Writer => _asciiCastWriter;

        public bool IsTerminal => _wrappedAnsiConsole.IsTerminal;

        public int Width => _wrappedAnsiConsole.Width;

        public int Height => _wrappedAnsiConsole.Height;

        public void SetEncoding(Encoding encoding)
        {
            _wrappedAnsiConsole.SetEncoding(encoding);
        }

        public string GetCastJson(string title, int? width = null, int? height = null)
        {
            var header = $"{{\"version\": 2, \"width\": {width ?? _wrappedAnsiConsole.Width}, \"height\": {height ?? _wrappedAnsiConsole.Height}, \"title\": \"{JsonEncodedText.Encode(title)}\", \"env\": {{\"TERM\": \"Spectre.Console\"}}}}";
            return $"{header}{Environment.NewLine}{_asciiCastWriter.GetJsonAndClearBuffer()}{Environment.NewLine}";
        }
    }
}