namespace Spectre.Console;

/// <summary>
/// Represents a column.
/// </summary>
public interface IColumn : IAlignable, IPaddable
{
    /// <summary>
    /// Gets or sets a value indicating whether
    /// or not wrapping should be prevented.
    /// </summary>
    bool NoWrap { get; set; }

    /// <summary>
    /// Gets or sets the width of the column.
    /// </summary>
    int? Width { get; set; }
}

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
        ArgumentNullException.ThrowIfNull(obj);

        obj.NoWrap = true;
        return obj;
    }

    /// <summary>
    /// Sets the width of the column.
    /// </summary>
    /// <typeparam name="T">An object implementing <see cref="IColumn"/>.</typeparam>
    /// <param name="obj">The column.</param>
    /// <param name="width">The column width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Width<T>(this T obj, int? width)
        where T : class, IColumn
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.Width = width;
        return obj;
    }
}