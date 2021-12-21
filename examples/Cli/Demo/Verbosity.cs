using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Demo;

public enum Verbosity
{
    Quiet,
    Minimal,
    Normal,
    Detailed,
    Diagnostic
}

public sealed class VerbosityConverter : TypeConverter
{
    private readonly Dictionary<string, Verbosity> _lookup;

    public VerbosityConverter()
    {
        _lookup = new Dictionary<string, Verbosity>(StringComparer.OrdinalIgnoreCase)
            {
                { "q", Verbosity.Quiet },
                { "quiet", Verbosity.Quiet },
                { "m", Verbosity.Minimal },
                { "minimal", Verbosity.Minimal },
                { "n", Verbosity.Normal },
                { "normal", Verbosity.Normal },
                { "d", Verbosity.Detailed },
                { "detailed", Verbosity.Detailed },
                { "diag", Verbosity.Diagnostic },
                { "diagnostic", Verbosity.Diagnostic }
            };
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string stringValue)
        {
            var result = _lookup.TryGetValue(stringValue, out var verbosity);
            if (!result)
            {
                const string format = "The value '{0}' is not a valid verbosity.";
                var message = string.Format(CultureInfo.InvariantCulture, format, value);
                throw new InvalidOperationException(message);
            }
            return verbosity;
        }
        throw new NotSupportedException("Can't convert value to verbosity.");
    }
}
