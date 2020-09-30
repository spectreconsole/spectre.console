using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a border.
    /// </summary>
    public abstract partial class BoxBorder
    {
        /// <summary>
        /// Gets an invisible border.
        /// </summary>
        public static BoxBorder None { get; } = new NoBoxBorder();

        /// <summary>
        /// Gets an ASCII border.
        /// </summary>
        public static BoxBorder Ascii { get; } = new AsciiBoxBorder();

        /// <summary>
        /// Gets a double border.
        /// </summary>
        [SuppressMessage("Naming", "CA1720:Identifier contains type name")]
        public static BoxBorder Double { get; } = new DoubleBoxBorder();

        /// <summary>
        /// Gets a heavy border.
        /// </summary>
        public static BoxBorder Heavy { get; } = new HeavyBoxBorder();

        /// <summary>
        /// Gets a rounded border.
        /// </summary>
        public static BoxBorder Rounded { get; } = new RoundedBoxBorder();

        /// <summary>
        /// Gets a square border.
        /// </summary>
        public static BoxBorder Square { get; } = new SquareBoxBorder();
    }
}
