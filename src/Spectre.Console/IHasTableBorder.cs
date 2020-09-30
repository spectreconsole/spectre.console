namespace Spectre.Console
{
    /// <summary>
    /// Represents something that has a border.
    /// </summary>
    public interface IHasTableBorder : IHasBorder
    {
        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        public TableBorder Border { get; set; }
    }
}
