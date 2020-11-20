using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Tests
{
    public sealed class PlainConsole : IAnsiConsole, IDisposable
    {
        public Capabilities Capabilities { get; }
        public Encoding Encoding { get; }
        public IAnsiConsoleCursor Cursor => throw new NotSupportedException();
        public TestableConsoleInput Input { get; }

        public int Width { get; }
        public int Height { get; }

        IAnsiConsoleInput IAnsiConsole.Input => Input;

        public Decoration Decoration { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }
        public string Link { get; set; }

        public StringWriter Writer { get; }
        public string Output => Writer.ToString();
        public IReadOnlyList<string> Lines => Output.TrimEnd('\n').Split(new char[] { '\n' });

        public PlainConsole(
            int width = 80, int height = 9000, Encoding encoding = null,
            bool supportsAnsi = true, ColorSystem colorSystem = ColorSystem.Standard,
            bool legacyConsole = false)
        {
            Capabilities = new Capabilities(supportsAnsi, colorSystem, legacyConsole);
            Encoding = encoding ?? Encoding.UTF8;
            Width = width;
            Height = height;
            Writer = new StringWriter();
            Input = new TestableConsoleInput();
        }

        public void Dispose()
        {
            Writer.Dispose();
        }

        public void Clear(bool home)
        {
        }

        public void Write(Segment segment)
        {
            if (segment is null)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            Writer.Write(segment.Text);
        }

        public string WriteNormalizedException(Exception ex, ExceptionFormats formats = ExceptionFormats.Default)
        {
            this.WriteException(ex, formats);
            return string.Join("\n", Output.NormalizeStackTrace()
                .NormalizeLineEndings()
                .Split(new char[] { '\n' })
                .Select(line => line.TrimEnd()));
        }
    }
}
