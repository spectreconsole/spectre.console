namespace Spectre.Console.Cli;

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
    /// Gets or sets a value indicating whether trailing period of a description is trimmed.
    /// </summary>
    bool TrimTrailingPeriod { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not parsing is strict.
    /// </summary>
    bool StrictParsing { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not exceptions should be propagated.
    /// <para>Setting this to <c>true</c> will disable default Exception handling and
    /// any <see cref="ExceptionHandler"/>, if set.</para>
    /// </summary>
    bool PropagateExceptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not examples should be validated.
    /// </summary>
    bool ValidateExamples { get; set; }

    /// <summary>
    /// Gets or sets a handler for Exceptions.
    /// <para>This handler will not be called, if <see cref="PropagateExceptions"/> is set to <c>true</c>.</para>
    /// </summary>
    public Func<Exception, int>? ExceptionHandler { get; set; }
}