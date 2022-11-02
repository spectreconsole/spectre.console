namespace Spectre.Console;

/// <summary>
/// Represents something that can be used to resolve ratios.
/// </summary>
internal interface IRatioResolvable
{
    /// <summary>
    /// Gets the ratio.
    /// </summary>
    int Ratio { get; }

    /// <summary>
    /// Gets the size.
    /// </summary>
    int? Width { get; }

    /// <summary>
    /// Gets the minimum size.
    /// </summary>
    int MinimumWidth { get; }
}
