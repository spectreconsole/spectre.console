using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Spectre.Console.Internal
{
    internal static class ColorSystemDetector
    {
        // Adapted from https://github.com/willmcgugan/rich/blob/f0c29052c22d1e49579956a9207324d9072beed7/rich/console.py#L391
        // which I think is battletested enough to trust.
        public static ColorSystem Detect(bool supportsAnsi)
        {
            if (Environment.GetEnvironmentVariables().Contains("NO_COLOR"))
            {
                // No colors supported
                return ColorSystem.NoColors;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (supportsAnsi)
                {
                    var regex = new Regex("^Microsoft Windows (?'major'[0-9]*).(?'minor'[0-9]*).(?'build'[0-9]*)\\s*$");
                    var match = regex.Match(RuntimeInformation.OSDescription);
                    if (match.Success && int.TryParse(match.Groups["major"].Value, out var major))
                    {
                        if (major > 10)
                        {
                            // Future Patrik will thank me.
                            return ColorSystem.TrueColor;
                        }

                        if (major == 10 && int.TryParse(match.Groups["build"].Value, out var build) && build >= 15063)
                        {
                            return ColorSystem.TrueColor;
                        }
                    }
                }
            }
            else
            {
                var colorTerm = Environment.GetEnvironmentVariable("COLORTERM");
                if (!string.IsNullOrWhiteSpace(colorTerm))
                {
                    if (colorTerm.Equals("truecolor", StringComparison.OrdinalIgnoreCase) ||
                       colorTerm.Equals("24bit", StringComparison.OrdinalIgnoreCase))
                    {
                        return ColorSystem.TrueColor;
                    }
                }
            }

            return ColorSystem.EightBit;
        }
    }
}
