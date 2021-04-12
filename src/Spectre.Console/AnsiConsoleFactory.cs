using System;
using System.Runtime.InteropServices;
using Spectre.Console.Enrichment;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// Factory for creating an ANSI console.
    /// </summary>
    public sealed class AnsiConsoleFactory
    {
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

            var output = settings.Out ?? new AnsiConsoleOutput(System.Console.Out);
            if (output.Writer == null)
            {
                throw new InvalidOperationException("Output writer was null");
            }

            // Detect if the terminal support ANSI or not
            var (supportsAnsi, legacyConsole) = DetectAnsi(settings, output.Writer);

            // Use console encoding or fall back to provided encoding
            var encoding = output.Writer.IsStandardOut() || output.Writer.IsStandardError()
                ? System.Console.OutputEncoding : output.Writer.Encoding;

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

            var profile = new Profile(output, encoding);

            profile.Capabilities.ColorSystem = colorSystem;
            profile.Capabilities.Ansi = supportsAnsi;
            profile.Capabilities.Links = supportsAnsi && !legacyConsole;
            profile.Capabilities.Legacy = legacyConsole;
            profile.Capabilities.Interactive = interactive;
            profile.Capabilities.Unicode = encoding.EncodingName.ContainsExact("Unicode");

            // Enrich the profile
            ProfileEnricher.Enrich(
                profile,
                settings.Enrichment,
                settings.EnvironmentVariables);

            return new AnsiConsoleFacade(
                profile,
                settings.ExclusivityMode ?? new DefaultExclusivityMode());
        }

        private static (bool Ansi, bool Legacy) DetectAnsi(AnsiConsoleSettings settings, System.IO.TextWriter buffer)
        {
            var supportsAnsi = settings.Ansi == AnsiSupport.Yes;
            var legacyConsole = false;

            if (settings.Ansi == AnsiSupport.Detect)
            {
                (supportsAnsi, legacyConsole) = AnsiDetector.Detect(buffer.IsStandardError(), true);

                // Check whether or not this is a legacy console from the existing instance (if any).
                // We need to do this because once we upgrade the console to support ENABLE_VIRTUAL_TERMINAL_PROCESSING
                // on Windows, there is no way of detecting whether or not we're running on a legacy console or not.
                if (AnsiConsole.Created && !legacyConsole && (buffer.IsStandardOut() || buffer.IsStandardError()) && AnsiConsole.Profile.Capabilities.Legacy)
                {
                    legacyConsole = AnsiConsole.Profile.Capabilities.Legacy;
                }
            }
            else
            {
                if (buffer.IsStandardOut() || buffer.IsStandardError())
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
                            // Try detecting whether or not this is a legacy console
                            (_, legacyConsole) = AnsiDetector.Detect(buffer.IsStandardError(), false);
                        }
                    }
                }
            }

            return (supportsAnsi, legacyConsole);
        }
    }
}
