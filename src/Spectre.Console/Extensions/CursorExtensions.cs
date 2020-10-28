namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsoleCursor"/>.
    /// </summary>
    public static class CursorExtensions
    {
        /// <summary>
        /// Shows the cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public static void Show(this IAnsiConsoleCursor cursor)
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
        /// <param name="cursor">The cursor.</param>
        public static void Hide(this IAnsiConsoleCursor cursor)
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
        /// <param name="cursor">The cursor.</param>
        public static void MoveUp(this IAnsiConsoleCursor cursor)
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
        /// <param name="cursor">The cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveUp(this IAnsiConsoleCursor cursor, int steps)
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
        /// <param name="cursor">The cursor.</param>
        public static void MoveDown(this IAnsiConsoleCursor cursor)
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
        /// <param name="cursor">The cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveDown(this IAnsiConsoleCursor cursor, int steps)
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
        /// <param name="cursor">The cursor.</param>
        public static void MoveLeft(this IAnsiConsoleCursor cursor)
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
        /// <param name="cursor">The cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveLeft(this IAnsiConsoleCursor cursor, int steps)
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
        /// <param name="cursor">The cursor.</param>
        public static void MoveRight(this IAnsiConsoleCursor cursor)
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
        /// <param name="cursor">The cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveRight(this IAnsiConsoleCursor cursor, int steps)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Right, steps);
        }
    }
}
