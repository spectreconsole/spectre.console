namespace Spectre.Console;

internal static class RenderOptionsExtensions
{
    public static BoxBorder GetSafeBorder<T>(this RenderOptions options, T border)
        where T : IHasBoxBorder, IHasBorder
    {
        return border.Border.GetSafeBorder(!options.Unicode && border.UseSafeBorder);
    }
}
