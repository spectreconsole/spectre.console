using System;
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

        private readonly string _ansiSequence = "⣷⣯⣟⡿⢿⣻⣽⣾";
        private readonly string _asciiSequence = "-\\|/-\\|/";

        /// <inheritdoc/>
        protected internal override int? ColumnWidth => 1;

        /// <inheritdoc/>
        protected internal override bool NoWrap => true;

        /// <summary>
        /// Gets or sets the style of the spinner.
        /// </summary>
        public Style Style { get; set; } = new Style(foreground: Color.Yellow);

        /// <inheritdoc/>
        public override IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime)
        {
            if (!task.IsStarted || task.IsFinished)
            {
                return new Markup(" ");
            }

            var accumulated = task.State.Update<double>(ACCUMULATED, acc => acc + deltaTime.TotalMilliseconds);
            if (accumulated >= 100)
            {
                task.State.Update<double>(ACCUMULATED, _ => 0);
                task.State.Update<int>(INDEX, index => index + 1);
            }

            var useAscii = context.LegacyConsole || !context.Unicode;
            var sequence = useAscii ? _asciiSequence : _ansiSequence;

            var index = task.State.Get<int>(INDEX);
            return new Markup(sequence[index % sequence.Length].ToString(), Style ?? Style.Plain);
        }
    }
}
