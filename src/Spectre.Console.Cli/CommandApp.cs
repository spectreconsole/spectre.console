using Spectre.Console.Cli.Internal.Configuration;

namespace Spectre.Console.Cli;

/// <summary>
/// The entry point for a command line application.
/// </summary>
#if !NETSTANDARD2_0
[RequiresDynamicCode("Spectre.Console.Cli relies on reflection. Use during trimming and AOT compilation is not supported and may result in unexpected behaviors.")]
#endif
public sealed class CommandApp : ICommandApp
{
    private readonly Configurator _configurator;
    private readonly CommandExecutor _executor;
    private bool _executed;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandApp"/> class.
    /// </summary>
    /// <param name="registrar">The registrar.</param>
    public CommandApp(ITypeRegistrar? registrar = null)
    {
        registrar ??= new DefaultTypeRegistrar();

        _configurator = new Configurator(registrar);
        _executor = new CommandExecutor(registrar);
    }

    /// <inheritdoc/>
    public void Configure(Action<IConfigurator> configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        configuration(_configurator);
    }

    /// <summary>
    /// Sets the default command.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <returns>A <see cref="DefaultCommandConfigurator"/> that can be used to configure the default command.</returns>
    public DefaultCommandConfigurator SetDefaultCommand<TCommand>()
        where TCommand : class, ICommand
    {
        return new DefaultCommandConfigurator(GetConfigurator().SetDefaultCommand<TCommand>());
    }

    /// <inheritdoc/>
    public int Run(IEnumerable<string> args, CancellationToken cancellationToken = default)
    {
        return RunAsync(args, cancellationToken).GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    public async Task<int> RunAsync(IEnumerable<string> args, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_executed)
            {
                // Add built-in (hidden) commands.
                _configurator.AddBranch(CliConstants.Commands.Branch, cli =>
                {
                    cli.HideBranch();
                    cli.AddCommand<VersionCommand>(CliConstants.Commands.Version);
                    cli.AddCommand<XmlDocCommand>(CliConstants.Commands.XmlDoc);
                    cli.AddCommand<ExplainCommand>(CliConstants.Commands.Explain);
                    cli.AddCommand<OpenCliGeneratorCommand>(CliConstants.Commands.OpenCli);
                });

                _executed = true;
            }

            return await _executor
                .ExecuteAsync(_configurator, args, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Should we always propagate when debugging?
            if (Debugger.IsAttached
                && ex is CommandAppException appException
                && appException.AlwaysPropagateWhenDebugging)
            {
                throw;
            }

            if (_configurator.Settings.PropagateExceptions)
            {
                throw;
            }

            if (_configurator.Settings.ExceptionHandler != null)
            {
                return _configurator.Settings.ExceptionHandler(ex, null);
            }

            if (ex is OperationCanceledException)
            {
                return _configurator.Settings.CancellationExitCode;
            }

            // Render the exception.
            var pretty = GetRenderableErrorMessage(ex);
            if (pretty != null)
            {
                _configurator.Settings.Console.SafeRender(pretty);
            }

            return -1;
        }
    }

    internal Configurator GetConfigurator()
    {
        return _configurator;
    }

    private static List<IRenderable?>? GetRenderableErrorMessage(Exception ex, bool convert = true)
    {
        if (ex is CommandAppException renderable && renderable.Pretty != null)
        {
            return new List<IRenderable?> { renderable.Pretty };
        }

        if (convert)
        {
            var converted = new List<IRenderable?>
                {
                    new Composer()
                        .Text("[red]Error:[/]")
                        .Space()
                        .Text(ex.Message.EscapeMarkup())
                        .LineBreak(),
                };

            // Got a renderable inner exception?
            if (ex.InnerException != null)
            {
                var innerRenderable = GetRenderableErrorMessage(ex.InnerException, convert: false);
                if (innerRenderable != null)
                {
                    converted.AddRange(innerRenderable);
                }
            }

            return converted;
        }

        return null;
    }
}