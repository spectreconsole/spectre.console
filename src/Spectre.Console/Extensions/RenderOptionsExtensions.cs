namespace Spectre.Console;

internal static class RenderOptionsExtensions
{
    public static BoxBorder GetSafeBorder<T>(this RenderOptions options, T border)
        where T : IHasBoxBorder, IHasBorder
    {
        return BoxExtensions.GetSafeBorder(border.Border, !options.Unicode && border.UseSafeBorder);
    }
}
