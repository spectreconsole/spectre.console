using System;
using System.IO;
using System.Text;

namespace Spectre.Console.Internal
{
    internal sealed class AnsiConsoleRenderer : IAnsiConsole
    {
        private readonly TextWriter _out;

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

        public AnsiConsoleRenderer(TextWriter @out, ColorSystem system, bool legacyConsole)
        {
            _out = @out ?? throw new ArgumentNullException(nameof(@out));

            Capabilities = new Capabilities(true, system, legacyConsole);
            Encoding = @out.IsStandardOut() ? System.Console.OutputEncoding : Encoding.UTF8;
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
                    _out.Write(AnsiBuilder.GetAnsi(Capabilities, part, style.Decoration, style.Foreground, style.Background, style.Link));
                }

                if (!last)
                {
                    _out.Write(Environment.NewLine);
                }
            }
        }
    }
}