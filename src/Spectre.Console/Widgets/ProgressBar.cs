using System;
using System.Collections.Generic;
using System.Globalization;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class ProgressBar : Renderable
    {
        public double Value { get; set; }
        public double MaxValue { get; set; } = 100;

        public int? Width { get; set; }
        public bool ShowRemaining { get; set; } = true;
        public char UnicodeBar { get; set; } = '━';
        public char AsciiBar { get; set; } = '-';
        public bool ShowValue { get; set; }

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

            var token = !context.Unicode || context.LegacyConsole ? AsciiBar : UnicodeBar;
            var style = completed >= MaxValue ? FinishedStyle : CompletedStyle;

            var bars = Math.Max(0, (int)(width * (completed / MaxValue)));

            var value = completed.ToString(CultureInfo.InvariantCulture);
            if (ShowValue)
            {
                bars = bars - value.Length - 1;
            }

            yield return new Segment(new string(token, bars), style);

            if (ShowValue)
            {
                yield return new Segment(" " + value, style);
            }

            if (bars < width)
            {
                var diff = width - bars;
                if (ShowValue)
                {
                    diff = diff - value.Length - 1;
                    if (diff <= 0)
                    {
                        yield break;
                    }
                }

                var remainingToken = ShowRemaining ? token : ' ';
                yield return new Segment(new string(remainingToken, diff), RemainingStyle);
            }
        }
    }
}
