namespace Spectre.Console;

/// <summary>
/// Represents something that is expandable.
/// </summary>
public interface IExpandable
{
    /// <summary>
    /// Gets or sets a value indicating whether or not the object should
    /// expand to the available space. If <c>false</c>, the object's
    /// width will be auto calculated.
    /// </summary>
    bool Expand { get; set; }
}

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
            ArgumentNullException.ThrowIfNull(obj);

            obj.Expand = false;
            return obj;
        }

        /// <summary>
        /// Tells the specified object to expand to the available area.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Expand()
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Expand = true;
            return obj;
        }
    }
}