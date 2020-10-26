namespace Spectre.Console
{
    /// <summary>
    /// Represents something that is paddable.
    /// </summary>
    public interface IPaddable
    {
        /// <summary>
        /// Gets or sets the padding.
        /// </summary>
        public Padding? Padding { get; set; }
    }
}
