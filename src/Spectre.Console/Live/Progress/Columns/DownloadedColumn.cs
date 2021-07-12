using System;
using System.Globalization;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A column showing download progress.
    /// </summary>
    public sealed class DownloadedColumn : ProgressColumn
    {
        /// <summary>
        /// Gets or sets the <see cref="CultureInfo"/> to use.
        /// </summary>
        public CultureInfo? Culture { get; set; }

        /// <inheritdoc/>
        public override IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime)
        {
            var total = new FileSize(task.MaxValue);

            if (task.IsFinished)
            {
                return new Markup(string.Format(
                    "[green]{0} {1}[/]",
                    total.Format(Culture),
                    total.Suffix));
            }
            else
            {
                var downloaded = new FileSize(task.Value, total.Unit);

                return new Markup(string.Format(
                    "{0}[grey]/[/]{1} [grey]{2}[/]",
                    downloaded.Format(Culture),
                    total.Format(Culture),
                    total.Suffix));
            }
        }
    }
}
