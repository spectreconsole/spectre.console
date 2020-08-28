using System;
using System.IO;
using System.Text;

namespace Spectre.Console.Internal
{
    internal sealed class AnsiConsoleRenderer : IAnsiConsole
    {
        private readonly TextWriter _out;
        private readonly ColorSystem _system;

        public Capabilities Capabilities { get; }
        public Encoding Encoding { get; }
        public Decoration Decoration { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }

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
            _system = system;

            Capabilities = new Capabilities(true, system, legacyConsole);
            Encoding = @out.IsStandardOut() ? System.Console.OutputEncoding : Encoding.UTF8;
            Foreground = Color.Default;
            Background = Color.Default;
            Decoration = Decoration.None;
        }

        public void Write(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var parts = text.NormalizeLineEndings().Split(new[] { '\n' });
            foreach (var (_, _, last, part) in parts.Enumerate())
            {
                if (!string.IsNullOrEmpty(part))
                {
                    _out.Write(AnsiBuilder.GetAnsi(_system, part, Decoration, Foreground, Background));
                }

                if (!last)
                {
                    _out.Write(Environment.NewLine);
                }
            }
        }
    }
}