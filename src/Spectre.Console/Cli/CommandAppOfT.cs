using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// The entry point for a command line application with a default command.
    /// </summary>
    /// <typeparam name="TDefaultCommand">The type of the default command.</typeparam>
    public sealed class CommandApp<TDefaultCommand> : ICommandApp
        where TDefaultCommand : class, ICommand
    {
        private readonly CommandApp _app;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandApp{TDefaultCommand}"/> class.
        /// </summary>
        /// <param name="registrar">The registrar.</param>
        public CommandApp(ITypeRegistrar? registrar = null)
        {
            _app = new CommandApp(registrar);
            _app.GetConfigurator().SetDefaultCommand<TDefaultCommand>();
        }

        /// <summary>
        /// Configures the command line application.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void Configure(Action<IConfigurator> configuration)
        {
            _app.Configure(configuration);
        }

        /// <summary>
        /// Runs the command line application with specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code from the executed command.</returns>
        public int Run(IEnumerable<string> args)
        {
            return _app.Run(args);
        }

        /// <summary>
        /// Runs the command line application with specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code from the executed command.</returns>
        public Task<int> RunAsync(IEnumerable<string> args)
        {
            return _app.RunAsync(args);
        }
    }
}
