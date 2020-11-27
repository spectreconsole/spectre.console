using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A column showing the task description.
    /// </summary>
    public sealed class TaskDescriptionColumn : ProgressColumn
    {
        /// <inheritdoc/>
        public override IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime)
        {
            var text = task.Description?.RemoveNewLines()?.Trim();
            return new Markup(text ?? string.Empty).RightAligned();
        }
    }
}
