using System;
using System.Globalization;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A column showing transfer speed.
    /// </summary>
    public sealed class TransferSpeedColumn : ProgressColumn
    {
        /// <summary>
        /// Gets or sets the <see cref="CultureInfo"/> to use.
        /// </summary>
        public CultureInfo? Culture { get; set; }

        /// <inheritdoc/>
        public override IRenderable Render(RenderContext context, ProgressTask task, TimeSpan deltaTime)
        {
            if (task.Speed == null)
            {
                return new Text("?/s");
            }

            var size = new FileSize(task.Speed.Value);
            return new Markup(string.Format("{0}/s", size.ToString(suffix: true, Culture)));
        }
    }
}
