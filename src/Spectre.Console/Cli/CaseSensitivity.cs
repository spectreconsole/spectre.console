using System;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents case sensitivity.
    /// </summary>
    [Flags]
    [SuppressMessage("Naming", "CA1714:Flags enums should have plural names")]
    public enum CaseSensitivity
    {
        /// <summary>
        /// Nothing is case sensitive.
        /// </summary>
        None = 0,

        /// <summary>
        /// Long options are case sensitive.
        /// </summary>
        LongOptions = 1,

        /// <summary>
        /// Commands are case sensitive.
        /// </summary>
        Commands = 2,

        /// <summary>
        /// Everything is case sensitive.
        /// </summary>
        All = LongOptions | Commands,
    }
}
