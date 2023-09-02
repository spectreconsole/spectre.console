namespace Spectre.Console.Cli;

/// <summary>
/// Represents a configurator.
/// </summary>
public interface IConfigurator
{
    /// <summary>
    /// Sets the help provider for the application.
    /// </summary>
    /// <param name="helpProvider">The help provider to use.</param>
    public void SetHelpProvider(IHelpProvider helpProvider);

    /// <summary>
    /// Sets the help provider for the application.
    /// </summary>
    /// <typeparam name="T">The type of the help provider to instantiate at runtime and use.</typeparam>
    public void SetHelpProvider<T>()
        where T : IHelpProvider;

    /// <summary>
    /// Gets the command app settings.
    /// </summary>
    public ICommandAppSettings Settings { get; }

    /// <summary>
    /// Adds an example of how to use the application.
    /// </summary>
    /// <param name="args">The example arguments.</param>
    void AddExample(params string[] args);

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
    /// Adds a command that executes an async delegate.
    /// </summary>
    /// <typeparam name="TSettings">The command setting type.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    ICommandConfigurator AddAsyncDelegate<TSettings>(string name, Func<CommandContext, TSettings, Task<int>> func)
        where TSettings : CommandSettings;

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <typeparam name="TSettings">The command setting type.</typeparam>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="action">The command branch configurator.</param>
    /// <returns>A branch configurator that can be used to configure the branch further.</returns>
    IBranchConfigurator AddBranch<TSettings>(string name, Action<IConfigurator<TSettings>> action)
        where TSettings : CommandSettings;
}