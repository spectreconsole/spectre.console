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