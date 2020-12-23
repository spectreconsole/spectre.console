using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents the remaining arguments.
    /// </summary>
    public interface IRemainingArguments
    {
        /// <summary>
        /// Gets the parsed remaining arguments.
        /// </summary>
        ILookup<string, string?> Parsed { get; }

        /// <summary>
        /// Gets the raw, non-parsed remaining arguments.
        /// </summary>
        IReadOnlyList<string> Raw { get; }
    }
}
