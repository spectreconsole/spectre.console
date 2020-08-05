using System.Collections.Generic;
using System.Text;

namespace Spectre.Console.Composition
{
    /// <summary>
    /// Represents something that can be rendered to the console.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Measures the renderable object.
        /// </summary>
        /// <param name="encoding">The encoding to use.</param>
        /// <param name="maxWidth">The maximum allowed width.</param>
        /// <returns>The minimum and maximum width of the object.</returns>
        Measurement Measure(Encoding encoding, int maxWidth);

        /// <summary>
        /// Renders the object.
        /// </summary>
        /// <param name="encoding">The encoding to use.</param>
        /// <param name="width">The width of the render area.</param>
        /// <returns>A collection of segments.</returns>
        IEnumerable<Segment> Render(Encoding encoding, int width);
    }
}
