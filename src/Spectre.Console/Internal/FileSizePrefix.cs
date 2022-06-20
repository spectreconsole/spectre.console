namespace Spectre.Console;

internal enum FileSizePrefix
{
    None = 0,
    Kilo = 1,
    Mega = 2,
    Giga = 3,
    Tera = 4,
    Peta = 5,
    Exa = 6,
    Zetta = 7,
    Yotta = 8,
}

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