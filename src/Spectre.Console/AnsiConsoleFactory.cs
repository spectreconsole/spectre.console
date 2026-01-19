namespace Spectre.Console;

/// <summary>
/// Factory for creating an ANSI console.
/// </summary>
[Obsolete("Consider using AnsiConsole.Create instead.")]
public sealed class AnsiConsoleFactory
{
    /// <summary>
    /// Creates an ANSI console.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <returns>An implementation of <see cref="IAnsiConsole"/>.</returns>
    public IAnsiConsole Create(AnsiConsoleSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var output = settings.Out ?? new AnsiConsoleOutput(System.Console.Out);
        if (output.Writer == null)
        {
            throw new InvalidOperationException("Output writer was null");
        }

        // Get the capabilities of the terminal
        var caps = Capabilities.Create(output.Writer, settings, out var encoding);

        // Create a profile using the capabilities and enrich it
        var profile = new Profile(output, caps, encoding);
        ProfileEnricher.Enrich(
            profile,
            settings.Enrichment,
            settings.EnvironmentVariables);

        return new AnsiConsoleFacade(
            profile,
            settings.ExclusivityMode ?? new DefaultExclusivityMode());
    }
}