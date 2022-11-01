namespace Spectre.Console;

internal static class RenderContextExtensions
{
    public static BoxBorder GetSafeBorder<T>(this RenderContext context, T border)
        where T : IHasBoxBorder, IHasBorder
    {
        return BoxExtensions.GetSafeBorder(border.Border, !context.Unicode && border.UseSafeBorder);
    }
}
