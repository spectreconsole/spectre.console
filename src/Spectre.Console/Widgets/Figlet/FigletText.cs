namespace Spectre.Console;

/// <summary>
/// Represents text rendered with a Figlet font.
/// </summary>
public sealed class FigletText : Renderable, IHasJustification
{
    private readonly FigletFont _font;
    private readonly string _text;

    private const string OverscoreChars = @"|/\[]{}()<>";

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
    /// Gets or sets the Figlet layout mode.
    /// </summary>
    /// <remarks>Defaults to <c>FullSize</c>.</remarks>
    public FigletLayoutMode LayoutMode { get; set; }

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
    /// <param name="font">The Figlet font to use.</param>
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
        var alignment = Justification ?? Justify.Left;

        foreach (var row in GetRows(maxWidth))
        {
            (int Amount, char[]? MergeChars)[]? junctions = null;
            if (LayoutMode is FigletLayoutMode.Fitted or FigletLayoutMode.Smushed
                && row.Count > 1)
            {
                junctions = new (int, char[]?)[row.Count - 1];
                var smushRules = LayoutMode == FigletLayoutMode.Smushed ? _font.SmushingRules : 0;
                for (var i = 0; i < row.Count - 1; i++)
                {
                    junctions[i] = ComputeJunction(row[i], row[i + 1], LayoutMode, smushRules);
                }
            }

            for (var index = 0; index < _font.Height; index++)
            {
                var lineText = junctions != null
                    ? BuildLine(row, index, junctions)
                    : string.Concat(row.Select(x => x.Lines[index]));
                var line = new Segment(lineText, style);

                var lineWidth = line.CellCount();
                if (alignment == Justify.Left)
                {
                    yield return line;

                    if (lineWidth < maxWidth && Pad)
                    {
                        yield return Segment.Padding(maxWidth - lineWidth);
                    }
                }
                else if (alignment == Justify.Center)
                {
                    var left = Math.Max(0, maxWidth - lineWidth) / 2;
                    var right = left + (Math.Max(0, maxWidth - lineWidth) % 2);

                    yield return Segment.Padding(left);
                    yield return line;

                    if (Pad)
                    {
                        yield return Segment.Padding(right);
                    }
                }
                else if (alignment == Justify.Right)
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

    private static (int Amount, char[]? MergeChars) ComputeJunction(
        FigletCharacter left, FigletCharacter right,
        FigletLayoutMode mode, int smushRules)
    {
        // Compute the fit amount: minimum across all rows of
        // (trailing spaces of left + leading spaces of right)
        var fit = int.MaxValue;
        for (var y = 0; y < left.Height; y++)
        {
            var trailing = left.Lines[y].Length - left.Lines[y].TrimEnd(' ').Length;
            var leading = right.Lines[y].Length - right.Lines[y].TrimStart(' ').Length;
            fit = Math.Min(fit, trailing + leading);
        }

        if (fit == int.MaxValue)
        {
            fit = 0;
        }

        // Never fit or smush the input space character; preserve natural spacing
        if (left.Code == 32 || right.Code == 32)
        {
            return (0, null);
        }

        if (mode != FigletLayoutMode.Smushed)
        {
            return (fit, null);
        }

        // Try smushing: go one column further than
        // fitting by merging the boundary characters
        var mergeChars = new char[left.Height];
        for (var i = 0; i < left.Height; i++)
        {
            var leftLine = left.Lines[i];
            var rightLine = right.Lines[i];
            var trailing = leftLine.Length - leftLine.TrimEnd(' ').Length;

            var trimFromLeft = Math.Min(trailing, fit);
            var trimFromRight = fit - trimFromLeft;

            var leftBound = leftLine.Length - trimFromLeft - 1;
            var rightBound = trimFromRight;

            var leftChar = (leftBound >= 0) ? leftLine[leftBound] : ' ';
            var rightChar = (rightBound < rightLine.Length) ? rightLine[rightBound] : ' ';

            // Both sides are space?
            if (leftChar == ' ' && rightChar == ' ')
            {
                mergeChars[i] = ' ';
                continue;
            }

            var merged = SmushChars(leftChar, rightChar, smushRules);
            if (merged == null)
            {
                return (fit, null);
            }

            mergeChars[i] = merged.Value;
        }

        return (fit + 1, mergeChars);
    }

    private static char? SmushChars(char left, char right, int rules)
    {
        switch (left)
        {
            // Both spaces: nothing to smush
            case ' ' when right == ' ':
                return null;
            // Space is transparent: the non-space side wins
            case ' ':
                return right;
        }

        if (right == ' ')
        {
            return left;
        }

        // Universal smushing
        if (rules == 0)
        {
            return right;
        }

        // Rule 1 (Equal character)
        if ((rules & 1) != 0 && left == right)
        {
            return left;
        }

        // Rule 2 (Underscore)
        if ((rules & 2) != 0)
        {
            if (left == '_' && OverscoreChars.Contains(right))
            {
                return right;
            }

            if (right == '_' && OverscoreChars.Contains(left))
            {
                return left;
            }
        }

        // Rule 3 (Hierarchy)
        if ((rules & 4) != 0)
        {
            static int GetHierarchyClass(char c) => c switch
            {
                '|' => 1,
                '/' or '\\' => 2,
                '[' or ']' => 3,
                '{' or '}' => 4,
                '(' or ')' => 5,
                '<' or '>' => 6,
                _ => 0,
            };

            var leftClass = GetHierarchyClass(left);
            var rightClass = GetHierarchyClass(right);
            if (leftClass != 0 && rightClass != 0 && leftClass != rightClass)
            {
                return leftClass > rightClass ? left : right;
            }
        }

        // Rule 4 (Opposite pairs)
        if ((rules & 8) != 0)
        {
            switch (left)
            {
                case '[' when right == ']':
                case ']' when right == '[':
                case '{' when right == '}':
                case '}' when right == '{':
                case '(' when right == ')':
                case ')' when right == '(':
                    return '|';
            }
        }

        // Rule 5 (Big X)
        if ((rules & 16) != 0)
        {
            switch (left)
            {
                case '/' when right == '\\':
                    return '|';
                case '\\' when right == '/':
                    return 'Y';
                case '>' when right == '<':
                    return 'X';
            }
        }

        // No smushing rule applies
        return null;
    }

    private static string BuildLine(
        IReadOnlyList<FigletCharacter> row,
        int lineIndex,
        IReadOnlyList<(int Amount, char[]? MergeChars)> junctions)
    {
        if (row.Count == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder(row[0].Lines[lineIndex]);

        for (var rowIndex = 1; rowIndex < row.Count; rowIndex++)
        {
            var (amount, mergeChars) = junctions[rowIndex - 1];
            var rightLine = row[rowIndex].Lines[lineIndex];

            var current = sb.ToString();
            var trailingSpaces = current.Length - current.TrimEnd(' ').Length;

            if (mergeChars != null)
            {
                // Smushing: amount = fit + 1. Remove fit chars via normal trim,
                // then swap boundary chars for the merged char
                var fit = amount - 1;
                var trimFromLeft = Math.Min(trailingSpaces, fit);
                var trimFromRight = fit - trimFromLeft;

                // Remove trailing spaces + boundary char from accumulated left
                var removeFromLeft = Math.Min(trimFromLeft + 1, sb.Length);
                sb.Remove(sb.Length - removeFromLeft, removeFromLeft);

                sb.Append(mergeChars[lineIndex]);

                // Skip leading spaces + boundary char from right
                var skipFromRight = trimFromRight + 1;
                if (skipFromRight < rightLine.Length)
                {
                    sb.Append(rightLine[skipFromRight..]);
                }
            }
            else
            {
                // Fitting: remove trailing/leading spaces to bring characters together
                var trimFromLeft = Math.Min(trailingSpaces, amount);
                var trimFromRight = amount - trimFromLeft;

                if (trimFromLeft > 0)
                {
                    sb.Remove(sb.Length - trimFromLeft, trimFromLeft);
                }

                sb.Append(trimFromRight > 0 ? rightLine[trimFromRight..] : rightLine);
            }
        }

        return sb.ToString();
    }

    private List<List<FigletCharacter>> GetRows(int maxWidth)
    {
        var result = new List<List<FigletCharacter>>();
        var words = _text.SplitWords();

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
                    line = [];
                    totalWidth = 0;

                    line.AddRange(_font.GetCharacters(word));
                    totalWidth += width;
                }
                else
                {
                    // We need to split it up
                    var queue = new Queue<FigletCharacter>(_font.GetCharacters(word));
                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();
                        if (totalWidth + current.Width > maxWidth)
                        {
                            // Flush the line
                            result.Add(line);
                            line = [];
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

/// <summary>
/// Contains extension methods for <see cref="FigletText"/>.
/// </summary>
public static class FigletTextExtensions
{
    /// <summary>
    /// Sets the color of the Figlet text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="color">The color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static FigletText Color(this FigletText text, Color? color)
    {
        ArgumentNullException.ThrowIfNull(text);

        text.Color = color ?? Console.Color.Default;
        return text;
    }

    /// <summary>
    /// Sets the Figlet layout mode.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="mode">The layout mode.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static FigletText LayoutMode(this FigletText text, FigletLayoutMode mode)
    {
        ArgumentNullException.ThrowIfNull(text);

        text.LayoutMode = mode;
        return text;
    }

    /// <summary>
    /// Sets the Figlet layout mode to <c>FullSize</c>.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static FigletText FullSize(this FigletText text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return LayoutMode(text, FigletLayoutMode.FullSize);
    }

    /// <summary>
    /// Sets the Figlet layout mode to <c>Fitted</c>.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static FigletText Fitted(this FigletText text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return LayoutMode(text, FigletLayoutMode.Fitted);
    }

    /// <summary>
    /// Sets the Figlet layout mode to <c>Smushed</c>.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static FigletText Smushed(this FigletText text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return LayoutMode(text, FigletLayoutMode.Smushed);
    }
}