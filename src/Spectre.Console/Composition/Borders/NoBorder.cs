namespace Spectre.Console.Composition
{
    internal sealed class NoBorder : Border
    {
        protected override string GetBoxPart(BorderPart part)
        {
            return " ";
        }
    }
}
