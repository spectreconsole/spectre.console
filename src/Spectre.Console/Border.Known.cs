using System.Diagnostics.CodeAnalysis;
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
        /// Gets another ASCII border.
        /// </summary>
        public static Border Ascii2 { get; } = new Ascii2Border();

        /// <summary>
        /// Gets an ASCII border with a double header border.
        /// </summary>
        public static Border AsciiDoubleHead { get; } = new AsciiDoubleHeadBorder();

        /// <summary>
        /// Gets a square border.
        /// </summary>
        public static Border Square { get; } = new SquareBorder();

        /// <summary>
        /// Gets a rounded border.
        /// </summary>
        public static Border Rounded { get; } = new RoundedBorder();

        /// <summary>
        /// Gets a minimal border.
        /// </summary>
        public static Border Minimal { get; } = new MinimalBorder();

        /// <summary>
        /// Gets a minimal border with a heavy head.
        /// </summary>
        public static Border MinimalHeavyHead { get; } = new MinimalHeavyHeadBorder();

        /// <summary>
        /// Gets a minimal border with a double header border.
        /// </summary>
        public static Border MinimalDoubleHead { get; } = new MinimalDoubleHeadBorder();

        /// <summary>
        /// Gets a simple border.
        /// </summary>
        public static Border Simple { get; } = new SimpleBorder();

        /// <summary>
        /// Gets a simple border with heavy lines.
        /// </summary>
        public static Border SimpleHeavy { get; } = new SimpleHeavyBorder();

        /// <summary>
        /// Gets a horizontal border.
        /// </summary>
        public static Border Horizontal { get; } = new HorizontalBorder();

        /// <summary>
        /// Gets a heavy border.
        /// </summary>
        public static Border Heavy { get; } = new HeavyBorder();

        /// <summary>
        /// Gets a border with a heavy edge.
        /// </summary>
        public static Border HeavyEdge { get; } = new HeavyEdgeBorder();

        /// <summary>
        /// Gets a border with a heavy header.
        /// </summary>
        public static Border HeavyHead { get; } = new HeavyHeadBorder();

        /// <summary>
        /// Gets a double border.
        /// </summary>
        [SuppressMessage("Naming", "CA1720:Identifier contains type name")]
        public static Border Double { get; } = new DoubleBorder();

        /// <summary>
        /// Gets a border with a double edge.
        /// </summary>
        public static Border DoubleEdge { get; } = new DoubleEdgeBorder();
    }
}
