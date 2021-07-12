using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a progress column.
    /// </summary>
    public abstract class ProgressColumn
    {
        /// <summary>
        /// Gets a value indicating whether or not content should not wrap.
        /// </summary>
        protected internal virtual bool NoWrap { get; }

        /// <summary>
        /// Gets a renderable representing the column.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <param name="task">The task.</param>
        /// <param name="deltaTime">The elapsed time since last call.</param>
        /// <returns>A renderable representing the column.</returns>
        public abstract IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime);

        /// <summary>
        /// Gets the width of the column.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The width of the column, or <c>null</c> to calculate.</returns>
        public virtual int? GetColumnWidth(RenderContext context)
        {
            return null;
        }
    }
}
