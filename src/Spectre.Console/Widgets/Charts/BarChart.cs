using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable (horizontal) bar chart.
    /// </summary>
    public sealed class BarChart : Renderable, IHasCulture
    {
        /// <summary>
        /// Gets the bar chart data.
        /// </summary>
        public List<IBarChartItem> Data { get; }

        /// <summary>
        /// Gets or sets the width of the bar chart.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the bar chart label.
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the bar chart label alignment.
        /// </summary>
        public Justify? LabelAlignment { get; set; } = Justify.Center;

        /// <summary>
        /// Gets or sets a value indicating whether or not
        /// values should be shown next to each bar.
        /// </summary>
        public bool ShowValues { get; set; } = true;

        /// <summary>
        /// Gets or sets the culture that's used to format values.
        /// </summary>
        /// <remarks>Defaults to invariant culture.</remarks>
        public CultureInfo? Culture { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarChart"/> class.
        /// </summary>
        public BarChart()
        {
            Data = new List<IBarChartItem>();
        }

        /// <inheritdoc/>
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            var width = Math.Min(Width ?? maxWidth, maxWidth);
            return new Measurement(width, width);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var width = Math.Min(Width ?? maxWidth, maxWidth);
            var maxValue = Data.Max(item => item.Value);

            var grid = new Grid();
            grid.Collapse();
            grid.AddColumn(new GridColumn().PadRight(2).RightAligned());
            grid.AddColumn(new GridColumn().PadLeft(0));
            grid.Width = width;

            if (!string.IsNullOrWhiteSpace(Label))
            {
                grid.AddRow(Text.Empty, new Markup(Label).Alignment(LabelAlignment));
            }

            foreach (var item in Data)
            {
                grid.AddRow(
                    new Markup(item.Label),
                    new ProgressBar()
                    {
                        Value = item.Value,
                        MaxValue = maxValue,
                        ShowRemaining = false,
                        CompletedStyle = new Style().Foreground(item.Color ?? Color.Default),
                        FinishedStyle = new Style().Foreground(item.Color ?? Color.Default),
                        UnicodeBar = '█',
                        AsciiBar = '█',
                        ShowValue = ShowValues,
                        Culture = Culture,
                    });
            }

            return ((IRenderable)grid).Render(context, width);
        }
    }
}
