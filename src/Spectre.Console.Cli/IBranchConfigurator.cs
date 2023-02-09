namespace Spectre.Console.Cli;

/// <summary>
/// Represents a branch configurator.
/// </summary>
public interface IBranchConfigurator
{
    /// <summary>
    /// Adds an alias (an alternative name) to the branch being configured.
    /// </summary>
    /// <param name="name">The alias to add to the branch being configured.</param>
    /// <returns>The same <see cref="IBranchConfigurator"/> instance so that multiple calls can be chained.</returns>
    IBranchConfigurator WithAlias(string name);
}