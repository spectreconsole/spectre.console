using System;
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

        public Capabilities Capabilities { get; }
        public Encoding Encoding { get; }
        public IAnsiConsoleCursor Cursor => _cursor;

        public int Width
        {
            get
            {
                if (_out.IsStandardOut())
                {
                    return ConsoleHelper.GetSafeBufferWidth(Constants.DefaultBufferWidth);
                }

                return Constants.DefaultBufferWidth;
            }
        }

        public int Height
        {
            get
            {
                if (_out.IsStandardOut())
                {
                    return ConsoleHelper.GetSafeBufferHeight(Constants.DefaultBufferHeight);
                }

                return Constants.DefaultBufferHeight;
            }
        }

        public AnsiBackend(TextWriter @out, Capabilities capabilities, ILinkIdentityGenerator? linkHasher)
        {
            _out = @out ?? throw new ArgumentNullException(nameof(@out));

            Capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
            Encoding = _out.IsStandardOut() ? System.Console.OutputEncoding : Encoding.UTF8;

            _ansiBuilder = new AnsiBuilder(Capabilities, linkHasher);
            _cursor = new AnsiCursor(this);
        }

        public void Clear(bool home)
        {
            Write(Segment.Control("\u001b[2J"));

            if (home)
            {
                Cursor.SetPosition(0, 0);
            }
        }

        public void Write(Segment segment)
        {
            var parts = segment.Text.NormalizeLineEndings().Split(new[] { '\n' });
            foreach (var (_, _, last, part) in parts.Enumerate())
            {
                if (!string.IsNullOrEmpty(part))
                {
                    _out.Write(_ansiBuilder.GetAnsi(part, segment.Style));
                }

                if (!last)
                {
                    _out.Write(Environment.NewLine);
                }
            }
        }
    }
}