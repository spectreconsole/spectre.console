namespace Spectre.Console;

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

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsoleCursor"/>.
/// </summary>
public static class CursorExtensions
{
    /// <param name="cursor">The cursor.</param>
    extension(IAnsiConsoleCursor cursor)
    {
        /// <summary>
        /// Shows the cursor.
        /// </summary>
        public void Show()
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Show(true);
        }

        /// <summary>
        /// Hides the cursor.
        /// </summary>
        public void Hide()
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Show(false);
        }

        /// <summary>
        /// Moves the cursor up.
        /// </summary>
        public void MoveUp()
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Up, 1);
        }

        /// <summary>
        /// Moves the cursor up.
        /// </summary>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public void MoveUp(int steps)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Up, steps);
        }

        /// <summary>
        /// Moves the cursor down.
        /// </summary>
        public void MoveDown()
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Down, 1);
        }

        /// <summary>
        /// Moves the cursor down.
        /// </summary>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public void MoveDown(int steps)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Down, steps);
        }

        /// <summary>
        /// Moves the cursor to the left.
        /// </summary>
        public void MoveLeft()
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Left, 1);
        }

        /// <summary>
        /// Moves the cursor to the left.
        /// </summary>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public void MoveLeft(int steps)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Left, steps);
        }

        /// <summary>
        /// Moves the cursor to the right.
        /// </summary>
        public void MoveRight()
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Right, 1);
        }

        /// <summary>
        /// Moves the cursor to the right.
        /// </summary>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public void MoveRight(int steps)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Right, steps);
        }
    }
}