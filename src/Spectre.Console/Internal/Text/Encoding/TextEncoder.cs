using System.Collections.Generic;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal sealed class TextEncoder : IAnsiConsoleEncoder
    {
        public string Encode(IAnsiConsole console, IEnumerable<IRenderable> renderables)
        {
            var context = new RenderContext(new EncoderCapabilities(ColorSystem.TrueColor));
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
