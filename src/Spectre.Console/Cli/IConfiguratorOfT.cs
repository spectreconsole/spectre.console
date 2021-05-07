using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents a configurator for specific settings.
    /// </summary>
    /// <typeparam name="TSettings">The command setting type.</typeparam>
    public interface IConfigurator<in TSettings>
        where TSettings : CommandSettings
    {
        /// <summary>
        /// Sets the description of the branch.
        /// </summary>
        /// <param name="description">The description of the branch.</param>
        void SetDescription(string description);

        /// <summary>
        /// Adds an example of how to use the branch.
        /// </summary>
        /// <param name="args">The example arguments.</param>
        void AddExample(string[] args);

        /// <summary>
        /// Marks the branch as hidden.
        /// Hidden branches do not show up in help documentation or
        /// generated XML models.
        /// </summary>
        void HideBranch();

        /// <summary>
        /// Adds a command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="name">The name of the command.</param>
        /// <returns>A command configurator that can be used to configure the command further.</returns>
        ICommandConfigurator AddCommand<TCommand>(string name)
            where TCommand : class, ICommandLimiter<TSettings>;

        /// <summary>
        /// Adds a command that executes a delegate.
        /// </summary>
        /// <typeparam name="TDerivedSettings">The derived command setting type.</typeparam>
        /// <param name="name">The name of the command.</param>
        /// <param name="func">The delegate to execute as part of command execution.</param>
        /// <returns>A command configurator that can be used to configure the command further.</returns>
        ICommandConfigurator AddDelegate<TDerivedSettings>(string name, Func<CommandContext, TDerivedSettings, int> func)
            where TDerivedSettings : TSettings;

        /// <summary>
        /// Adds a command branch.
        /// </summary>
        /// <typeparam name="TDerivedSettings">The derived command setting type.</typeparam>
        /// <param name="name">The name of the command branch.</param>
        /// <param name="action">The command branch configuration.</param>
        void AddBranch<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action)
            where TDerivedSettings : TSettings;
    }
}
