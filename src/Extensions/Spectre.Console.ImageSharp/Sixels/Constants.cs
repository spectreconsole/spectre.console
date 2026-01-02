namespace Spectre.Console.ImageSharp.Sixels;

/// <summary>
/// Sixel terminal compatibility helpers.
/// </summary>
public static class Constants
{
    /// <summary>
    /// The character to use when entering a terminal escape code sequence.
    /// </summary>
    public const string ESC = "\u001b";

    /// <summary>
    /// The character to indicate the start of a sixel color palette entry or to switch to a new color.
    /// https://vt100.net/docs/vt3xx-gp/chapter14.html#S14.3.3.
    /// </summary>
    public const char SIXELCOLOR = '#';

    /// <summary>
    /// The character to use when a sixel is empty/transparent.
    /// ? (hex 3F) represents the binary value 000000.
    /// https://vt100.net/docs/vt3xx-gp/chapter14.html#S14.2.1.
    /// </summary>
    public const char SIXELEMPTY = '?';

    /// <summary>
    /// The character to use when entering a repeated sequence of a color in a sixel.
    /// https://vt100.net/docs/vt3xx-gp/chapter14.html#S14.3.1.
    /// </summary>
    public const char SIXELREPEAT = '!';

    /// <summary>
    /// The character to use when moving to the next line in a sixel.
    /// https://vt100.net/docs/vt3xx-gp/chapter14.html#S14.3.5.
    /// </summary>
    public const char SIXELDECGNL = '-';

    /// <summary>
    /// The character to use when going back to the start of the current line in a sixel to write more data over it.
    /// https://vt100.net/docs/vt3xx-gp/chapter14.html#S14.3.4.
    /// </summary>
    public const char SIXELDECGCR = '$';

    /// <summary>
    /// The start of a sixel sequence.
    /// https://vt100.net/docs/vt3xx-gp/chapter14.html#S14.2.1.
    /// </summary>
    public const string SIXELSTART = $"{ESC}P0;1q";

    /// <summary>
    /// The raster settings for setting the sixel pixel ratio to 1:1 so images are square when rendered instead of the 2:1 double height default.
    /// https://vt100.net/docs/vt3xx-gp/chapter14.html#S14.3.2.
    /// </summary>
    public const string SIXELRASTERATTRIBUTES = "\"1;1;";

    /// <summary>
    /// The end of a sixel sequence.
    /// </summary>
    public const string SIXELEND = $"{ESC}\\";

    /// <summary>
    /// The transparent color for the sixel, this is black but the sixel should be transparent so this is not visible.
    /// </summary>
    public const string SIXELTRANSPARENTCOLOR = "#0;2;0;0;0";
}