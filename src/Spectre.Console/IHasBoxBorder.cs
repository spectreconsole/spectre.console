namespace Spectre.Console
{
    /// <summary>
    /// Represents something that has a box border.
    /// </summary>
    public interface IHasBoxBorder
    {
        /// <summary>
        /// Gets or sets the box.
        /// </summary>
        public BoxBorder Border { get; set; }
    }
}
