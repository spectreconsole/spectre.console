namespace Spectre.Console;

internal sealed class ProgressBar : Renderable, IHasCulture
{
    private const int PULSESIZE = 20;
    private const int PULSESPEED = 15;

    public double Value { get; set; }
    public double MaxValue { get; set; } = 100;

    public int? Width { get; set; }
    public bool ShowRemaining { get; set; } = true;
    public char UnicodeBar { get; set; } = '━';
    public char AsciiBar { get; set; } = '-';
    public bool ShowValue { get; set; }
    public bool IsIndeterminate { get; set; }
    public CultureInfo? Culture { get; set; }

    public Style CompletedStyle { get; set; } = new Style(foreground: Color.Yellow);
    public Style FinishedStyle { get; set; } = new Style(foreground: Color.Green);
    public Style RemainingStyle { get; set; } = new Style(foreground: Color.Grey);
    public Style IndeterminateStyle { get; set; } = DefaultPulseStyle;

    internal static Style DefaultPulseStyle { get; } = new Style(foreground: Color.DodgerBlue1, background: Color.Grey23);

    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);
        return new Measurement(4, width);
    }

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);
        var completedBarCount = Math.Min(MaxValue, Math.Max(0, Value));
        var isCompleted = completedBarCount >= MaxValue;

        if (IsIndeterminate && !isCompleted)
        {
            foreach (var segment in RenderIndeterminate(options, width))
            {
                yield return segment;
            }

            yield break;
        }

        var bar = !options.Unicode ? AsciiBar : UnicodeBar;
        var style = isCompleted ? FinishedStyle : CompletedStyle;
        var barCount = Math.Max(0, (int)(width * (completedBarCount / MaxValue)));

        // Show value?
        var value = completedBarCount.ToString(Culture ?? CultureInfo.InvariantCulture);
        if (ShowValue)
        {
            barCount = barCount - value.Length - 1;
            barCount = Math.Max(0, barCount);
        }

        if (barCount < 0)
        {
            yield break;
        }

        yield return new Segment(new string(bar, barCount), style);

        if (ShowValue)
        {
            yield return barCount == 0
                ? new Segment(value, style)
                : new Segment(" " + value, style);
        }

        // More space available?
        if (barCount < width)
        {
            var diff = width - barCount;
            if (ShowValue)
            {
                diff = diff - value.Length - 1;
                if (diff <= 0)
                {
                    yield break;
                }
            }

            var legacy = options.ColorSystem == ColorSystem.NoColors || options.ColorSystem == ColorSystem.Legacy;
            var remainingToken = ShowRemaining && !legacy ? bar : ' ';
            yield return new Segment(new string(remainingToken, diff), RemainingStyle);
        }
    }

    private IEnumerable<Segment> RenderIndeterminate(RenderOptions options, int width)
    {
        var bar = options.Unicode ? UnicodeBar.ToString() : AsciiBar.ToString();
        var style = IndeterminateStyle ?? DefaultPulseStyle;

        IEnumerable<Segment> GetPulseSegments()
        {
            // For 1-bit and 3-bit colors, fall back to
            // a simpler versions with only two colors.
            if (options.ColorSystem == ColorSystem.NoColors ||
                options.ColorSystem == ColorSystem.Legacy)
            {
                // First half of the pulse
                var segments = Enumerable.Repeat(new Segment(bar, new Style(style.Foreground)), PULSESIZE / 2);

                // Second half of the pulse
                var legacy = options.ColorSystem == ColorSystem.NoColors || options.ColorSystem == ColorSystem.Legacy;
                var bar2 = legacy ? " " : bar;
                segments = segments.Concat(Enumerable.Repeat(new Segment(bar2, new Style(style.Background)), PULSESIZE - (PULSESIZE / 2)));

                foreach (var segment in segments)
                {
                    yield return segment;
                }

                yield break;
            }

            for (var index = 0; index < PULSESIZE; index++)
            {
                var position = index / (float)PULSESIZE;
                var fade = 0.5f + ((float)Math.Cos(position * Math.PI * 2) / 2.0f);
                var color = style.Foreground.Blend(style.Background, fade);

                yield return new Segment(bar, new Style(foreground: color));
            }
        }

        // Get the pulse segments
        var pulseSegments = GetPulseSegments();
        pulseSegments = pulseSegments.Repeat((width / PULSESIZE) + 2);

        // Repeat the pulse segments
        var currentTime = (DateTime.Now - DateTime.Today).TotalSeconds;
        var offset = (int)(currentTime * PULSESPEED) % PULSESIZE;

        return pulseSegments.Skip(offset).Take(width);
    }
}