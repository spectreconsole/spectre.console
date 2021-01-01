namespace Spectre.Console.Rendering
{
    public class TreeRendering
    {
        /// <summary>
        /// Gets ASCII rendering of a tree.
        /// </summary>
        public static ITreeRendering Ascii { get; } = new AsciiTreeRendering();
    }
}