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
    /// Sets the width of the column to a fix size.
    /// </summary>
    /// <typeparam name="T">An object implementing <see cref="IColumn"/>.</typeparam>
    /// <param name="obj">The column.</param>
    /// <param name="size">The column width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T FixWidth<T>(this T obj, int size)
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

        obj.Width = size;
        obj.SizeMode = SizeMode.Fixed;
        return obj;
    }

    /// <summary>
    /// Sets the width of the column to a proportional weight.
    /// </summary>
    /// <typeparam name="T">An object implementing <see cref="IColumn"/>.</typeparam>
    /// <param name="obj">The column.</param>
    /// <param name="weight">The column width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T StarWidth<T>(this T obj, double weight = 1.0)
        where T : class, IColumn
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (weight < 0.0)
        {
            throw new ArgumentException("Weight cannot be negative", nameof(weight));
        }

        obj.Width = weight;
        obj.SizeMode = SizeMode.Star;
        return obj;
    }

    /// <summary>
    /// Sets the column to auto size to content.
    /// </summary>
    /// <typeparam name="T">An object implementing <see cref="IColumn"/>.</typeparam>
    /// <param name="obj">The column.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T SizeToContent<T>(this T obj)
        where T : class, IColumn
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Width = null;
        obj.SizeMode = SizeMode.SizeToContent;
        return obj;
    }
}