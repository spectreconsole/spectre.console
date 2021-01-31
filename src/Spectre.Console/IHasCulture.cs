using System.Globalization;

namespace Spectre.Console
{
    /// <summary>
    /// Represents something that has a culture.
    /// </summary>
    public interface IHasCulture
    {
        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        CultureInfo? Culture { get; set; }
    }
}
