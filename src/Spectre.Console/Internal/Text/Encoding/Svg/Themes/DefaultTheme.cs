namespace Spectre.Console.Internal;

/// <summary>
/// The default SVG theme.
/// </summary>
internal sealed class DefaultTheme : SvgTheme
{
    /// <inheritdoc/>
    public override Color ForegroundColor { get; } = Color.FromHex("#CCCCCC");

    /// <inheritdoc/>
    public override Color BackgroundColor { get; } = Color.FromHex("#2D2D2D");

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultTheme"/> class.
    /// </summary>
    public DefaultTheme()
    {
        Override(Color.Black, Color.FromHex("#2D2D2D"));
        Override(Color.Maroon, Color.FromHex("#F2777A"));
        Override(Color.Green, Color.FromHex("#99CC99"));
        Override(Color.Olive, Color.FromHex("#FFCC66"));
        Override(Color.Navy, Color.FromHex("#6699CC"));
        Override(Color.Purple, Color.FromHex("#CC99CC"));
        Override(Color.Teal, Color.FromHex("#66CCCC"));
        Override(Color.Silver, Color.FromHex("#CCCCCC"));
        Override(Color.Grey, Color.FromHex("#999999"));
        Override(Color.Red, Color.FromHex("#F2777A"));
        Override(Color.Lime, Color.FromHex("#99CC99"));
        Override(Color.Yellow, Color.FromHex("#FFCC66"));
        Override(Color.Blue, Color.FromHex("#6699CC"));
        Override(Color.Fuchsia, Color.FromHex("#CC99CC"));
        Override(Color.Aqua, Color.FromHex("#66CCCC"));
        Override(Color.White, Color.FromHex("#FFFFFF"));
    }
}