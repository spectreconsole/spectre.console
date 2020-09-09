namespace Spectre.Console
{
    /// <summary>
    /// Represents something that is expandable.
    /// </summary>
    public interface IExpandable
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the object should
        /// expand to the available space. If <c>false</c>, the object's
        /// width will be auto calculated.
        /// </summary>
        bool Expand { get; set; }
    }
}
