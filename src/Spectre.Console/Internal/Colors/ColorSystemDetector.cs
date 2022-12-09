namespace Spectre.Console;

internal static class ColorSystemDetector
{
    // Adapted from https://github.com/willmcgugan/rich/blob/f0c29052c22d1e49579956a9207324d9072beed7/rich/console.py#L391
    public static ColorSystem Detect(bool supportsAnsi)
    {
        // No colors?
        if (Environment.GetEnvironmentVariables().Contains("NO_COLOR"))
        {
            return ColorSystem.NoColors;
        }

        // Windows?
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
            if (GetWindowsVersionInformation(out var major, out var build))
            {
                if (major == 10 && build >= 15063)
                {
                    return ColorSystem.TrueColor;
                }
                else if (major > 10)
                {
                    return ColorSystem.TrueColor;
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

        // Should we default to eight-bit colors?
        return ColorSystem.EightBit;
    }

    private static bool GetWindowsVersionInformation(out int major, out int build)
    {
        major = 0;
        build = 0;

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

#if NET6_0_OR_GREATER
        // The reason we're not always using this, is because it will return wrong values on other runtimes than .NET 6+
        // See https://docs.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/5.0/environment-osversion-returns-correct-version
        var version = Environment.OSVersion.Version;
        major = version.Major;
        build = version.Build;
        return true;
#else
        var regex = new System.Text.RegularExpressions.Regex("Microsoft Windows (?'major'[0-9]*).(?'minor'[0-9]*).(?'build'[0-9]*)\\s*$");
        var match = regex.Match(RuntimeInformation.OSDescription);
        if (match.Success && int.TryParse(match.Groups["major"].Value, out major))
        {
            if (int.TryParse(match.Groups["build"].Value, out build))
            {
                return true;
            }
        }

        return false;
#endif
    }
}