using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Spectre.Console
{
    /// <summary>
    /// Factory for creating an ANSI console.
    /// </summary>
    public sealed class AnsiConsoleFactory
    {
        private readonly List<IProfileEnricher> _enrichers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiConsoleFactory"/> class.
        /// </summary>
        public AnsiConsoleFactory()
        {
            _enrichers = new List<IProfileEnricher>
            {
                new AppVeyorProfile(),
                new BambooProfile(),
                new BitbucketProfile(),
                new BitriseProfile(),
                new ContinuaCIProfile(),
                new GitHubProfile(),
                new GitLabProfile(),
                new GoCDProfile(),
                new JenkinsProfile(),
                new MyGetProfile(),
                new TeamCityProfile(),
                new TfsProfile(),
                new TravisProfile(),
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiConsoleFactory"/> class.
        /// </summary>
        /// <param name="enrichers">The profile enrichers to use.</param>
        public AnsiConsoleFactory(IEnumerable<IProfileEnricher> enrichers)
        {
            _enrichers = new List<IProfileEnricher>(enrichers ?? Enumerable.Empty<IProfileEnricher>());
        }

        /// <summary>
        /// Creates a new <see cref="AnsiConsoleFactory"/> without default profile enrichers.
        /// </summary>
        /// <returns>A new <see cref="AnsiConsoleFactory"/> without default profile enrichers.</returns>
        public static AnsiConsoleFactory NoEnrichers()
        {
            return new AnsiConsoleFactory(Enumerable.Empty<IProfileEnricher>());
        }

        /// <summary>
        /// Creates an ANSI console.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>An implementation of <see cref="IAnsiConsole"/>.</returns>
        public IAnsiConsole Create(AnsiConsoleSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var buffer = settings.Out ?? System.Console.Out;

            // Detect if the terminal support ANSI or not
            var (supportsAnsi, legacyConsole) = DetectAnsi(settings, buffer);

            // Use the provided encoding or fall back to UTF-8
            var encoding = buffer.IsStandardOut() ? System.Console.OutputEncoding : Encoding.UTF8;

            // Get the color system
            var colorSystem = settings.ColorSystem == ColorSystemSupport.Detect
                ? ColorSystemDetector.Detect(supportsAnsi)
                : (ColorSystem)settings.ColorSystem;

            // Get whether or not we consider the terminal interactive
            var interactive = settings.Interactive == InteractionSupport.Yes;
            if (settings.Interactive == InteractionSupport.Detect)
            {
                interactive = Environment.UserInteractive;
            }

            var profile = new Profile("Default", buffer, encoding)
            {
                ColorSystem = colorSystem,
            };

            profile.Capabilities.Ansi = supportsAnsi;
            profile.Capabilities.Links = supportsAnsi && !legacyConsole;
            profile.Capabilities.Legacy = legacyConsole;
            profile.Capabilities.Interactive = interactive;

            // Enrich the profile
            var variables = GetEnvironmentVariables(settings);
            var customEnrichers = settings.Enrichers ?? Enumerable.Empty<IProfileEnricher>();
            foreach (var enricher in _enrichers.Concat(customEnrichers))
            {
                if (enricher.Enabled(variables))
                {
                    enricher.Enrich(profile);
                }
            }

            return new AnsiConsoleFacade(profile);
        }

        private static IDictionary<string, string> GetEnvironmentVariables(AnsiConsoleSettings settings)
        {
            if (settings.EnvironmentVariables != null)
            {
                return new Dictionary<string, string>(settings.EnvironmentVariables, StringComparer.OrdinalIgnoreCase);
            }

            return Environment.GetEnvironmentVariables()
                .Cast<System.Collections.DictionaryEntry>()
                .Aggregate(
                    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                    (dictionary, entry) =>
                    {
                        var key = (string)entry.Key;
                        if (!dictionary.TryGetValue(key, out _))
                        {
                            dictionary.Add(key, entry.Value as string ?? string.Empty);
                        }

                        return dictionary;
                    },
                    dictionary => dictionary);
        }

        private static (bool Ansi, bool Legacy) DetectAnsi(AnsiConsoleSettings settings, System.IO.TextWriter buffer)
        {
            var supportsAnsi = settings.Ansi == AnsiSupport.Yes;
            var legacyConsole = false;

            if (settings.Ansi == AnsiSupport.Detect)
            {
                (supportsAnsi, legacyConsole) = AnsiDetector.Detect(true);

                // Check whether or not this is a legacy console from the existing instance (if any).
                // We need to do this because once we upgrade the console to support ENABLE_VIRTUAL_TERMINAL_PROCESSING
                // on Windows, there is no way of detecting whether or not we're running on a legacy console or not.
                if (AnsiConsole.Created && !legacyConsole && buffer.IsStandardOut() && AnsiConsole.Profile.Capabilities.Legacy)
                {
                    legacyConsole = AnsiConsole.Profile.Capabilities.Legacy;
                }
            }
            else
            {
                if (buffer.IsStandardOut())
                {
                    // Are we running on Windows?
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        // Not the first console we're creating?
                        if (AnsiConsole.Created)
                        {
                            legacyConsole = AnsiConsole.Profile.Capabilities.Legacy;
                        }
                        else
                        {
                            // Try detecting whether or not this
                            (_, legacyConsole) = AnsiDetector.Detect(false);
                        }
                    }
                }
            }

            return (supportsAnsi, legacyConsole);
        }
    }
}
