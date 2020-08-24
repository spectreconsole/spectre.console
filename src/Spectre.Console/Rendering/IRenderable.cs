using System.Collections.Generic;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents something that can be rendered to the console.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Measures the renderable object.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <param name="maxWidth">The maximum allowed width.</param>
        /// <returns>The minimum and maximum width of the object.</returns>
        Measurement Measure(RenderContext context, int maxWidth);

        /// <summary>
        /// Renders the object.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <param name="maxWidth">The maximum allowed width.</param>
        /// <returns>A collection of segments.</returns>
        IEnumerable<Segment> Render(RenderContext context, int maxWidth);
    }
}
