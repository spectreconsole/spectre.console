namespace Spectre.Console
{
    /// <summary>
    /// Represents the console's cursor.
    /// </summary>
    public interface IAnsiConsoleCursor
    {
        /// <summary>
        /// Shows or hides the cursor.
        /// </summary>
        /// <param name="show"><c>true</c> to show the cursor, <c>false</c> to hide it.</param>
        void Show(bool show);

        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="column">The column to move the cursor to.</param>
        /// <param name="line">The line to move the cursor to.</param>
        void SetPosition(int column, int line);

        /// <summary>
        /// Moves the cursor relative to the current position.
        /// </summary>
        /// <param name="direction">The direction to move the cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        void Move(CursorDirection direction, int steps);
    }
}
