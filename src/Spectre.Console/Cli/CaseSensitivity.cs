using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents case sensitivity.
    /// </summary>
    [Flags]
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
