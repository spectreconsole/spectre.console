using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents a flag with an optional value.
    /// </summary>
    public interface IFlagValue
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the flag was set or not.
        /// </summary>
        bool IsSet { get; set; }

        /// <summary>
        /// Gets the flag's element type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets or sets the flag's value.
        /// </summary>
        object? Value { get; set; }
    }
}
