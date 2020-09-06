using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a border.
    /// </summary>
    public abstract partial class Border
    {
        /// <summary>
        /// Gets an invisible border.
        /// </summary>
        public static Border None { get; } = new NoBorder();

        /// <summary>
        /// Gets an ASCII border.
        /// </summary>
        public static Border Ascii { get; } = new AsciiBorder();

        /// <summary>
        /// Gets a square border.
        /// </summary>
        public static Border Square { get; } = new SquareBorder();

        /// <summary>
        /// Gets a rounded border.
        /// </summary>
        public static Border Rounded { get; } = new RoundedBorder();
    }
}
