using System;

namespace Spectre.Console
{
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
    }
}
