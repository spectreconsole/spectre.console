namespace Spectre.Console.Cli.Help;

/// <summary>
/// The help provider interface for spectre.console.
/// </summary>
/// <remarks>
/// Implementations of this interface are responsbile
/// for writing command help to the terminal when the
/// `-h` or `--help` has been specified on the command line.
/// </remarks>
public interface IHelpProvider
{
    /// <summary>
    /// Writes help information for the specified command model.
    /// </summary>
    /// <param name="model">The command model to write help for.</param>
    /// <returns>An enumerable collection of <see cref="IRenderable"/> objects representing the help information.</returns>
    IEnumerable<IRenderable> Write(ICommandModel model);

    /// <summary>
    /// Writes help information for the specified command model and command.
    /// </summary>
    /// <param name="model">The command model to write help for.</param>
    /// <param name="command">The command for which to write help information (optional).</param>
    /// <returns>An enumerable collection of <see cref="IRenderable"/> objects representing the help information.</returns>
    IEnumerable<IRenderable> WriteCommand(ICommandModel model, ICommandInfo? command);
}
