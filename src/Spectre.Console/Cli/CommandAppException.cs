using System;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents errors that occur during application execution.
    /// </summary>
    public abstract class CommandAppException : Exception
    {
        /// <summary>
        /// Gets the pretty formatted exception message.
        /// </summary>
        public IRenderable? Pretty { get; }

        internal virtual bool AlwaysPropagateWhenDebugging => false;

        internal CommandAppException(string message, IRenderable? pretty = null)
            : base(message)
        {
            Pretty = pretty;
        }

        internal CommandAppException(string message, Exception ex, IRenderable? pretty = null)
            : base(message, ex)
        {
            Pretty = pretty;
        }
    }
}