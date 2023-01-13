namespace Spectre.Console.Cli;

/// <summary>
/// Represents the help provider.
/// </summary>
public interface IHelpProvider
{
    /// <summary>
    /// Gets the help text.
    /// </summary>
    /// <remarks>
    /// The help text is formatted and ready to be rendered.
    /// </remarks>
    IEnumerable<IRenderable> Help { get; }
}
