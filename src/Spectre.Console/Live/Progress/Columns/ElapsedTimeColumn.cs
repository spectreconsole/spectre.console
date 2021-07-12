using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A column showing the elapsed time of a task.
    /// </summary>
    public sealed class ElapsedTimeColumn : ProgressColumn
    {
        /// <inheritdoc/>
        protected internal override bool NoWrap => true;

        /// <summary>
        /// Gets or sets the style of the remaining time text.
        /// </summary>
        public Style Style { get; set; } = new Style(foreground: Color.Blue);

        /// <inheritdoc/>
        public override IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime)
        {
            var elapsed = task.ElapsedTime;
            if (elapsed == null)
            {
                return new Markup("--:--:--");
            }

            if (elapsed.Value.TotalHours > 99)
            {
                return new Markup("**:**:**");
            }

            return new Text($"{elapsed.Value:hh\\:mm\\:ss}", Style ?? Style.Plain);
        }

        /// <inheritdoc/>
        public override int? GetColumnWidth(RenderContext context)
        {
            return 8;
        }
    }
}
