using System;
using System.IO;
using System.Text;

namespace Spectre.Console.Internal
{
    internal sealed class AnsiConsoleRenderer : IAnsiConsole
    {
        private readonly TextWriter _out;
        private readonly AnsiBuilder _ansiBuilder;

        public Capabilities Capabilities { get; }
        public Encoding Encoding { get; }

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

        public AnsiConsoleRenderer(TextWriter @out, Capabilities capabilities, ILinkIdentityGenerator? linkHasher)
        {
            _out = @out ?? throw new ArgumentNullException(nameof(@out));

            Capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
            Encoding = _out.IsStandardOut() ? System.Console.OutputEncoding : Encoding.UTF8;

            _ansiBuilder = new AnsiBuilder(Capabilities, linkHasher);
        }

        public void Write(string text, Style style)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            style ??= Style.Plain;

            var parts = text.NormalizeLineEndings().Split(new[] { '\n' });
            foreach (var (_, _, last, part) in parts.Enumerate())
            {
                if (!string.IsNullOrEmpty(part))
                {
                    _out.Write(_ansiBuilder.GetAnsi(part, style));
                }

                if (!last)
                {
                    _out.Write(Environment.NewLine);
                }
            }
        }
    }
}