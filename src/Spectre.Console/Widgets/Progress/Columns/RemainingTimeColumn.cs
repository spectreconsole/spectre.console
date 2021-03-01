using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A column showing the remaining time of a task.
    /// </summary>
    public sealed class RemainingTimeColumn : ProgressColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemainingTimeColumn"/> class.
        /// </summary>
        public RemainingTimeColumn()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemainingTimeColumn"/> class.
        /// </summary>
        /// <param name="elapsedWhenFinished">Indicates if the elapsed time should display when the task is complete.</param>
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
        /// Gets or sets the text style to render the elapsed time on completion.
        /// </summary>
        public Style FinishedStyle { get; set; } = new Style(foreground: Color.Green);

        /// <inheritdoc/>
        public override IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime)
        {
            if (_elapsedWhenFinished && task.IsFinished)
            {
                var elapsedTime = task.ElapsedTime;
                if (elapsedTime == null)
                {
                    return new Markup("--:--:--");
                }
                else
                {
                    return new Text($"{elapsedTime.Value:hh\\:mm\\:ss}", FinishedStyle ?? Style.Plain);
                }
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
