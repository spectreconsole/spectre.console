using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A column showing the remaining time of a task.
    /// </summary>
    public sealed class RemainingTimeColumn : ProgressColumn
    {
        public RemainingTimeColumn()
            : this(false)
        {
        }

        public RemainingTimeColumn(bool elapsedWhenFinished)
        {
            _elapsedWhenFinished = elapsedWhenFinished;
        }

        private readonly bool _elapsedWhenFinished;

        /// <inheritdoc/>
        protected internal override bool NoWrap => true;

        /// <summary>
        /// Gets or sets the style of the remaining time text.
        /// </summary>
        public Style Style { get; set; } = new Style(foreground: Color.Blue);

        /// <summary>
        /// Gets or sets the style of the remaining time text.
        /// </summary>
        public Style FinishedStyle { get; set; } = new Style(foreground: Color.Green);

        /// <inheritdoc/>
        public override IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime)
        {
            if (_elapsedWhenFinished && task.IsFinished)
            {
                return new Text($"{task.ElapsedTime:hh\\:mm\\:ss}", FinishedStyle ?? Style.Plain);
            }
            else
            {
                var remaining = task.RemainingTime;
                if (remaining == null)
                {
                    return new Markup("--:--:--");
                }

                if (remaining.Value.TotalHours > 99)
                {
                    return new Markup("**:**:**");
                }

                return new Text($"{remaining.Value:hh\\:mm\\:ss}", Style ?? Style.Plain);
            }
        }

        /// <inheritdoc/>
        public override int? GetColumnWidth(RenderContext context)
        {
            return 8;
        }
    }
}
