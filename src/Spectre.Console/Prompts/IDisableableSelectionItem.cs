namespace Spectre.Console;

/// <summary>
/// Represents a selection item that can be disabled.
/// A disabled item is rendered with the prompt's disabled style and cannot be navigated to or selected.
/// </summary>
/// <typeparam name="T">The data type.</typeparam>
public interface IDisableableSelectionItem<T> : ISelectionItem<T>
    where T : notnull
{
    /// <summary>
    /// Gets a value indicating whether or not this item is disabled.
    /// </summary>
    bool IsDisabled { get; }

    /// <summary>
    /// Disables the item.
    /// </summary>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    IDisableableSelectionItem<T> Disable();
}
