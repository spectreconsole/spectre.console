namespace Spectre.Console;

/// <summary>
/// Represents a column.
/// </summary>
public interface IColumn : IAlignable, IPaddable
{
    /// <summary>
    /// Gets or sets a value indicating whether
    /// or not wrapping should be prevented.
    /// </summary>
    bool NoWrap { get; set; }

    /// <summary>
    /// Gets or sets the width of the column.
    /// </summary>
    double? Width { get; set; }

    /// <summary>
    /// Gets or sets the size mode, to define
    /// how the <see cref="Width"/> value is interpreted.
    /// </summary>
    SizeMode SizeMode { get; set; }
}