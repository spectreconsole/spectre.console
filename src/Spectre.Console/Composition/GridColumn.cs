namespace Spectre.Console
{
    /// <summary>
    /// Represents a grid column.
    /// </summary>
    public sealed class GridColumn
    {
        /// <summary>
        /// Gets or sets the width of the column.
        /// If <c>null</c>, the column will adapt to it's contents.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether wrapping of
        /// text within the column should be prevented.
        /// </summary>
        public bool NoWrap { get; set; }
    }
}
