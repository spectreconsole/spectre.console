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
    [RequiresDynamicCode("Uses MakeGenericType")]
    [RequiresUnreferencedCode("If some of the generic arguments are annotated (either with DynamicallyAccessedMembersAttribute, or generic constraints), trimming can\'t validate that the requirements of those annotations are met.")]
    ICommandConfigurator AddCommand(string name, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties
        | DynamicallyAccessedMemberTypes.Interfaces)] Type command);

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="settings">The command setting type.</param>
    /// <param name="action">The command branch configurator.</param>
    /// <returns>A branch configurator that can be used to configure the branch further.</returns>
    [RequiresDynamicCode("Uses MakeGenericType")]
    IBranchConfigurator AddBranch(string name, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties
                                                                           | DynamicallyAccessedMemberTypes.Interfaces)] Type settings, Action<IUnsafeBranchConfigurator> action);
}