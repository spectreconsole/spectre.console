namespace Spectre.Console.Rendering
{
    public class TreeRendering
    {
        public static ITreeRendering Ascii { get; } = new AsciiTreeRendering();
    }
}