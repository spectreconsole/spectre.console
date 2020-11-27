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
        /// Gets the requested column width for the column.
        /// </summary>
        protected internal virtual int? ColumnWidth { get; }

        /// <summary>
        /// Gets a renderable representing the column.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <param name="task">The task.</param>
        /// <param name="deltaTime">The elapsed time since last call.</param>
        /// <returns>A renderable representing the column.</returns>
        public abstract IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime);
    }
}
