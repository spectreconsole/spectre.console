using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A column showing task progress in percentage.
    /// </summary>
    public sealed class PercentageColumn : ProgressColumn
    {
        /// <summary>
        /// Gets or sets the style for a non-complete task.
        /// </summary>
        public Style Style { get; set; } = Style.Plain;

        /// <summary>
        /// Gets or sets the style for a completed task.
        /// </summary>
        public Style CompletedStyle { get; set; } = new Style(foreground: Color.Green);

        /// <summary>
        /// Gets or sets the style for a failed task.
        /// </summary>
        public Style FailedStyle { get; set; } = new Style(foreground: Color.Red);

        /// <inheritdoc/>
        public override IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime)
        {
            var percentage = (int)task.Percentage;
            Style style;
            if (percentage == 100)
            {
                style = CompletedStyle;
            }
            else if (task.IsFailed)
            {
                style = FailedStyle;
            }
            else
            {
                style = Style ?? Style.Plain;
            }

            return new Text($"{percentage}%", style).RightAligned();
        }

        /// <inheritdoc/>
        public override int? GetColumnWidth(RenderContext context)
        {
            return 4;
        }
    }
}
