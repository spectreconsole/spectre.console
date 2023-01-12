namespace Spectre.Console.Cli.Internal.Configuration;

/// <summary>
/// Fluent configurator for the default command.
/// </summary>
public sealed class DefaultCommandConfigurator
{
    private readonly ConfiguredCommand _defaultCommand;

    internal DefaultCommandConfigurator(ConfiguredCommand defaultCommand)
    {
        _defaultCommand = defaultCommand;
    }

    /// <summary>
    /// Sets the description of the default command.
    /// </summary>
    /// <param name="description">The default command description.</param>
    /// <returns>The same <see cref="DefaultCommandConfigurator"/> instance so that multiple calls can be chained.</returns>
    public DefaultCommandConfigurator WithDescription(string description)
    {
        _defaultCommand.Description = description;
        return this;
    }

    /// <summary>
    /// Sets data that will be passed to the command via the <see cref="CommandContext"/>.
    /// </summary>
    /// <param name="data">The data to pass to the default command.</param>
    /// <returns>The same <see cref="DefaultCommandConfigurator"/> instance so that multiple calls can be chained.</returns>
    public DefaultCommandConfigurator WithData(object data)
    {
        _defaultCommand.Data = data;
        return this;
    }
}