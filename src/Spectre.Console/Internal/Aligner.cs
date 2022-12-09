namespace Spectre.Console;

internal static class Aligner
{
    public static string Align(string text, Justify? alignment, int maxWidth)
    {
        if (alignment == null || alignment == Justify.Left)
        {
            return text;
        }

        var width = Cell.GetCellLength(text);
        if (width >= maxWidth)
        {
            return text;
        }

        switch (alignment)
        {
            case Justify.Right:
                {
                    var diff = maxWidth - width;
                    return new string(' ', diff) + text;
                }

            case Justify.Center:
                {
                    var diff = (maxWidth - width) / 2;

                    var left = new string(' ', diff);
                    var right = new string(' ', diff);

                    // Right side
                    var remainder = (maxWidth - width) % 2;
                    if (remainder != 0)
                    {
                        right += new string(' ', remainder);
                    }

                    return left + text + right;
                }

            default:
                throw new NotSupportedException("Unknown alignment");
        }
    }

    public static void Align<T>(T segments, Justify? alignment, int maxWidth)
        where T : List<Segment>
    {
        if (alignment == null || alignment == Justify.Left)
        {
            return;
        }

        var width = Segment.CellCount(segments);
        if (width >= maxWidth)
        {
            return;
        }

        switch (alignment)
        {
            case Justify.Right:
                {
                    var diff = maxWidth - width;
                    segments.Insert(0, Segment.Padding(diff));
                    break;
                }

            case Justify.Center:
                {
                    // Left side.
                    var diff = (maxWidth - width) / 2;
                    segments.Insert(0, Segment.Padding(diff));

                    // Right side
                    segments.Add(Segment.Padding(diff));
                    var remainder = (maxWidth - width) % 2;
                    if (remainder != 0)
                    {
                        segments.Add(Segment.Padding(remainder));
                    }

                    break;
                }

            default:
                throw new NotSupportedException("Unknown alignment");
        }
    }

    public static void AlignHorizontally<T>(T segments, HorizontalAlignment alignment, int maxWidth)
        where T : List<Segment>
    {
        var width = Segment.CellCount(segments);
        if (width >= maxWidth)
        {
            return;
        }

        switch (alignment)
        {
            case HorizontalAlignment.Left:
                {
                    var diff = maxWidth - width;
                    segments.Add(Segment.Padding(diff));
                    break;
                }

            case HorizontalAlignment.Right:
                {
                    var diff = maxWidth - width;
                    segments.Insert(0, Segment.Padding(diff));
                    break;
                }

            case HorizontalAlignment.Center:
                {
                    // Left side.
                    var diff = (maxWidth - width) / 2;
                    segments.Insert(0, Segment.Padding(diff));

                    // Right side
                    segments.Add(Segment.Padding(diff));
                    var remainder = (maxWidth - width) % 2;
                    if (remainder != 0)
                    {
                        segments.Add(Segment.Padding(remainder));
                    }

                    break;
                }

            default:
                throw new NotSupportedException("Unknown alignment");
        }
    }
}