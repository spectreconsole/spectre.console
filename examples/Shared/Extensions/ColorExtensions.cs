namespace Spectre.Console.Examples;

public static class ColorExtensions
{
    public static Color GetInvertedColor(this Color color)
    {
        return GetLuminance(color) < 140 ? Color.White : Color.Black;
    }

    private static float GetLuminance(this Color color)
    {
        return (float)((0.2126 * color.R) + (0.7152 * color.G) + (0.0722 * color.B));
    }
}
