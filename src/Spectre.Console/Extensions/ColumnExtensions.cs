namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IColumn"/>.
/// </summary>
public static class ColumnExtensions
{
    /// <param name="obj">The column.</param>
    /// <typeparam name="T">An object implementing <see cref="IColumn"/>.</typeparam>
    extension<T>(T obj) where T : class, IColumn
    {
        /// <summary>
        /// Prevents a column from wrapping.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T NoWrap()
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
        /// <param name="width">The column width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Width(int? width)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Width = width;
            return obj;
        }
    }
}