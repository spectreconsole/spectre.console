namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents a command configurator.
    /// </summary>
    public interface ICommandConfigurator
    {
        /// <summary>
        /// Adds an example of how to use the command.
        /// </summary>
        /// <param name="args">The example arguments.</param>
        /// <returns>The same <see cref="ICommandConfigurator"/> instance so that multiple calls can be chained.</returns>
        ICommandConfigurator WithExample(string[] args);

        /// <summary>
        /// Adds an alias (an alternative name) to the command being configured.
        /// </summary>
        /// <param name="name">The alias to add to the command being configured.</param>
        /// <returns>The same <see cref="ICommandConfigurator"/> instance so that multiple calls can be chained.</returns>
        ICommandConfigurator WithAlias(string name);

        /// <summary>
        /// Sets the description of the command.
        /// </summary>
        /// <param name="description">The command description.</param>
        /// <returns>The same <see cref="ICommandConfigurator"/> instance so that multiple calls can be chained.</returns>
        ICommandConfigurator WithDescription(string description);

        /// <summary>
        /// Sets data that will be passed to the command via the <see cref="CommandContext"/>.
        /// </summary>
        /// <param name="data">The data to pass to the command.</param>
        /// <returns>The same <see cref="ICommandConfigurator"/> instance so that multiple calls can be chained.</returns>
        ICommandConfigurator WithData(object data);

        /// <summary>
        /// Marks the command as hidden.
        /// Hidden commands do not show up in help documentation or
        /// generated XML models.
        /// </summary>
        /// <returns>The same <see cref="ICommandConfigurator"/> instance so that multiple calls can be chained.</returns>
        ICommandConfigurator IsHidden();
    }
}