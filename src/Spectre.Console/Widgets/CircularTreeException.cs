using System;

namespace Spectre.Console
{
    /// <summary>
    /// Indicates that the tree being rendered includes a cycle, and cannot be rendered.
    /// </summary>
    public sealed class CircularTreeException : Exception
    {
        internal CircularTreeException(string message)
            : base(message)
        {
        }
    }
}