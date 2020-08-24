namespace Spectre.Console
{
    /// <summary>
    /// Represents something that is alignable.
    /// </summary>
    public interface IAlignable
    {
        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        Justify? Alignment { get; set; }
    }
}
