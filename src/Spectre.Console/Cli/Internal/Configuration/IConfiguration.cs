using System.Collections.Generic;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents a configuration.
    /// </summary>
    internal interface IConfiguration
    {
        /// <summary>
        /// Gets the configured commands.
        /// </summary>
        IList<ConfiguredCommand> Commands { get; }

        /// <summary>
        /// Gets the settings for the configuration.
        /// </summary>
        CommandAppSettings Settings { get; }

        /// <summary>
        /// Gets the default command for the configuration.
        /// </summary>
        ConfiguredCommand? DefaultCommand { get; }

        /// <summary>
        /// Gets all examples for the configuration.
        /// </summary>
        IList<string[]> Examples { get; }
    }
}