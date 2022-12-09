namespace Spectre.Console;

/// <summary>
/// Represents a renderable used to align content.
/// </summary>
public sealed class Align : Renderable
{
    private readonly IRenderable _renderable;

    /// <summary>
    /// Gets or sets the horizontal alignment.
    /// </summary>
    public HorizontalAlignment Horizontal { get; set; } = HorizontalAlignment.Left;

    /// <summary>
    /// Gets or sets the vertical alignment.
    /// </summary>
    public VerticalAlignment? Vertical { get; set; }

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Align"/> class.
    /// </summary>
    /// <param name="renderable">The renderable to align.</param>
    /// <param name="horizontal">The horizontal alignment.</param>
    /// <param name="vertical">The vertical alignment, or <c>null</c> if none.</param>
    public Align(IRenderable renderable, HorizontalAlignment horizontal, VerticalAlignment? vertical = null)
    {
        _renderable = renderable ?? throw new ArgumentNullException(nameof(renderable));

        Horizontal = horizontal;
        Vertical = vertical;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Align"/> class that is left aligned.
    /// </summary>
    /// <param name="renderable">The <see cref="IRenderable"/> to align.</param>
    /// <param name="vertical">The vertical alignment, or <c>null</c> if none.</param>
    /// <returns>A new <see cref="Align"/> object.</returns>
    public static Align Left(IRenderable renderable, VerticalAlignment? vertical = null)
    {
        return new Align(renderable, HorizontalAlignment.Left, vertical);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Align"/> class that is center aligned.
    /// </summary>
    /// <param name="renderable">The <see cref="IRenderable"/> to align.</param>
    /// <param name="vertical">The vertical alignment, or <c>null</c> if none.</param>
    /// <returns>A new <see cref="Align"/> object.</returns>
    public static Align Center(IRenderable renderable, VerticalAlignment? vertical = null)
    {
        return new Align(renderable, HorizontalAlignment.Center, vertical);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Align"/> class that is right aligned.
    /// </summary>
    /// <param name="renderable">The <see cref="IRenderable"/> to align.</param>
    /// <param name="vertical">The vertical alignment, or <c>null</c> if none.</param>
    /// <returns>A new <see cref="Align"/> object.</returns>
    public static Align Right(IRenderable renderable, VerticalAlignment? vertical = null)
    {
        return new Align(renderable, HorizontalAlignment.Right, vertical);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var rendered = _renderable.Render(options with { Height = null }, maxWidth);
        var lines = Segment.SplitLines(rendered);

        var width = Math.Min(Width ?? maxWidth, maxWidth);
        var height = Height ?? options.Height;

        var blank = new SegmentLine(new[] { new Segment(new string(' ', width)) });

        // Align vertically
        if (Vertical != null && height != null)
        {
            switch (Vertical)
            {
                case VerticalAlignment.Top:
                    {
                        var diff = height - lines.Count;
                        for (var i = 0; i < diff; i++)
                        {
                            lines.Add(blank);
                        }

                        break;
                    }

                case VerticalAlignment.Middle:
                    {
                        var top = (height - lines.Count) / 2;
                        var bottom = height - top - lines.Count;

                        for (var i = 0; i < top; i++)
                        {
                            lines.Insert(0, blank);
                        }

                        for (var i = 0; i < bottom; i++)
                        {
                            lines.Add(blank);
                        }

                        break;
                    }

                case VerticalAlignment.Bottom:
                    {
                        var diff = height - lines.Count;
                        for (var i = 0; i < diff; i++)
                        {
                            lines.Insert(0, blank);
                        }

                        break;
                    }

                default:
                    throw new NotSupportedException("Unknown vertical alignment");
            }
        }

        // Align horizontally
        foreach (var line in lines)
        {
            Aligner.AlignHorizontally(line, Horizontal, width);
        }

        return new SegmentLineEnumerator(lines);
    }
}
