using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a tree appearance.
    /// </summary>
    public abstract partial class TreeAppearance
    {
        /// <summary>
        /// Gets ASCII rendering of a tree.
        /// </summary>
        public static TreeAppearance Ascii { get; } = new AsciiTreeAppearance();
    }
}
