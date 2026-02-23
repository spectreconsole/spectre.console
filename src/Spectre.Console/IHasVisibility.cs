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
    /// <summary>
    /// Marks the specified object as being invisible.
    /// </summary>
    /// <typeparam name="T">An object implementing <see cref="IHasVisibility"/>.</typeparam>
    /// <param name="obj">The object to hide.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Invisible<T>(this T obj)
        where T : class, IHasVisibility
    {
        ArgumentNullException.ThrowIfNull(obj);

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
        ArgumentNullException.ThrowIfNull(obj);

        obj.IsVisible = true;
        return obj;
    }
}