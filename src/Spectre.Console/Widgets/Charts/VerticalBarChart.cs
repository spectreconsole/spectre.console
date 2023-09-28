namespace Spectre.Console;

/// <summary>
/// A renderable vertical bar chart.
/// </summary>
public sealed class VerticalBarChart : Renderable
{
    /// <summary>
    /// Gets the vertical bar chart data.
    /// </summary>
    public List<double> Data { get; }

    /// <summary>
    /// Gets the item color.
    /// </summary>
    public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets the width of the vertical bar chart.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the width of the vertical bar chart.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VerticalBarChart"/> class.
    /// </summary>
    public VerticalBarChart()
    {
        Data = new List<double>();
        Height = 1;
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        // TODO: adjust for width
        var width = Data.Count;
        return new Measurement(width, width);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        return ComputeCharacter(
            (celValue, dataValue) =>
            {
                var character = (dataValue, celValue, celValue - dataValue) switch
                {
                    (0, _, _) => ' ',
                    ( < 0, > 0, _) => ' ',
                    ( < 0, _, > .5) => '█',
                    ( < 0, _, > .25) => '▀',
                    ( > 0, < 0, _) => ' ',
                    ( > 0, _, < -0.5) => '█',
                    ( > 0, _, < -0.25) => '▄',
                    (_, _, _) => ' '
                };

                return character;
            });
    }

    private IEnumerable<Segment> ComputeCharacter(Func<int, double, char> compute)
    {
        var maxValue = Data.Max();
        var minValue = -1 * Math.Min(0, Data.Min());
        var totalRange = maxValue + minValue;

        var positiveRows = (int)Math.Ceiling(maxValue * Height / totalRange);
        var negativeRows = Height - positiveRows;

        var len = Data.Count; // TODO CHANGE TO WIDTH

        var lines = Enumerable
            .Range(0, Height)
            .Select(line => new string(' ', len).ToCharArray())
            .ToArray();

        for (var col = 0; col < len; col++)
        {
            var value = Data[col];
            var normalizedValue = value == 0
                ? 0
                : value >= 0
                    ? value * positiveRows / maxValue
                    : value * negativeRows / minValue;

            for (var row = 0; row < Height; row++)
            {
                var normalizedRow = normalizedValue > 0 ? row - negativeRows : row - negativeRows + 1;
                lines[row][col] = compute(normalizedRow, normalizedValue);
            }
        }

        return lines
            .Select((l, i) => CreateSegment(l, i != 0))
            .Reverse(); // reverse as the computation was done from top to bottom

        Segment CreateSegment(char[] chars, bool addNewLine)
            => new(addNewLine ? new string(chars) + "\n" : new string(chars), new Style(Color));
    }
}