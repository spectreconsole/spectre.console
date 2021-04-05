using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Testing
{
    public sealed class FakeConsole : IAnsiConsole, IDisposable
    {
        public Profile Profile { get; }
        public IAnsiConsoleCursor Cursor => new FakeAnsiConsoleCursor();
        IAnsiConsoleInput IAnsiConsole.Input => Input;
        public IExclusivityMode ExclusivityMode { get; }
        public RenderPipeline Pipeline { get; }

        public FakeConsoleInput Input { get; }
        public string Output => Profile.Out.ToString();
        public IReadOnlyList<string> Lines => Output.TrimEnd('\n').Split(new char[] { '\n' });

        public FakeConsole(
            int width = 80, int height = 9000, Encoding encoding = null,
            bool supportsAnsi = true, ColorSystem colorSystem = ColorSystem.Standard,
            bool legacyConsole = false, bool interactive = true)
        {
            Input = new FakeConsoleInput();
            ExclusivityMode = new FakeExclusivityMode();
            Pipeline = new RenderPipeline();

            Profile = new Profile(new StringWriter(), encoding ?? Encoding.UTF8);
            Profile.Width = width;
            Profile.Height = height;
            Profile.ColorSystem = colorSystem;
            Profile.Capabilities.Ansi = supportsAnsi;
            Profile.Capabilities.Legacy = legacyConsole;
            Profile.Capabilities.Interactive = interactive;
            Profile.Capabilities.Links = true;
            Profile.Capabilities.Unicode = true;
        }

        public void Dispose()
        {
            Profile.Out.Dispose();
        }

        public void Clear(bool home)
        {
        }

        public void Write(IRenderable renderable)
        {
            foreach (var segment in this.GetSegments(renderable))
            {
                Profile.Out.Write(segment.Text);
            }
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
