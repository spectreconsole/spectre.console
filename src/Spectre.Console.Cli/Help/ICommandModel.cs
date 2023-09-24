namespace Spectre.Console.Cli.Help;

/// <summary>
/// Represents a command model.
/// </summary>
public interface ICommandModel : ICommandContainer
{
    /// <summary>
    /// Gets the name of the application.
    /// </summary>
    string ApplicationName { get; }
}
