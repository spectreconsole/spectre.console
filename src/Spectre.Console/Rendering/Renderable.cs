using System.Collections.Generic;
using System.Diagnostics;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Base class for a renderable object implementing <see cref="IRenderable"/>.
    /// </summary>
    public abstract class Renderable : IRenderable
    {
        /// <inheritdoc/>
        [DebuggerStepThrough]
        Measurement IRenderable.Measure(RenderContext context, int maxWidth)
        {
            return Measure(context, maxWidth);
        }

        /// <inheritdoc/>
        [DebuggerStepThrough]
        IEnumerable<Segment> IRenderable.Render(RenderContext context, int maxWidth)
        {
            return Render(context, maxWidth);
        }

        /// <summary>
        /// Measures the renderable object.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <param name="maxWidth">The maximum allowed width.</param>
        /// <returns>The minimum and maximum width of the object.</returns>
        protected virtual Measurement Measure(RenderContext context, int maxWidth)
        {
            return new Measurement(maxWidth, maxWidth);
        }

        /// <summary>
        /// Renders the object.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <param name="maxWidth">The maximum allowed width.</param>
        /// <returns>A collection of segments.</returns>
        protected abstract IEnumerable<Segment> Render(RenderContext context, int maxWidth);
    }
}
