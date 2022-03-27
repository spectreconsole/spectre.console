namespace Spectre.Console.Internal;

/// <summary>
/// Represents settings for the SVG encoder.
/// </summary>
internal sealed class SvgEncoderSettings
{
    /// <summary>
    /// Gets or sets the font size.
    /// Default value is 18.
    /// </summary>
    public int FontSize { get; set; } = 18;

    /// <summary>
    /// Gets or sets the line height.
    /// Default value is 22.
    /// </summary>
    public int LineHeight { get; set; } = 22;

    /// <summary>
    /// Gets or sets the font width scale.
    /// Default value is 0.6.
    /// </summary>
    public float FontWidthScale { get; set; } = 0.6f;

    /// <summary>
    /// Gets or sets the margin (in pixels).
    /// </summary>
    public int Margin { get; set; } = 140;

    /// <summary>
    /// Gets or sets the theme to use.
    /// </summary>
    public SvgTheme Theme { get; set; } = new DefaultTheme();
}
