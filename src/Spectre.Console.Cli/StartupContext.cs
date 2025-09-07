namespace Spectre.Console.Cli;

/// <summary>
/// Represents a startup context.
/// </summary>
public sealed class StartupContext
{
    /// <summary>
    /// Gets the type resolver of the current command app.
    /// </summary>
    /// <value>
    /// The type resolver.
    /// </value>
    public ITypeResolver TypeResolver { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StartupContext"/> class.
    /// </summary>
    /// <param name="typeResolver">The type resolver.</param>
    public StartupContext(ITypeResolver typeResolver)
    {
        TypeResolver = typeResolver;
    }
}