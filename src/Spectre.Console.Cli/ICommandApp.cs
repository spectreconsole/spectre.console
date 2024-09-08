namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command line application.
/// </summary>
public interface ICommandApp
{
    /// <summary>
    /// Configures the command line application.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    void Configure(Action<IConfigurator> configuration);

    /// <summary>
    /// Runs the command line application with specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to abort the application.</param>
    /// <returns>The exit code from the executed command.</returns>
    int Run(IEnumerable<string> args, CancellationToken cancellationToken = default);

    /// <summary>
    /// Runs the command line application with specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to abort the application.</param>
    /// <returns>The exit code from the executed command.</returns>
    Task<int> RunAsync(IEnumerable<string> args, CancellationToken cancellationToken = default);
}