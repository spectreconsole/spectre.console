using System.Collections.Generic;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents a render hook.
    /// </summary>
    public interface IRenderHook
    {
        /// <summary>
        /// Processes the specified renderables.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <param name="renderables">The renderables to process.</param>
        /// <returns>The processed renderables.</returns>
        IEnumerable<IRenderable> Process(RenderContext context, IEnumerable<IRenderable> renderables);
    }
}
