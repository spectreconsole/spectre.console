using System;
using System.Collections.Generic;
using System.Globalization;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable breakdown chart.
    /// </summary>
    public sealed class BreakdownChart : Renderable, IHasCulture
    {
        /// <summary>
        /// Gets the breakdown chart data.
        /// </summary>
        public List<IBreakdownChartItem> Data { get; }

        /// <summary>
        /// Gets or sets the width of the breakdown chart.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show tags.
        /// </summary>
        public bool ShowTags { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not to show tag values.
        /// </summary>
        public bool ShowTagValues { get; set; } = true;

        /// <summary>
        /// Gets or sets the tag value formatter.
        /// </summary>
        public Func<double, CultureInfo, string>? ValueFormatter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the
        /// chart and tags should be rendered in compact mode.
        /// </summary>
        public bool Compact { get; set; } = true;

        /// <summary>
        /// Gets or sets the <see cref="CultureInfo"/> to use
        /// when rendering values.
        /// </summary>
        /// <remarks>Defaults to invariant culture.</remarks>
        public CultureInfo? Culture { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakdownChart"/> class.
        /// </summary>
        public BreakdownChart()
        {
            Data = new List<IBreakdownChartItem>();
            Culture = CultureInfo.InvariantCulture;
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

            var grid = new Grid().Width(width);
            grid.AddColumn(new GridColumn().NoWrap());

            // Bar
            grid.AddRow(new BreakdownBar(Data)
            {
                Width = width,
            });

            if (ShowTags)
            {
                if (!Compact)
                {
                    grid.AddEmptyRow();
                }

                // Tags
                grid.AddRow(new BreakdownTags(Data)
                {
                    Width = width,
                    Culture = Culture,
                    ShowTagValues = ShowTagValues,
                    ValueFormatter = ValueFormatter,
                });
            }

            return ((IRenderable)grid).Render(context, width);
        }
    }
}
