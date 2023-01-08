namespace Spectre.Console.Cli;

/// <summary>
/// Fluent builder for the default command.
/// </summary>
public sealed class DefaultCommandBuilder
{
    private readonly ConfiguredCommand _defaultCommand;

    internal DefaultCommandBuilder(ConfiguredCommand defaultCommand)
    {
        _defaultCommand = defaultCommand;
    }

    /// <summary>
    /// Sets the description of the default command.
    /// </summary>
    /// <param name="description">The default command description.</param>
    /// <returns>The same <see cref="DefaultCommandBuilder"/> instance so that multiple calls can be chained.</returns>
    public DefaultCommandBuilder WithDescription(string description)
    {
        _defaultCommand.Description = description;
        return this;
    }

    /// <summary>
    /// Sets data that will be passed to the command via the <see cref="CommandContext"/>.
    /// </summary>
    /// <param name="data">The data to pass to the default command.</param>
    /// <returns>The same <see cref="DefaultCommandBuilder"/> instance so that multiple calls can be chained.</returns>
    public DefaultCommandBuilder WithData(object data)
    {
        _defaultCommand.Data = data;
        return this;
    }
}