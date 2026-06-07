namespace Spectre.Console;

/// <summary>
/// Represents a border.
/// </summary>
public abstract partial class BoxBorder
{
    /// <summary>
    /// Gets an invisible border.
    /// </summary>
    public static BoxBorder None { get; } = new NoBoxBorder();

    /// <summary>
    /// Gets an ASCII border.
    /// </summary>
    public static BoxBorder Ascii { get; } = new AsciiBoxBorder();

    /// <summary>
    /// Gets a double border.
    /// </summary>
    [SuppressMessage("Naming", "CA1720:Identifier contains type name")]
    public static BoxBorder Double { get; } = new DoubleBoxBorder();

    /// <summary>
    /// Gets a heavy border.
    /// </summary>
    public static BoxBorder Heavy { get; } = new HeavyBoxBorder();

    /// <summary>
    /// Gets a rounded border.
    /// </summary>
    public static BoxBorder Rounded { get; } = new RoundedBoxBorder();

    /// <summary>
    /// Gets a square border.
    /// </summary>
    public static BoxBorder Square { get; } = new SquareBoxBorder();

    /// <summary>
    /// Gets a border with heavy horizontal edges and light vertical edges.
    /// </summary>
    public static BoxBorder HeavyHorizontal { get; } = new HeavyHorizontalBoxBorder();

    /// <summary>
    /// Gets a border with heavy vertical edges and light horizontal edges.
    /// </summary>
    public static BoxBorder HeavyVertical { get; } = new HeavyVerticalBoxBorder();

    /// <summary>
    /// Gets a border with double horizontal edges and single vertical edges.
    /// </summary>
    public static BoxBorder DoubleHorizontal { get; } = new DoubleHorizontalBoxBorder();

    /// <summary>
    /// Gets a border with double vertical edges and single horizontal edges.
    /// </summary>
    public static BoxBorder DoubleVertical { get; } = new DoubleVerticalBoxBorder();

    /// <summary>
    /// Gets a dashed border with square corners.
    /// </summary>
    public static BoxBorder Dashed { get; } = new DashedBoxBorder();

    /// <summary>
    /// Gets a dashed border with rounded corners.
    /// </summary>
    public static BoxBorder RoundedDashed { get; } = new RoundedDashedBoxBorder();

    /// <summary>
    /// Gets a heavy dashed border.
    /// </summary>
    public static BoxBorder HeavyDashed { get; } = new HeavyDashedBoxBorder();

    /// <summary>
    /// Gets a dotted border with square corners.
    /// </summary>
    public static BoxBorder Dotted { get; } = new DottedBoxBorder();

    /// <summary>
    /// Gets a dotted border with rounded corners.
    /// </summary>
    public static BoxBorder RoundedDotted { get; } = new RoundedDottedBoxBorder();

    /// <summary>
    /// Gets a heavy dotted border.
    /// </summary>
    public static BoxBorder HeavyDotted { get; } = new HeavyDottedBoxBorder();

    /// <summary>
    /// Gets a wide-dashed border with square corners.
    /// </summary>
    public static BoxBorder DashedWide { get; } = new DashedWideBoxBorder();

    /// <summary>
    /// Gets a wide-dashed border with rounded corners.
    /// </summary>
    public static BoxBorder RoundedDashedWide { get; } = new RoundedDashedWideBoxBorder();

    /// <summary>
    /// Gets a heavy wide-dashed border.
    /// </summary>
    public static BoxBorder HeavyDashedWide { get; } = new HeavyDashedWideBoxBorder();

    /// <summary>
    /// Gets a "near" border that hugs the content using thin block elements.
    /// </summary>
    public static BoxBorder Near { get; } = new NearBoxBorder();

    /// <summary>
    /// Gets a beveled border using thin block edges and diagonal corners.
    /// </summary>
    public static BoxBorder Beveled { get; } = new BeveledBoxBorder();

    /// <summary>
    /// Gets the horizontal variant of the McGugan border.
    /// </summary>
    public static BoxBorder McGuganHorizontal { get; } = new McGuganHorizontalBoxBorder();

    /// <summary>
    /// Gets the vertical variant of the McGugan border.
    /// </summary>
    public static BoxBorder McGuganVertical { get; } = new McGuganVerticalBoxBorder();
}