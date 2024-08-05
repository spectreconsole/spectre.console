namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Exception"/>.
/// </summary>
[RequiresUnreferencedCode("Exception formatter relies on reflection and isn't guaranteed to have valid results when trimming.")]
public static class ExceptionExtensions
{
    internal const string UnreferencedWarning =
        "Exception formatter relies on reflection. Results will not be formatted.";

    /// <summary>
    /// Gets a <see cref="IRenderable"/> representation of the exception.
    /// </summary>
    /// <param name="exception">The exception to format.</param>
    /// <param name="format">The exception format options.</param>
    /// <returns>A <see cref="IRenderable"/> representing the exception.</returns>
    public static IRenderable GetRenderable(this Exception exception, ExceptionFormats format = ExceptionFormats.Default)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return GetRenderable(exception, new ExceptionSettings
        {
            Format = format,
        });
    }

    /// <summary>
    /// Gets a <see cref="IRenderable"/> representation of the exception.
    /// </summary>
    /// <param name="exception">The exception to format.</param>
    /// <param name="settings">The exception settings.</param>
    /// <returns>A <see cref="IRenderable"/> representing the exception.</returns>
    public static IRenderable GetRenderable(this Exception exception, ExceptionSettings settings)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        return ExceptionFormatter.Format(exception, settings);
    }
}