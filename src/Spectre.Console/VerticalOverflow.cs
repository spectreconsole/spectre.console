namespace Spectre.Console;

/// <summary>
/// Represents vertical overflow.
/// </summary>
public enum VerticalOverflow
{
    /// <summary>
    /// Crop overflow.
    /// </summary>
    Crop = 0,

    /// <summary>
    /// Add an ellipsis at the end.
    /// </summary>
    Ellipsis = 1,

    /// <summary>
    /// Do not do anything about overflow.
    /// </summary>
    Visible = 2,
}