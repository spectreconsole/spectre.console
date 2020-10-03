using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Gets a <see cref="IRenderable"/> representation of the exception.
        /// </summary>
        /// <param name="exception">The exception to format.</param>
        /// <param name="format">The exception format options.</param>
        /// <returns>A <see cref="IRenderable"/> representing the exception.</returns>
        public static IRenderable GetRenderable(this Exception exception, ExceptionFormats format = ExceptionFormats.None)
        {
            return ExceptionFormatter.Format(exception, format);
        }
    }
}
