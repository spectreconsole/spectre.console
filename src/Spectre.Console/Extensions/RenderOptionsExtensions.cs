namespace Spectre.Console;

internal static class RenderOptionsExtensions
{
    extension(RenderOptions options)
    {
        public BoxBorder GetSafeBorder<T>(T border)
            where T : IHasBoxBorder, IHasBorder
        {
            return BoxExtensions.GetSafeBorder(border.Border, !options.Unicode && border.UseSafeBorder);
        }
    }
}