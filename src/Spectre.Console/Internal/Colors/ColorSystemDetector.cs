using System;

namespace Spectre.Console
{
    internal static class ColorSystemDetector
    {
        // Adapted from https://github.com/willmcgugan/rich/blob/f0c29052c22d1e49579956a9207324d9072beed7/rich/console.py#L391
        public static ColorSystem Detect(bool supportsAnsi)
        {
            // No colors?
            if (Environment.GetEnvironmentVariable("NO_COLOR") is not null)
            {
                return ColorSystem.NoColors;
            }

            // Windows?
            if (OperatingSystem.IsWindows())
            {
                if (!supportsAnsi)
                {
                    // Figure out what we should do here.
                    // Does really all Windows terminals support
                    // eight-bit colors? Probably not...
                    return ColorSystem.EightBit;
                }

                // Windows 10.0.15063 and above support true color,
                // and we can probably assume that the next major
                // version of Windows will support true color as well.
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 15063))
                {
                    return ColorSystem.TrueColor;
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

            // Should we default to eight-bit colors?
            return ColorSystem.EightBit;
        }
    }
}
