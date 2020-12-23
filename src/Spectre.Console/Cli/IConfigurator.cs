using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents a configurator.
    /// </summary>
    public interface IConfigurator
    {
        /// <summary>
        /// Gets the command app settings.
        /// </summary>
        public ICommandAppSettings Settings { get; }

        /// <summary>
        /// Adds an example of how to use the application.
        /// </summary>
        /// <param name="args">The example arguments.</param>
        void AddExample(string[] args);

        /// <summary>
        /// Adds a command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="name">The name of the command.</param>
        /// <returns>A command configurator that can be used to configure the command further.</returns>
        ICommandConfigurator AddCommand<TCommand>(string name)
            where TCommand : class, ICommand;

        /// <summary>
        /// Adds a command that executes a delegate.
        /// </summary>
        /// <typeparam name="TSettings">The command setting type.</typeparam>
        /// <param name="name">The name of the command.</param>
        /// <param name="func">The delegate to execute as part of command execution.</param>
        /// <returns>A command configurator that can be used to configure the command further.</returns>
        ICommandConfigurator AddDelegate<TSettings>(string name, Func<CommandContext, TSettings, int> func)
            where TSettings : CommandSettings;

        /// <summary>
        /// Adds a command branch.
        /// </summary>
        /// <typeparam name="TSettings">The command setting type.</typeparam>
        /// <param name="name">The name of the command branch.</param>
        /// <param name="action">The command branch configurator.</param>
        void AddBranch<TSettings>(string name, Action<IConfigurator<TSettings>> action)
            where TSettings : CommandSettings;
    }
}