using System.Linq;

namespace Spectre.Console
{
    internal static class AnsiSequences
    {
        /// <summary>
        /// The ASCII escape character (decimal 27).
        /// </summary>
        public const string ESC = "\u001b";

        /// <summary>
        /// Introduces a control sequence that uses 8-bit characters.
        /// </summary>
        public const string CSI = ESC + "[";

        /// <summary>
        /// Text cursor enable.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/DECRQM.html#T5-8"/>.
        /// </remarks>
        public const int DECTCEM = 25;

        /// <summary>
        /// This control function selects one or more character attributes at the same time.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/SGR.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string SGR(params byte[] codes)
        {
            var joinedCodes = string.Join(";", codes.Select(c => c.ToString()));
            return $"{CSI}{joinedCodes}m";
        }

        /// <summary>
        /// This control function erases characters from part or all of the display.
        /// When you erase complete lines, they become single-height, single-width lines,
        /// with all visual character attributes cleared.
        /// ED works inside or outside the scrolling margins.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/ED.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string ED(int code)
        {
            return $"{CSI}{code}J";
        }

        /// <summary>
        /// Moves the cursor up a specified number of lines in the same column.
        /// The cursor stops at the top margin.
        /// If the cursor is already above the top margin, then the cursor stops at the top line.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/CUU.html"/>.
        /// </remarks>
        /// <param name="steps">The number of steps to move up.</param>
        /// <returns>The ANSI escape code.</returns>
        public static string CUU(int steps)
        {
            return $"{CSI}{steps}A";
        }

        /// <summary>
        /// This control function moves the cursor down a specified number of lines in the same column.
        /// The cursor stops at the bottom margin.
        /// If the cursor is already below the bottom margin, then the cursor stops at the bottom line.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/CUD.html"/>.
        /// </remarks>
        /// <param name="steps">The number of steps to move down.</param>
        /// <returns>The ANSI escape code.</returns>
        public static string CUD(int steps)
        {
            return $"{CSI}{steps}B";
        }

        /// <summary>
        /// This control function moves the cursor to the right by a specified number of columns.
        /// The cursor stops at the right border of the page.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/CUF.html"/>.
        /// </remarks>
        /// <param name="steps">The number of steps to move forward.</param>
        /// <returns>The ANSI escape code.</returns>
        public static string CUF(int steps)
        {
            return $"{CSI}{steps}C";
        }

        /// <summary>
        /// This control function moves the cursor to the left by a specified number of columns.
        /// The cursor stops at the left border of the page.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/CUB.html"/>.
        /// </remarks>
        /// <param name="steps">The number of steps to move backward.</param>
        /// <returns>The ANSI escape code.</returns>
        public static string CUB(int steps)
        {
            return $"{CSI}{steps}D";
        }

        /// <summary>
        /// Moves the cursor to the specified position.
        /// </summary>
        /// <param name="line">The line to move to.</param>
        /// <param name="column">The column to move to.</param>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/CUP.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string CUP(int line, int column)
        {
            return $"{CSI}{line};{column}H";
        }

        /// <summary>
        /// Hides the cursor.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/RM.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string RM(int code)
        {
            return $"{CSI}?{code}l";
        }

        /// <summary>
        /// Shows the cursor.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/SM.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string SM(int code)
        {
            return $"{CSI}?{code}h";
        }

        /// <summary>
        /// This control function erases characters on the line that has the cursor.
        /// EL clears all character attributes from erased character positions.
        /// EL works inside or outside the scrolling margins.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/EL.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string EL(int code)
        {
            return $"{CSI}{code}K";
        }
    }
}
