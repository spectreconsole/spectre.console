using System;
using System.Collections.Generic;
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

            var context = new RenderContext(console.Profile.Encoding, console.Profile.Capabilities.Legacy);
            var renderables = console.Pipeline.Process(context, new[] { renderable });

            Render(console, context, renderables);
        }

        private static void Render(IAnsiConsole console, RenderContext options, IEnumerable<IRenderable> renderables)
        {
            var result = new List<Segment>();
            foreach (var renderable in renderables)
            {
                result.AddRange(renderable.Render(options, console.Profile.Width));
            }

            console.Write(Segment.Merge(result));
        }
    }
}
