namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Selection of different renderings which can be used by <see cref="Tree"/>.
    /// </summary>
    public static class TreeRendering
    {
        /// <summary>
        /// Gets ASCII rendering of a tree.
        /// </summary>
        public static ITreeRendering Ascii { get; } = new AsciiTreeRendering();
    }
}