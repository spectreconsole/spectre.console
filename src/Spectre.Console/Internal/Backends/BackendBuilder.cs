using System;
using System.Runtime.InteropServices;

namespace Spectre.Console.Internal
{
    internal static class BackendBuilder
    {
        public static IAnsiConsole Build(AnsiConsoleSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var buffer = settings.Out ?? System.Console.Out;

            var supportsAnsi = settings.Ansi == AnsiSupport.Yes;
            var legacyConsole = false;

            if (settings.Ansi == AnsiSupport.Detect)
            {
                (supportsAnsi, legacyConsole) = AnsiDetector.Detect(true);

                // Check whether or not this is a legacy console from the existing instance (if any).
                // We need to do this because once we upgrade the console to support ENABLE_VIRTUAL_TERMINAL_PROCESSING
                // on Windows, there is no way of detecting whether or not we're running on a legacy console or not.
                if (AnsiConsole.Created && !legacyConsole && buffer.IsStandardOut() && AnsiConsole.Capabilities.LegacyConsole)
                {
                    legacyConsole = AnsiConsole.Capabilities.LegacyConsole;
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
                            legacyConsole = AnsiConsole.Capabilities.LegacyConsole;
                        }
                        else
                        {
                            // Try detecting whether or not this
                            (_, legacyConsole) = AnsiDetector.Detect(false);
                        }
                    }
                }
            }

            var colorSystem = settings.ColorSystem == ColorSystemSupport.Detect
                ? ColorSystemDetector.Detect(supportsAnsi)
                : (ColorSystem)settings.ColorSystem;

            // Get the capabilities
            var capabilities = new Capabilities(supportsAnsi, colorSystem, legacyConsole);

            // Create the renderer
            if (supportsAnsi)
            {
                return new AnsiBackend(buffer, capabilities, settings.LinkIdentityGenerator);
            }
            else
            {
                return new FallbackBackend(buffer, capabilities);
            }
        }
    }
}
