namespace Spectre.Console;

/// <summary>
/// Defines how the width value will be interpreted.
/// </summary>
public enum SizeMode
{
    /// <summary>Fixed size.</summary>
    Fixed = 1,

    /// <summary>Proportional sizing.</summary>
    Star = 2,

    /// <summary>Auto-size width to content.</summary>
    SizeToContent = 0,
}