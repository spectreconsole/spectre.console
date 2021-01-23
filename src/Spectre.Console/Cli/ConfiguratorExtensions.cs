using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Contains extensions for <see cref="IConfigurator"/>
    /// and <see cref="IConfigurator{TSettings}"/>.
    /// </summary>
    public static class ConfiguratorExtensions
    {
        /// <summary>
        /// Sets the name of the application.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The name of the application.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator SetApplicationName(this IConfigurator configurator, string name)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.Settings.ApplicationName = name;
            return configurator;
        }

        /// <summary>
        /// Overrides the auto-detected version of the application.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="version">The version of application.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator SetApplicationVersion(this IConfigurator configurator, string version)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.Settings.ApplicationVersion = version;
            return configurator;
        }

        /// <summary>
        /// Configures the console.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="console">The console.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator ConfigureConsole(this IConfigurator configurator, IAnsiConsole console)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.Settings.Console = console;
            return configurator;
        }

        /// <summary>
        /// Sets the parsing mode to strict.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator UseStrictParsing(this IConfigurator configurator)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.Settings.StrictParsing = true;
            return configurator;
        }

        /// <summary>
        /// Tells the command line application to propagate all
        /// exceptions to the user.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator PropagateExceptions(this IConfigurator configurator)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.Settings.PropagateExceptions = true;
            return configurator;
        }

        /// <summary>
        /// Configures case sensitivity.
        /// </summary>
        /// <param name="configurator">The configuration.</param>
        /// <param name="sensitivity">The case sensitivity.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator CaseSensitivity(this IConfigurator configurator, CaseSensitivity sensitivity)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.Settings.CaseSensitivity = sensitivity;
            return configurator;
        }

        /// <summary>
        /// Tells the command line application to validate all
        /// examples before running the application.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator ValidateExamples(this IConfigurator configurator)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.Settings.ValidateExamples = true;
            return configurator;
        }

        /// <summary>
        /// Sets the command interceptor to be used.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="interceptor">A <see cref="ICommandInterceptor"/>.</param>
        /// <returns>A configurator that can be used to configure the application further.</returns>
        public static IConfigurator SetInterceptor(this IConfigurator configurator, ICommandInterceptor interceptor)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.Settings.Interceptor = interceptor;
            return configurator;
        }

        /// <summary>
        /// Adds a command branch.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The name of the command branch.</param>
        /// <param name="action">The command branch configuration.</param>
        public static void AddBranch(
            this IConfigurator configurator,
            string name,
            Action<IConfigurator<CommandSettings>> action)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.AddBranch(name, action);
        }

        /// <summary>
        /// Adds a command branch.
        /// </summary>
        /// <typeparam name="TSettings">The command setting type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The name of the command branch.</param>
        /// <param name="action">The command branch configuration.</param>
        public static void AddBranch<TSettings>(
            this IConfigurator<TSettings> configurator,
            string name,
            Action<IConfigurator<TSettings>> action)
                where TSettings : CommandSettings
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.AddBranch(name, action);
        }

        /// <summary>
        /// Adds a command without settings that executes a delegate.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The name of the command.</param>
        /// <param name="func">The delegate to execute as part of command execution.</param>
        /// <returns>A command configurator that can be used to configure the command further.</returns>
        public static ICommandConfigurator AddDelegate(
            this IConfigurator configurator,
            string name,
            Func<CommandContext, int> func)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            return configurator.AddDelegate<EmptyCommandSettings>(name, (c, _) => func(c));
        }

        /// <summary>
        /// Adds a command without settings that executes a delegate.
        /// </summary>
        /// <typeparam name="TSettings">The command setting type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="name">The name of the command.</param>
        /// <param name="func">The delegate to execute as part of command execution.</param>
        /// <returns>A command configurator that can be used to configure the command further.</returns>
        public static ICommandConfigurator AddDelegate<TSettings>(
            this IConfigurator<TSettings> configurator,
            string name,
            Func<CommandContext, int> func)
                where TSettings : CommandSettings
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            return configurator.AddDelegate<TSettings>(name, (c, _) => func(c));
        }
    }
}
