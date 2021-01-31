using System;
using System.Collections.Generic;
using System.Globalization;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class BreakdownTags : Renderable
    {
        private readonly List<IBreakdownChartItem> _data;

        public int? Width { get; set; }
        public CultureInfo? Culture { get; set; }
        public bool ShowPercentages { get; set; }
        public bool ShowTagValues { get; set; } = true;

        public BreakdownTags(List<IBreakdownChartItem> data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            var width = Math.Min(Width ?? maxWidth, maxWidth);
            return new Measurement(width, width);
        }

        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var culture = Culture ?? CultureInfo.InvariantCulture;

            var panels = new List<Panel>();
            foreach (var item in _data)
            {
                var panel = new Panel(GetTag(item, culture));
                panel.Inline = true;
                panel.Padding = new Padding(0, 0);
                panel.NoBorder();

                panels.Add(panel);
            }

            foreach (var segment in ((IRenderable)new Columns(panels).Padding(0, 0)).Render(context, maxWidth))
            {
                yield return segment;
            }
        }

        private string GetTag(IBreakdownChartItem item, CultureInfo culture)
        {
            return string.Format(
                culture, "[{0}]■[/] {1}",
                item.Color.ToMarkup() ?? "default",
                FormatValue(item, culture)).Trim();
        }

        private string FormatValue(IBreakdownChartItem item, CultureInfo culture)
        {
            if (ShowTagValues)
            {
                return string.Format(culture, "{0} [grey]{1}{2}[/]",
                    item.Label.EscapeMarkup(), item.Value,
                    ShowPercentages ? "%" : string.Empty);
            }

            return item.Label.EscapeMarkup();
        }
    }
}
