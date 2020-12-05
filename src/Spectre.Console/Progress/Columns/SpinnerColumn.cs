using System;
using System.Linq;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A column showing a spinner.
    /// </summary>
    public sealed class SpinnerColumn : ProgressColumn
    {
        private const string ACCUMULATED = "SPINNER_ACCUMULATED";
        private const string INDEX = "SPINNER_INDEX";

        private readonly ProgressSpinner _spinner;
        private int? _maxLength;

        /// <inheritdoc/>
        protected internal override int? ColumnWidth => 1;

        /// <inheritdoc/>
        protected internal override bool NoWrap => true;

        /// <summary>
        /// Gets or sets the style of the spinner.
        /// </summary>
        public Style Style { get; set; } = new Style(foreground: Color.Yellow);

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnerColumn"/> class.
        /// </summary>
        public SpinnerColumn()
            : this(ProgressSpinner.Known.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnerColumn"/> class.
        /// </summary>
        /// <param name="spinner">The spinner to use.</param>
        public SpinnerColumn(ProgressSpinner spinner)
        {
            _spinner = spinner ?? throw new ArgumentNullException(nameof(spinner));
        }

        /// <inheritdoc/>
        public override IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime)
        {
            var useAscii = (context.LegacyConsole || !context.Unicode) && _spinner.IsUnicode;
            var spinner = useAscii ? ProgressSpinner.Known.Ascii : _spinner;

            if (!task.IsStarted || task.IsFinished)
            {
                if (_maxLength == null)
                {
                    _maxLength = _spinner.Frames.Max(frame => Cell.GetCellLength(context, frame));
                }

                return new Markup(new string(' ', _maxLength.Value));
            }

            var accumulated = task.State.Update<double>(ACCUMULATED, acc => acc + deltaTime.TotalMilliseconds);
            if (accumulated >= _spinner.Interval.TotalMilliseconds)
            {
                task.State.Update<double>(ACCUMULATED, _ => 0);
                task.State.Update<int>(INDEX, index => index + 1);
            }

            var index = task.State.Get<int>(INDEX);
            return new Markup(spinner.Frames[index % spinner.Frames.Count], Style ?? Style.Plain);
        }
    }
}
