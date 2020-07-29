using System;
using Spectre.Console.Composition;
using Spectre.Console.Internal;

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

            foreach (var segment in renderable.Render(console.Encoding, console.Width))
            {
                if (!segment.Appearance.Equals(Appearance.Plain))
                {
                    using (var appearance = console.PushAppearance(segment.Appearance))
                    {
                        console.Write(segment.Text);
                    }
                }
                else
                {
                    console.Write(segment.Text);
                }
            }
        }
    }
}
