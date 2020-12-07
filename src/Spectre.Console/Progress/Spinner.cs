using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a spinner used in a <see cref="SpinnerColumn"/>.
    /// </summary>
    public abstract partial class Spinner
    {
        /// <summary>
        /// Gets the update interval for the spinner.
        /// </summary>
        public abstract TimeSpan Interval { get; }

        /// <summary>
        /// Gets a value indicating whether or not the spinner
        /// uses Unicode characters.
        /// </summary>
        public abstract bool IsUnicode { get; }

        /// <summary>
        /// Gets the spinner frames.
        /// </summary>
        public abstract IReadOnlyList<string> Frames { get; }
    }
}
