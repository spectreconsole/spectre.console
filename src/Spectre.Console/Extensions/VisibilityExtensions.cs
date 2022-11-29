namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IHasVisibility"/>.
/// </summary>
public static class VisibilityExtensions
{
    /// <summary>
    /// Marks the specified object as being invisible.
    /// </summary>
    /// <typeparam name="T">An object implementing <see cref="IHasVisibility"/>.</typeparam>
    /// <param name="obj">The object to hide.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Invisible<T>(this T obj)
        where T : class, IHasVisibility
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
    /// <typeparam name="T">An object implementing <see cref="IHasVisibility"/>.</typeparam>
    /// <param name="obj">The object to show.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Visible<T>(this T obj)
        where T : class, IHasVisibility
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.IsVisible = true;
        return obj;
    }
}