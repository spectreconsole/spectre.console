using System.Collections.Generic;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal sealed class EncoderCapabilities : IReadOnlyCapabilities
    {
        public bool Ansi => false;
        public bool Links => false;
        public bool Legacy => false;
        public bool Tty => false;
        public bool Interactive => false;
        public bool Unicode => true;

        public static EncoderCapabilities Default { get; } = new EncoderCapabilities();
    }

    internal sealed class TextEncoder : IAnsiConsoleEncoder
    {
        public string Encode(IAnsiConsole console, IEnumerable<IRenderable> renderables)
        {
            var context = new RenderContext(EncoderCapabilities.Default);
            var builder = new StringBuilder();

            foreach (var renderable in renderables)
            {
                var segments = renderable.Render(context, console.Profile.Width);
                foreach (var segment in Segment.Merge(segments))
                {
                    if (segment.IsControlCode)
                    {
                        continue;
                    }

                    builder.Append(segment.Text);
                }
            }

            return builder.ToString().TrimEnd('\n');
        }
    }
}
