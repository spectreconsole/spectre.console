using System;
using System.IO;
using System.Text;

namespace Spectre.Console.Internal
{
    internal sealed class FallbackConsoleRenderer : IAnsiConsole
    {
        private readonly ConsoleColor _defaultForeground;
        private readonly ConsoleColor _defaultBackground;

        private readonly TextWriter _out;
        private readonly ColorSystem _system;
        private ConsoleColor _foreground;
        private ConsoleColor _background;

        public Capabilities Capabilities { get; }

        public Encoding Encoding { get; }

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

        public Styles Style { get; set; }

        public Color Foreground
        {
            get => _foreground;
            set
            {
                _foreground = Color.ToConsoleColor(value);
                if (_system != ColorSystem.NoColors && _out.IsStandardOut())
                {
                    if ((int)_foreground == -1)
                    {
                        _foreground = _defaultForeground;
                    }

                    System.Console.ForegroundColor = _foreground;
                }
            }
        }

        public Color Background
        {
            get => _background;
            set
            {
                _background = Color.ToConsoleColor(value);
                if (_system != ColorSystem.NoColors && _out.IsStandardOut())
                {
                    if ((int)_background == -1)
                    {
                        _background = _defaultBackground;
                    }

                    if (_system != ColorSystem.NoColors)
                    {
                        System.Console.BackgroundColor = _background;
                    }
                }
            }
        }

        public FallbackConsoleRenderer(TextWriter @out, ColorSystem system)
        {
            _out = @out;
            _system = system;

            if (_out.IsStandardOut())
            {
                _defaultForeground = System.Console.ForegroundColor;
                _defaultBackground = System.Console.BackgroundColor;

                Encoding = System.Console.OutputEncoding;
            }
            else
            {
                _defaultForeground = ConsoleColor.Gray;
                _defaultBackground = ConsoleColor.Black;

                Encoding = Encoding.UTF8;
            }

            Capabilities = new Capabilities(false, _system);
        }

        public void Write(string text)
        {
            _out.Write(text.NormalizeLineEndings(native: true));
        }
    }
}
