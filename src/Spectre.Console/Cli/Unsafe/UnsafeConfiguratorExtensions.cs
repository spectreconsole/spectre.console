using System.Linq;

namespace Spectre.Console.Cli.Unsafe
{
    /// <summary>
    /// Contains unsafe extensions for <see cref="IConfigurator"/>.
    /// </summary>
    public static class UnsafeConfiguratorExtensions
    {
        /// <summary>
        /// Gets an <see cref="IUnsafeConfigurator"/> that allows
        /// composition of commands without type safety.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <returns>An <see cref="IUnsafeConfigurator"/>.</returns>
        public static IUnsafeConfigurator SafetyOff(this IConfigurator configurator)
        {
            if (!(configurator is IUnsafeConfigurator @unsafe))
            {
                throw new CommandConfigurationException("Configurator does not support manual configuration");
            }

            return @unsafe;
        }

        /// <summary>
        /// Converts an <see cref="IUnsafeConfigurator"/> to
        /// a configurator with type safety.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <returns>An <see cref="IConfigurator"/>.</returns>
        public static IConfigurator SafetyOn(this IUnsafeConfigurator configurator)
        {
            if (!(configurator is IConfigurator safe))
            {
                throw new CommandConfigurationException("Configurator cannot be converted to a safe configurator.");
            }

            return safe;
        }

        /// <summary>
        /// Gets an <see cref="IUnsafeConfigurator"/> that allows
        /// composition of commands without type safety.
        /// </summary>
        /// <typeparam name="TSettings">The command settings.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns>An <see cref="IUnsafeConfigurator"/>.</returns>
        public static IUnsafeConfigurator SafetyOff<TSettings>(this IConfigurator<TSettings> configurator)
            where TSettings : CommandSettings
        {
            if (!(configurator is IUnsafeConfigurator @unsafe))
            {
                throw new CommandConfigurationException("Configurator does not support manual configuration");
            }

            return @unsafe;
        }

        /// <summary>
        /// Converts an <see cref="IUnsafeConfigurator"/> to
        /// a configurator with type safety.
        /// </summary>
        /// <typeparam name="TSettings">The command settings.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns>An <see cref="IConfigurator{TSettings}"/>.</returns>
        public static IConfigurator<TSettings> SafetyOn<TSettings>(this IUnsafeBranchConfigurator configurator)
            where TSettings : CommandSettings
        {
            if (!(configurator is IConfigurator<TSettings> safe))
            {
                throw new CommandConfigurationException($"Configurator cannot be converted to a safe configurator of type '{typeof(TSettings).Name}'.");
            }

            if (safe.GetType().GetGenericArguments().First() != typeof(TSettings))
            {
                throw new CommandConfigurationException($"Configurator cannot be converted to a safe configurator of type '{typeof(TSettings).Name}'.");
            }

            return safe;
        }
    }
}
