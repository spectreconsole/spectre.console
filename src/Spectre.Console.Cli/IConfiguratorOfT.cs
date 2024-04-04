namespace Spectre.Console.Cli;

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
    void AddExample(params string[] args);

    /// <summary>
    /// Adds a default command.
    /// </summary>
    /// <remarks>
    /// This is the command that will run if the user doesn't specify one on the command line.
    /// It must be able to execute successfully by itself ie. without requiring any command line
    /// arguments, flags or option values.
    /// </remarks>
    /// <typeparam name="TDefaultCommand">The default command type.</typeparam>
#if NET7_0_OR_GREATER
    [RequiresDynamicCode(TrimWarnings.AddCommandShouldBeExplicitAboutSettings)]
#endif
    void SetDefaultCommand<TDefaultCommand>()
        where TDefaultCommand : class, ICommandLimiter<TSettings>;

#if NET6_0_OR_GREATER
    /// <summary>
    /// Adds a default command.
    /// </summary>
    /// <remarks>
    /// This is the command that will run if the user doesn't specify one on the command line.
    /// It must be able to execute successfully by itself ie. without requiring any command line
    /// arguments, flags or option values.
    /// </remarks>
    /// <typeparam name="TDefaultCommand">The default command type.</typeparam>
    /// <typeparam name="TDefaultCommandSettings">The default command settings type.</typeparam>
    void SetDefaultCommand<TDefaultCommand, TDefaultCommandSettings>()
        where TDefaultCommand : class, ICommandLimiter<TSettings>
        where TDefaultCommandSettings : CommandSettings
    ;
#endif

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
#if NET7_0_OR_GREATER
    [RequiresDynamicCode(TrimWarnings.AddCommandShouldBeExplicitAboutSettings)]
#endif
    ICommandConfigurator AddCommand<TCommand>(string name)
        where TCommand : class, ICommandLimiter<TSettings>;

#if NET6_0_OR_GREATER
    /// <summary>
    /// Adds a command.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <typeparam name="TCommandSettings">The command settings type.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    ICommandConfigurator AddCommand<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TCommand,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TCommandSettings>(string name)
        where TCommand : class, ICommandLimiter<TSettings>
        where TCommandSettings : CommandSettings
    ;
#endif

    /// <summary>
    /// Adds a command that executes a delegate.
    /// </summary>
    /// <typeparam name="TDerivedSettings">The derived command setting type.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    ICommandConfigurator AddDelegate<
#if NET6_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        TDerivedSettings>(string name, Func<CommandContext, TDerivedSettings, int> func)
        where TDerivedSettings : TSettings;

    /// <summary>
    /// Adds a command that executes an async delegate.
    /// </summary>
    /// <typeparam name="TDerivedSettings">The derived command setting type.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    ICommandConfigurator AddAsyncDelegate<
#if NET6_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        TDerivedSettings>(string name, Func<CommandContext, TDerivedSettings, Task<int>> func)
        where TDerivedSettings : TSettings;

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <typeparam name="TDerivedSettings">The derived command setting type.</typeparam>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="action">The command branch configuration.</param>
    /// <returns>A branch configurator that can be used to configure the branch further.</returns>
    IBranchConfigurator AddBranch<
#if NET6_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action)
        where TDerivedSettings : TSettings;
}