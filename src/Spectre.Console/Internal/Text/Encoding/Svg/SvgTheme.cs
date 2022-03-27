namespace Spectre.Console.Internal;

/// <summary>
/// Represents a theme used for SVG rendering.
/// </summary>
internal abstract class SvgTheme
{
    private readonly Dictionary<Color, Color> _lookup;
    private readonly Dictionary<byte, Color> _lookupNumbers;
    private readonly Dictionary<string, Color> _lookupNames;

    /// <summary>
    /// Initializes a new instance of the <see cref="SvgTheme"/> class.
    /// </summary>
    protected SvgTheme()
    {
        _lookup = new Dictionary<Color, Color>();
        _lookupNumbers = new Dictionary<byte, Color>();
        _lookupNames = new Dictionary<string, Color>();
    }

    /// <summary>
    /// Gets the background color.
    /// </summary>
    public abstract Color BackgroundColor { get; }

    /// <summary>
    /// Gets the foreground color.
    /// </summary>
    public abstract Color ForegroundColor { get; }

    /// <summary>
    /// Overrides a color with the specified one.
    /// </summary>
    /// <param name="color">The color to override.</param>
    /// <param name="newColor">The color to use instead of the overridden color.</param>
    protected void Override(Color color, Color newColor)
    {
        if (color.Number != null)
        {
            _lookupNumbers[color.Number.Value] = newColor;

            var name = ColorTable.GetName(color.Number.Value);
            if (name != null)
            {
                _lookupNames[name] = newColor;
            }
        }

        _lookup[color] = newColor;
    }

    internal Color GetColor(Color color)
    {
        if (color.IsDefault)
        {
            return color;
        }

        if (color.Number != null)
        {
            // Found by number?
            if (_lookupNumbers.TryGetValue(color.Number.Value, out var numberResult))
            {
                return numberResult;
            }

            // Found by name?
            var name = ColorTable.GetName(color.Number.Value);
            if (name != null)
            {
                if (_lookupNames.TryGetValue(name, out var namedResult))
                {
                    return namedResult;
                }
            }
        }

        // Exact match?
        if (_lookup.TryGetValue(color, out var result))
        {
            return result;
        }

        return color;
    }
}
