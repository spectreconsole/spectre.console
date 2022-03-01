namespace Spectre.Console;

/// <summary>
/// Represent a selection item.
/// </summary>
/// <typeparam name="T">The data type.</typeparam>
public interface ISelectionItem<T>
    where T : notnull
{
    /// <summary>
    /// Adds a child to the item.
    /// </summary>
    /// <param name="child">The child to add.</param>
    /// <returns>A new <see cref="ISelectionItem{T}"/> instance representing the child.</returns>
    ISelectionItem<T> AddChild(T child);
}