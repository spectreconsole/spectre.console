namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Exception"/>.
/// </summary>
public static class ExceptionExtensions
{
    /// <param name="exception">The exception to format.</param>
    extension(Exception exception)
    {
        /// <summary>
        /// Gets a <see cref="IRenderable"/> representation of the exception.
        /// </summary>
        /// <param name="format">The exception format options.</param>
        /// <returns>A <see cref="IRenderable"/> representing the exception.</returns>
        [RequiresDynamicCode(ExceptionFormatter.AotWarning)]
        public IRenderable GetRenderable(ExceptionFormats format = ExceptionFormats.Default)
        {
            ArgumentNullException.ThrowIfNull(exception);

            return GetRenderable(exception, new ExceptionSettings
            {
                Format = format,
            });
        }

        /// <summary>
        /// Gets a <see cref="IRenderable"/> representation of the exception.
        /// </summary>
        /// <param name="settings">The exception settings.</param>
        /// <returns>A <see cref="IRenderable"/> representing the exception.</returns>
        [RequiresDynamicCode(ExceptionFormatter.AotWarning)]
        public IRenderable GetRenderable(ExceptionSettings settings)
        {
            ArgumentNullException.ThrowIfNull(exception);

            ArgumentNullException.ThrowIfNull(settings);

            return ExceptionFormatter.Format(exception, settings);
        }
    }
}