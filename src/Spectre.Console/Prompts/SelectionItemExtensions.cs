namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="ISelectionItem{T}"/>.
/// </summary>
public static class SelectionItemExtensions
{
    /// <summary>
    /// Disables the item, if it supports being disabled.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    /// <param name="item">The item to disable.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ISelectionItem<T> Disable<T>(this ISelectionItem<T> item)
        where T : notnull
    {
        if (item is IDisableableSelectionItem<T> disableable)
        {
            disableable.Disable();
        }

        return item;
    }
}
