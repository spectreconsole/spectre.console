using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a border.
    /// </summary>
    public abstract partial class TableBorder
    {
        /// <summary>
        /// Gets an invisible border.
        /// </summary>
        public static TableBorder None { get; } = new NoTableBorder();

        /// <summary>
        /// Gets an ASCII border.
        /// </summary>
        public static TableBorder Ascii { get; } = new AsciiTableBorder();

        /// <summary>
        /// Gets an ASCII border.
        /// </summary>
        public static TableBorder Ascii2 { get; } = new Ascii2TableBorder();

        /// <summary>
        /// Gets an ASCII border with a double header border.
        /// </summary>
        public static TableBorder AsciiDoubleHead { get; } = new AsciiDoubleHeadTableBorder();

        /// <summary>
        /// Gets a square border.
        /// </summary>
        public static TableBorder Square { get; } = new SquareTableBorder();

        /// <summary>
        /// Gets a rounded border.
        /// </summary>
        public static TableBorder Rounded { get; } = new RoundedTableBorder();

        /// <summary>
        /// Gets a minimal border.
        /// </summary>
        public static TableBorder Minimal { get; } = new MinimalTableBorder();

        /// <summary>
        /// Gets a minimal border with a heavy head.
        /// </summary>
        public static TableBorder MinimalHeavyHead { get; } = new MinimalHeavyHeadTableBorder();

        /// <summary>
        /// Gets a minimal border with a double header border.
        /// </summary>
        public static TableBorder MinimalDoubleHead { get; } = new MinimalDoubleHeadTableBorder();

        /// <summary>
        /// Gets a simple border.
        /// </summary>
        public static TableBorder Simple { get; } = new SimpleTableBorder();

        /// <summary>
        /// Gets a simple border with heavy lines.
        /// </summary>
        public static TableBorder SimpleHeavy { get; } = new SimpleHeavyTableBorder();

        /// <summary>
        /// Gets a horizontal border.
        /// </summary>
        public static TableBorder Horizontal { get; } = new HorizontalTableBorder();

        /// <summary>
        /// Gets a heavy border.
        /// </summary>
        public static TableBorder Heavy { get; } = new HeavyTableBorder();

        /// <summary>
        /// Gets a border with a heavy edge.
        /// </summary>
        public static TableBorder HeavyEdge { get; } = new HeavyEdgeTableBorder();

        /// <summary>
        /// Gets a border with a heavy header.
        /// </summary>
        public static TableBorder HeavyHead { get; } = new HeavyHeadTableBorder();

        /// <summary>
        /// Gets a double border.
        /// </summary>
        [SuppressMessage("Naming", "CA1720:Identifier contains type name")]
        public static TableBorder Double { get; } = new DoubleTableBorder();

        /// <summary>
        /// Gets a border with a double edge.
        /// </summary>
        public static TableBorder DoubleEdge { get; } = new DoubleEdgeTableBorder();

        /// <summary>
        /// Gets a markdown border.
        /// </summary>
        public static TableBorder Markdown { get; } = new MarkdownTableBorder();
    }
}
