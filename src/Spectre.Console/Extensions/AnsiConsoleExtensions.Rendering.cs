using System;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
        /// <summary>
        /// Renders the specified object to the console.
        /// </summary>
        /// <param name="console">The console to render to.</param>
        /// <param name="renderable">The object to render.</param>
        public static void Render(this IAnsiConsole console, IRenderable renderable)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (renderable is null)
            {
                throw new ArgumentNullException(nameof(renderable));
            }

            var options = new RenderContext(console.Encoding, console.Capabilities.LegacyConsole);
            var segments = renderable.Render(options, console.Width).Where(x => !(x.Text.Length == 0 && !x.IsLineBreak)).ToArray();
            segments = Segment.Merge(segments).ToArray();

            var current = Style.Plain;
            foreach (var segment in segments)
            {
                if (string.IsNullOrEmpty(segment.Text))
                {
                    continue;
                }

                console.Write(segment.Text, segment.Style);
            }
        }
    }
}
