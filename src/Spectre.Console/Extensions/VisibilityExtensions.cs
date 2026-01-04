namespace Spectre.Console;

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
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.IsVisible = false;
            return obj;
        }

        /// <summary>
        /// Marks the specified object as being visible.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Visible()
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.IsVisible = true;
            return obj;
        }
    }
}