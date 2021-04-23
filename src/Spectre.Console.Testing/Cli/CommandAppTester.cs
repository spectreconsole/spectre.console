using System;
using Spectre.Console.Cli;

namespace Spectre.Console.Testing
{
    /// <summary>
    /// A <see cref="CommandApp"/> test harness.
    /// </summary>
    public sealed class CommandAppTester
    {
        private Action<CommandApp>? _appConfiguration;
        private Action<IConfigurator>? _configuration;

        /// <summary>
        /// Sets the default command.
        /// </summary>
        /// <typeparam name="T">The default command type.</typeparam>
        public void SetDefaultCommand<T>()
            where T : class, ICommand
        {
            _appConfiguration = (app) => app.SetDefaultCommand<T>();
        }

        /// <summary>
        /// Configures the command application.
        /// </summary>
        /// <param name="action">The configuration action.</param>
        public void Configure(Action<IConfigurator> action)
        {
            if (_configuration != null)
            {
                throw new InvalidOperationException("The command app harnest have already been configured.");
            }

            _configuration = action;
        }

        /// <summary>
        /// Runs the command application and expects an exception of a specific type to be thrown.
        /// </summary>
        /// <typeparam name="T">The expected exception type.</typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns>The information about the failure.</returns>
        public CommandAppFailure RunAndCatch<T>(params string[] args)
            where T : Exception
        {
            var console = new TestConsole().Width(int.MaxValue);

            try
            {
                Run(args, console, c => c.PropagateExceptions());
                throw new InvalidOperationException("Expected an exception to be thrown, but there was none.");
            }
            catch (T ex)
            {
                if (ex is CommandAppException commandAppException && commandAppException.Pretty != null)
                {
                    console.Write(commandAppException.Pretty);
                }
                else
                {
                    console.WriteLine(ex.Message);
                }

                return new CommandAppFailure(ex, console.Output);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Expected an exception of type '{typeof(T).FullName}' to be thrown, "
                    + $"but received {ex.GetType().FullName}.");
            }
        }

        /// <summary>
        /// Runs the command application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The result.</returns>
        public CommandAppResult Run(params string[] args)
        {
            var console = new TestConsole().Width(int.MaxValue);
            return Run(args, console);
        }

        private CommandAppResult Run(string[] args, TestConsole console, Action<IConfigurator>? config = null)
        {
            CommandContext? context = null;
            CommandSettings? settings = null;

            var app = new CommandApp();
            _appConfiguration?.Invoke(app);

            if (_configuration != null)
            {
                app.Configure(_configuration);
            }

            if (config != null)
            {
                app.Configure(config);
            }

            app.Configure(c => c.ConfigureConsole(console));
            app.Configure(c => c.SetInterceptor(new CallbackCommandInterceptor((ctx, s) =>
            {
                context = ctx;
                settings = s;
            })));

            var result = app.Run(args);

            var output = console.Output
                .NormalizeLineEndings()
                .TrimLines()
                .Trim();

            return new CommandAppResult(result, output, context, settings);
        }
    }
}
