namespace Spectre.Console;

/// <summary>
/// Determines possible file size base prefixes.  (base 2/base 10).
/// </summary>
public enum FileSizeBase
{
    /// <summary>
    /// The SI prefix definition (base 10) of kilobyte, megabyte, etc.
    /// </summary>
    Decimal = 1000,

    /// <summary>
    /// The IEC binary prefix definition (base 2) of kibibyte, mebibyte, etc.
    /// </summary>
    Binary = 1024,
}