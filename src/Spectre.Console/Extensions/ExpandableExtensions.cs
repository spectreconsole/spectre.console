namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IExpandable"/>.
/// </summary>
public static class ExpandableExtensions
{
    /// <param name="obj">The object to collapse.</param>
    /// <typeparam name="T">The expandable object.</typeparam>
    extension<T>(T obj) where T : class, IExpandable
    {
        /// <summary>
        /// Tells the specified object to not expand to the available area
        /// but take as little space as possible.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Collapse()
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Expand = false;
            return obj;
        }

        /// <summary>
        /// Tells the specified object to expand to the available area.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Expand()
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Expand = true;
            return obj;
        }
    }
}