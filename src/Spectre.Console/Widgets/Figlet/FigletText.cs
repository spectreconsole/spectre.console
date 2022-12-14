namespace Spectre.Console;

/// <summary>
/// Represents text rendered with a FIGlet font.
/// </summary>
public sealed class FigletText : Renderable, IHasJustification
{
    private readonly FigletFont _font;
    private readonly string _text;

    /// <summary>
    /// Gets or sets the color of the text.
    /// </summary>
    public Color? Color { get; set; }

    /// <inheritdoc/>
    public Justify? Justification { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// the right side should be padded.
    /// </summary>
    /// <remarks>Defaults to <c>false</c>.</remarks>
    public bool Pad { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FigletText"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    public FigletText(string text)
        : this(FigletFont.Default, text)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FigletText"/> class.
    /// </summary>
    /// <param name="font">The FIGlet font to use.</param>
    /// <param name="text">The text.</param>
    public FigletText(FigletFont font, string text)
    {
        _font = font ?? throw new ArgumentNullException(nameof(font));
        _text = text ?? throw new ArgumentNullException(nameof(text));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var style = new Style(Color ?? Console.Color.Default);
        var alignment = Justification ?? Console.Justify.Left;

        foreach (var row in GetRows(maxWidth))
        {
            for (var index = 0; index < _font.Height; index++)
            {
                var line = new Segment(string.Concat(row.Select(x => x.Lines[index])), style);

                var lineWidth = line.CellCount();
                if (alignment == Console.Justify.Left)
                {
                    yield return line;

                    if (lineWidth < maxWidth && Pad)
                    {
                        yield return Segment.Padding(maxWidth - lineWidth);
                    }
                }
                else if (alignment == Console.Justify.Center)
                {
                    var left = (maxWidth - lineWidth) / 2;
                    var right = left + ((maxWidth - lineWidth) % 2);

                    yield return Segment.Padding(left);
                    yield return line;

                    if (Pad)
                    {
                        yield return Segment.Padding(right);
                    }
                }
                else if (alignment == Console.Justify.Right)
                {
                    if (lineWidth < maxWidth)
                    {
                        yield return Segment.Padding(maxWidth - lineWidth);
                    }

                    yield return line;
                }

                yield return Segment.LineBreak;
            }
        }
    }

    private List<List<FigletCharacter>> GetRows(int maxWidth)
    {
        var result = new List<List<FigletCharacter>>();
        var words = _text.SplitWords(StringSplitOptions.None);

        var totalWidth = 0;
        var line = new List<FigletCharacter>();

        foreach (var word in words)
        {
            // Does the whole word fit?
            var width = _font.GetWidth(word);
            if (width + totalWidth < maxWidth)
            {
                // Add it to the line
                line.AddRange(_font.GetCharacters(word));
                totalWidth += width;
            }
            else
            {
                // Does it fit on its own line?
                if (width < maxWidth)
                {
                    // Flush the line
                    result.Add(line);
                    line = new List<FigletCharacter>();
                    totalWidth = 0;

                    line.AddRange(_font.GetCharacters(word));
                    totalWidth += width;
                }
                else
                {
                    // We need to split it up.
                    var queue = new Queue<FigletCharacter>(_font.GetCharacters(word));
                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();
                        if (totalWidth + current.Width > maxWidth)
                        {
                            // Flush the line
                            result.Add(line);
                            line = new List<FigletCharacter>();
                            totalWidth = 0;
                        }

                        line.Add(current);
                        totalWidth += current.Width;
                    }
                }
            }
        }

        if (line.Count > 0)
        {
            result.Add(line);
        }

        return result;
    }
}