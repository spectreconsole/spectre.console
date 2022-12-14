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
    int? Size { get; }

    /// <summary>
    /// Gets the minimum size.
    /// </summary>
    int MinimumSize { get; }
}
