using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// The entry point for a command line application.
    /// </summary>
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

        /// <summary>
        /// Configures the command line application.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
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
        public void SetDefaultCommand<TCommand>()
            where TCommand : class, ICommand
        {
            GetConfigurator().SetDefaultCommand<TCommand>();
        }

        /// <summary>
        /// Runs the command line application with specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code from the executed command.</returns>
        public int Run(IEnumerable<string> args)
        {
            return RunAsync(args).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Runs the command line application with specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code from the executed command.</returns>
        public async Task<int> RunAsync(IEnumerable<string> args)
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
                    });

                    _executed = true;
                }

                return await _executor
                    .Execute(_configurator, args)
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
                        .LineBreak()
                        .Text("[red]Error:[/]")
                        .Space().Text(ex.Message.EscapeMarkup()),
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
}
