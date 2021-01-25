namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents a command line application settings.
    /// </summary>
    public interface ICommandAppSettings
    {
        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        string? ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the application version (use it to override auto-detected value).
        /// </summary>
        string? ApplicationVersion { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IAnsiConsole"/>.
        /// </summary>
        IAnsiConsole? Console { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ICommandInterceptor"/> used
        /// to intercept settings before it's being sent to the command.
        /// </summary>
        ICommandInterceptor? Interceptor { get; set; }

        /// <summary>
        /// Gets the type registrar.
        /// </summary>
        ITypeRegistrarFrontend Registrar { get; }

        /// <summary>
        /// Gets or sets case sensitivity.
        /// </summary>
        CaseSensitivity CaseSensitivity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not parsing is strict.
        /// </summary>
        bool StrictParsing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not exceptions should be propagated.
        /// </summary>
        bool PropagateExceptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not examples should be validated.
        /// </summary>
        bool ValidateExamples { get; set; }
    }
}