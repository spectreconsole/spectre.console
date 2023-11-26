namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command line application settings.
/// </summary>
public interface ICommandAppSettings
{
    /// <summary>
    /// Gets or sets the culture.
    /// </summary>
    /// <remarks>
    /// Text displayed by <see cref="Help.HelpProvider"/> can be localised, but defaults to English.
    /// Setting this property informs the resource manager which culture to use when fetching strings.
    /// English will be used when a culture has not been specified (ie. this property is null)
    /// or a string has not been localised for the specified culture.
    /// </remarks>
    CultureInfo? Culture { get; set; }

    /// <summary>
    /// Gets or sets the application name.
    /// </summary>
    string? ApplicationName { get; set; }

    /// <summary>
    /// Gets or sets the application version (use it to override auto-detected value).
    /// </summary>
    string? ApplicationVersion { get; set; }

    /// <summary>
    /// Gets or sets a value indicating how many examples from direct children to show in the help text.
    /// </summary>
    int MaximumIndirectExamples { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether any default values for command options are shown in the help text.
    /// </summary>
    bool ShowOptionDefaultValues { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a trailing period of a command description is trimmed in the help text.
    /// </summary>
    bool TrimTrailingPeriod { get; set; }

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
    /// Gets or sets a value indicating whether or not flags found on the commnd line
    /// that would normally result in a <see cref="CommandParseException"/> being thrown
    /// during parsing with the message "Flags cannot be assigned a value."
    /// should instead be added to the remaining arguments collection.
    /// </summary>
    bool ConvertFlagsToRemainingArguments { get; set; }

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
    /// The <see cref="ITypeResolver"/> argument will only be not-null, when the exception occurs during execution of
    /// a command. I.e. only when the resolver is available.
    /// </summary>
    public Func<Exception, ITypeResolver?, int>? ExceptionHandler { get; set; }
}