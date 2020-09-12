using System;
using System.IO;
using System.Text;

namespace Spectre.Console.Internal
{
    internal sealed class FallbackConsoleRenderer : IAnsiConsole
    {
        private readonly TextWriter _out;
        private readonly ColorSystem _system;
        private Style? _lastStyle;

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

        public FallbackConsoleRenderer(TextWriter @out, Capabilities capabilities)
        {
            if (capabilities == null)
            {
                throw new ArgumentNullException(nameof(capabilities));
            }

            _out = @out ?? throw new ArgumentNullException(nameof(@out));
            _system = capabilities.ColorSystem;

            if (_out.IsStandardOut())
            {
                Encoding = System.Console.OutputEncoding;
            }
            else
            {
                Encoding = Encoding.UTF8;
            }

            Capabilities = capabilities;
        }

        public void Write(string text, Style style)
        {
            if (_lastStyle?.Equals(style) != true)
            {
                SetStyle(style);
            }

            _out.Write(text.NormalizeLineEndings(native: true));
        }

        private void SetStyle(Style style)
        {
            _lastStyle = style;

            if (_out.IsStandardOut())
            {
                System.Console.ResetColor();

                var background = Color.ToConsoleColor(style.Background);
                if (_system != ColorSystem.NoColors && _out.IsStandardOut() && (int)background != -1)
                {
                    System.Console.BackgroundColor = background;
                }

                var foreground = Color.ToConsoleColor(style.Foreground);
                if (_system != ColorSystem.NoColors && _out.IsStandardOut() && (int)foreground != -1)
                {
                    System.Console.ForegroundColor = foreground;
                }
            }
        }
    }
}
