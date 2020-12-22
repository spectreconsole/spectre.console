using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable (horizontal) bar chart.
    /// </summary>
    public sealed class BarChart : Renderable
    {
        /// <summary>
        /// Gets the bar chart data.
        /// </summary>
        public List<BarChartItem> Data { get; }

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
        /// Initializes a new instance of the <see cref="BarChart"/> class.
        /// </summary>
        public BarChart()
        {
            Data = new List<BarChartItem>();
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var maxValue = Data.Max(item => item.Value);

            var table = new Grid();
            table.Collapse();
            table.AddColumn(new GridColumn().PadRight(2).RightAligned());
            table.AddColumn(new GridColumn().PadLeft(0));
            table.Width = Width;

            if (!string.IsNullOrWhiteSpace(Label))
            {
                table.AddRow(Text.Empty, new Markup(Label).Alignment(LabelAlignment));
            }

            foreach (var item in Data)
            {
                table.AddRow(
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
                    });
            }

            return ((IRenderable)table).Render(context, maxWidth);
        }
    }
}
