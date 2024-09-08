using Spectre.Console.Cli.Internal.Configuration;

namespace Spectre.Console.Cli;

/// <summary>
/// The entry point for a command line application with a default command.
/// </summary>
/// <typeparam name="TDefaultCommand">The type of the default command.</typeparam>
#if !NETSTANDARD2_0
[RequiresDynamicCode("Spectre.Console.Cli relies on reflection. Use during trimming and AOT compilation is not supported and may result in unexpected behaviors.")]
#endif
public sealed class CommandApp<TDefaultCommand> : ICommandApp
    where TDefaultCommand : class, ICommand
{
    private readonly CommandApp _app;
    private readonly DefaultCommandConfigurator _defaultCommandConfigurator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandApp{TDefaultCommand}"/> class.
    /// </summary>
    /// <param name="registrar">The registrar.</param>
    public CommandApp(ITypeRegistrar? registrar = null)
    {
        _app = new CommandApp(registrar);
        _defaultCommandConfigurator = _app.SetDefaultCommand<TDefaultCommand>();
    }

    /// <inheritdoc/>
    public void Configure(Action<IConfigurator> configuration)
    {
        _app.Configure(configuration);
    }

    /// <inheritdoc/>
    public int Run(IEnumerable<string> args, CancellationToken cancellationToken = default)
    {
        return _app.Run(args, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<int> RunAsync(IEnumerable<string> args, CancellationToken cancellationToken = default)
    {
        return _app.RunAsync(args, cancellationToken);
    }

    internal Configurator GetConfigurator()
    {
        return _app.GetConfigurator();
    }

    /// <summary>
    /// Sets the description of the default command.
    /// </summary>
    /// <param name="description">The default command description.</param>
    /// <returns>The same <see cref="CommandApp{TDefaultCommand}"/> instance so that multiple calls can be chained.</returns>
    public CommandApp<TDefaultCommand> WithDescription(string description)
    {
        _defaultCommandConfigurator.WithDescription(description);
        return this;
    }

    /// <summary>
    /// Sets data that will be passed to the command via the <see cref="CommandContext"/>.
    /// </summary>
    /// <param name="data">The data to pass to the default command.</param>
    /// <returns>The same <see cref="CommandApp{TDefaultCommand}"/> instance so that multiple calls can be chained.</returns>
    public CommandApp<TDefaultCommand> WithData(object data)
    {
        _defaultCommandConfigurator.WithData(data);
        return this;
    }
}