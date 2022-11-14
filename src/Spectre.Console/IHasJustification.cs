namespace Spectre.Console;

/// <summary>
/// Represents something that has justification.
/// </summary>
public interface IHasJustification
{
    /// <summary>
    /// Gets or sets the justification.
    /// </summary>
    Justify? Justification { get; set; }
}