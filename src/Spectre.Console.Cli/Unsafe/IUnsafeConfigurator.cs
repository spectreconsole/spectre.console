namespace Spectre.Console.Cli.Unsafe;

/// <summary>
/// Represents an unsafe configurator.
/// </summary>
public interface IUnsafeConfigurator
{
    /// <summary>
    /// Adds a command.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <param name="command">The command type.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    ICommandConfigurator AddCommand(string name, [DynamicallyAccessedMembers(PublicConstructors | Interfaces)] Type command);

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="settings">The command setting type.</param>
    /// <param name="action">The command branch configurator.</param>
    /// <returns>A branch configurator that can be used to configure the branch further.</returns>
    [RequiresDynamicCode("Calls System.Type.MakeGenericType(params Type[])")]
    IBranchConfigurator AddBranch(string name, Type settings, Action<IUnsafeBranchConfigurator> action);
}