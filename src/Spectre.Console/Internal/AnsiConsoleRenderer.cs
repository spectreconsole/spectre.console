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
        public Styles Style { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }

        public int Width
        {
            get
            {
                if (_out.IsStandardOut())
                {
                    return System.Console.BufferWidth;
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
                    return System.Console.BufferHeight;
                }

                return Constants.DefaultBufferHeight;
            }
        }

        public AnsiConsoleRenderer(TextWriter @out, ColorSystem system)
        {
            _out = @out ?? throw new ArgumentNullException(nameof(@out));
            _system = system;

            Capabilities = new Capabilities(true, system);
            Encoding = @out.IsStandardOut() ? System.Console.OutputEncoding : Encoding.UTF8;
            Foreground = Color.Default;
            Background = Color.Default;
            Style = Styles.None;
        }

        public void Reset(bool colors, bool styles)
        {
            if (colors)
            {
                Foreground = Color.Default;
                Background = Color.Default;
            }

            if (styles)
            {
                Style = Styles.None;
            }
        }

        public void Write(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            _out.Write(AnsiBuilder.GetAnsi(
                _system,
                text.NormalizeLineEndings(native: true),
                Style,
                Foreground,
                Background));
        }
    }
}