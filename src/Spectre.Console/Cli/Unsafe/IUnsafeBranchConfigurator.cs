namespace Spectre.Console.Cli.Unsafe
{
    /// <summary>
    /// Represents an unsafe configurator for a branch.
    /// </summary>
    public interface IUnsafeBranchConfigurator : IUnsafeConfigurator
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
    }
}
