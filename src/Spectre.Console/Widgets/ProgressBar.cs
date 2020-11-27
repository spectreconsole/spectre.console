using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class ProgressBar : Renderable
    {
        public double Value { get; set; }
        public double MaxValue { get; set; } = 100;

        public int? Width { get; set; }

        public Style CompletedStyle { get; set; } = new Style(foreground: Color.Yellow);
        public Style FinishedStyle { get; set; } = new Style(foreground: Color.Green);
        public Style RemainingStyle { get; set; } = new Style(foreground: Color.Grey);

        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            var width = Math.Min(Width ?? maxWidth, maxWidth);
            return new Measurement(4, width);
        }

        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var width = Math.Min(Width ?? maxWidth, maxWidth);
            var completed = Math.Min(MaxValue, Math.Max(0, Value));

            var token = !context.Unicode || context.LegacyConsole ? '-' : '━';
            var style = completed >= MaxValue ? FinishedStyle : CompletedStyle;

            var bars = Math.Max(0, (int)(width * (completed / MaxValue)));
            yield return new Segment(new string(token, bars), style);

            if (bars < width)
            {
                yield return new Segment(new string(token, width - bars), RemainingStyle);
            }
        }
    }
}
