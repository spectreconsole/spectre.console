namespace Spectre.Console;

/// <summary>
/// Represents something that can be hidden.
/// </summary>
public interface IHasVisibility
{
    /// <summary>
    /// Gets or sets a value indicating whether or not the object should
    /// be visible or not.
    /// </summary>
    bool IsVisible { get; set; }
}

/// <summary>
/// Contains extension methods for <see cref="IHasVisibility"/>.
/// </summary>
public static class VisibilityExtensions
{
    /// <param name="obj">The object to hide.</param>
    /// <typeparam name="T">An object implementing <see cref="IHasVisibility"/>.</typeparam>
    extension<T>(T obj) where T : class, IHasVisibility
    {
        /// <summary>
        /// Marks the specified object as being invisible.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Invisible()
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.IsVisible = false;
            return obj;
        }

        /// <summary>
        /// Marks the specified object as being visible.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Visible()
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.IsVisible = true;
            return obj;
        }
    }
}