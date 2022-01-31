namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IColumn"/>.
/// </summary>
public static class ColumnExtensions
{
    /// <summary>
    /// Prevents a column from wrapping.
    /// </summary>
    /// <typeparam name="T">An object implementing <see cref="IColumn"/>.</typeparam>
    /// <param name="obj">The column.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T NoWrap<T>(this T obj)
        where T : class, IColumn
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.NoWrap = true;
        return obj;
    }

    /// <summary>
    /// Sets the width of the column.
    /// </summary>
    /// <typeparam name="T">An object implementing <see cref="IColumn"/>.</typeparam>
    /// <param name="obj">The column.</param>
    /// <param name="size">The column width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">if <paramref name="obj"/> is null.</exception>
    /// <exception cref="ArgumentException">if <paramref name="size"/> is negative.</exception>
    public static T Width<T>(this T obj, int? size)
        where T : class, IColumn
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (size < 0)
        {
            throw new ArgumentException("Fixed width cannot be negative", nameof(size));
        }

        // Implicitly convert int? to ColumnWidth
        obj.Width = size;
        return obj;
    }

    /// <summary>
    /// Sets the width of the column.
    /// </summary>
    /// <typeparam name="T">An object implementing <see cref="IColumn"/>.</typeparam>
    /// <param name="obj">The column.</param>
    /// <param name="width">The column width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">if <paramref name="obj"/> is null.</exception>
    public static T Width<T>(this T obj, ColumnWidth width)
        where T : class, IColumn
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Width = width;
        return obj;
    }

    /// <summary>
    /// Checks column width size mode.
    /// </summary>
    /// <param name="obj">The column.</param>
    /// <returns><see langword="true"/> if column width is proportional.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="obj"/> is null.</exception>
    public static bool IsProportionalWidth(this IColumn obj)
        => obj switch
        {
            null => throw new ArgumentNullException(nameof(obj)),
            _ => obj.Width.SizeMode == SizeMode.Proportional,
        };

    /// <summary>
    /// Checks column width size mode.
    /// </summary>
    /// <param name="obj">The column.</param>
    /// <returns><see langword="true"/> if column width is fix.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="obj"/> is null.</exception>
    public static bool IsFixedWidth(this IColumn obj)
        => obj switch
        {
            null => throw new ArgumentNullException(nameof(obj)),
            _ => obj.Width.SizeMode == SizeMode.Fixed,
        };

    /// <summary>
    /// Checks column width size mode.
    /// </summary>
    /// <param name="obj">The column.</param>
    /// <returns><see langword="true"/> if column width is auto-size.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="obj"/> is null.</exception>
    public static bool IsAutoWidth(this IColumn obj)
        => obj switch
        {
            null => throw new ArgumentNullException(nameof(obj)),
            _ => obj.Width.SizeMode == SizeMode.SizeToContent,
        };
}