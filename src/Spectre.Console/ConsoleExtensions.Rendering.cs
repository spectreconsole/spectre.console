using System;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class ConsoleExtensions
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

            using (console.PushStyle(Style.Plain))
            {
                var segments = renderable.Render(options, console.Width);
                segments = Segment.Merge(segments);

                var current = Style.Plain;
                foreach (var segment in segments)
                {
                    if (string.IsNullOrEmpty(segment.Text))
                    {
                        continue;
                    }

                    if (!segment.Style.Equals(current))
                    {
                        console.Foreground = segment.Style.Foreground;
                        console.Background = segment.Style.Background;
                        console.Decoration = segment.Style.Decoration;
                        current = segment.Style;
                    }

                    console.Write(segment.Text);
                }
            }
        }
    }
}
