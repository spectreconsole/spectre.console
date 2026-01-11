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
    /// <summary>
    /// Shows the cursor.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    public static void Show(this IAnsiConsoleCursor cursor)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        cursor.Show(true);
    }

    /// <summary>
    /// Hides the cursor.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    public static void Hide(this IAnsiConsoleCursor cursor)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        cursor.Show(false);
    }

    /// <summary>
    /// Moves the cursor up.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    public static void MoveUp(this IAnsiConsoleCursor cursor)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        cursor.Move(CursorDirection.Up, 1);
    }

    /// <summary>
    /// Moves the cursor up.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    /// <param name="steps">The number of steps to move the cursor.</param>
    public static void MoveUp(this IAnsiConsoleCursor cursor, int steps)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        cursor.Move(CursorDirection.Up, steps);
    }

    /// <summary>
    /// Moves the cursor down.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    public static void MoveDown(this IAnsiConsoleCursor cursor)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        cursor.Move(CursorDirection.Down, 1);
    }

    /// <summary>
    /// Moves the cursor down.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    /// <param name="steps">The number of steps to move the cursor.</param>
    public static void MoveDown(this IAnsiConsoleCursor cursor, int steps)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        cursor.Move(CursorDirection.Down, steps);
    }

    /// <summary>
    /// Moves the cursor to the left.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    public static void MoveLeft(this IAnsiConsoleCursor cursor)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        cursor.Move(CursorDirection.Left, 1);
    }

    /// <summary>
    /// Moves the cursor to the left.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    /// <param name="steps">The number of steps to move the cursor.</param>
    public static void MoveLeft(this IAnsiConsoleCursor cursor, int steps)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        cursor.Move(CursorDirection.Left, steps);
    }

    /// <summary>
    /// Moves the cursor to the right.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    public static void MoveRight(this IAnsiConsoleCursor cursor)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        cursor.Move(CursorDirection.Right, 1);
    }

    /// <summary>
    /// Moves the cursor to the right.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    /// <param name="steps">The number of steps to move the cursor.</param>
    public static void MoveRight(this IAnsiConsoleCursor cursor, int steps)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        cursor.Move(CursorDirection.Right, steps);
    }
}