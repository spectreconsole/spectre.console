using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal sealed class AnsiBackend : IAnsiConsole
    {
        private readonly TextWriter _out;
        private readonly AnsiBuilder _ansiBuilder;
        private readonly AnsiCursor _cursor;
        private readonly ConsoleInput _input;
        private readonly object _lock;

        public Capabilities Capabilities { get; }
        public Encoding Encoding { get; }
        public RenderPipeline Pipeline { get; }
        public IAnsiConsoleCursor Cursor => _cursor;
        public IAnsiConsoleInput Input => _input;

        public int Width
        {
            get
            {
                if (_out.IsStandardOut())
                {
                    return ConsoleHelper.GetSafeWidth(Constants.DefaultTerminalWidth);
                }

                return Constants.DefaultTerminalWidth;
            }
        }

        public int Height
        {
            get
            {
                if (_out.IsStandardOut())
                {
                    return ConsoleHelper.GetSafeHeight(Constants.DefaultTerminalHeight);
                }

                return Constants.DefaultTerminalHeight;
            }
        }

        public AnsiBackend(TextWriter @out, Capabilities capabilities, ILinkIdentityGenerator? linkHasher)
        {
            _out = @out ?? throw new ArgumentNullException(nameof(@out));

            Capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
            Encoding = _out.IsStandardOut() ? System.Console.OutputEncoding : Encoding.UTF8;
            Pipeline = new RenderPipeline();

            _ansiBuilder = new AnsiBuilder(Capabilities, linkHasher);
            _cursor = new AnsiCursor(this);
            _input = new ConsoleInput();
            _lock = new object();
        }

        public void Clear(bool home)
        {
            lock (_lock)
            {
                Write(new[] { Segment.Control("\u001b[2J") });

                if (home)
                {
                    Cursor.SetPosition(0, 0);
                }
            }
        }

        public void Write(IEnumerable<Segment> segments)
        {
            lock (_lock)
            {
                var builder = new StringBuilder();
                foreach (var segment in segments)
                {
                    if (segment.IsControlCode)
                    {
                        builder.Append(segment.Text);
                        continue;
                    }

                    var parts = segment.Text.NormalizeNewLines().Split(new[] { '\n' });
                    foreach (var (_, _, last, part) in parts.Enumerate())
                    {
                        if (!string.IsNullOrEmpty(part))
                        {
                            builder.Append(_ansiBuilder.GetAnsi(part, segment.Style));
                        }

                        if (!last)
                        {
                            builder.Append(Environment.NewLine);
                        }
                    }
                }

                if (builder.Length > 0)
                {
                    _out.Write(builder.ToString());
                    _out.Flush();
                }
            }
        }
    }
}