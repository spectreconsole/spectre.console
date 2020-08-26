namespace Spectre.Console
{
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
    }
}
