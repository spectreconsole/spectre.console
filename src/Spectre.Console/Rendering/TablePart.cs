namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents different parts of a table.
    /// </summary>
    public enum TablePart
    {
        /// <summary>
        /// The top of a table.
        /// </summary>
        Top,

        /// <summary>
        /// The separator between the header and the cells.
        /// </summary>
        HeaderSeparator,

        /// <summary>
        /// The separator between the footer and the cells.
        /// </summary>
        FooterSeparator,

        /// <summary>
        /// The bottom of a table.
        /// </summary>
        Bottom,
    }
}
