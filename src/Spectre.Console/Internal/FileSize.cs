namespace Spectre.Console;

internal struct FileSize
{
    public double Bytes { get; }
    public double Bits => Bytes * 8;

    public FileSizePrefix Prefix { get; } = FileSizePrefix.None;

    private readonly FileSizeBase _prefixBase = FileSizeBase.Binary;

    /// <summary>
    /// If enabled, will display the output in bits, rather than bytes.
    /// </summary>
    private readonly bool _showBits = false;

    public string Suffix => GetSuffix();

    public FileSize(double bytes)
    {
        Bytes = bytes;
        Prefix = DetectPrefix(bytes);
    }

    public FileSize(double bytes, FileSizeBase @base)
    {
        Bytes = bytes;
        _prefixBase = @base;
        Prefix = DetectPrefix(bytes);
    }

    public FileSize(double bytes, FileSizeBase @base, bool showBits)
    {
        Bytes = bytes;
        _showBits = showBits;

        _prefixBase = @base;
        Prefix = DetectPrefix(bytes);
    }

    public FileSize(double bytes, FileSizePrefix prefix)
    {
        Bytes = bytes;
        Prefix = prefix;
    }

    public FileSize(double bytes, FileSizePrefix prefix, FileSizeBase @base, bool showBits)
    {
        Bytes = bytes;
        _showBits = showBits;

        _prefixBase = @base;
        Prefix = prefix;
    }

    public string Format(CultureInfo? culture = null)
    {
        var unitBase = Math.Pow((int)_prefixBase, (int)Prefix);

        if (_showBits)
        {
            var bits = Bits / unitBase;
            return Prefix == FileSizePrefix.None ?
                ((int)bits).ToString(culture ?? CultureInfo.InvariantCulture)
                : bits.ToString("F1", culture ?? CultureInfo.InvariantCulture);
        }

        var bytes = Bytes / unitBase;
        return Prefix == FileSizePrefix.None ?
            ((int)bytes).ToString(culture ?? CultureInfo.InvariantCulture)
            : bytes.ToString("F1", culture ?? CultureInfo.InvariantCulture);
    }

    public override string ToString()
    {
        return ToString(suffix: true, CultureInfo.InvariantCulture);
    }

    public string ToString(bool suffix = true, CultureInfo? culture = null)
    {
        if (suffix)
        {
            return $"{Format(culture)} {Suffix}";
        }

        return Format(culture);
    }

    private string GetSuffix()
    {
        return (Bytes, Unit: Prefix, PrefixBase: _prefixBase, ShowBits: _showBits) switch
        {
            (_, FileSizePrefix.Kilo, FileSizeBase.Binary, false) => "KiB",
            (_, FileSizePrefix.Mega, FileSizeBase.Binary, false) => "MiB",
            (_, FileSizePrefix.Giga, FileSizeBase.Binary, false) => "GiB",
            (_, FileSizePrefix.Tera, FileSizeBase.Binary, false) => "TiB",
            (_, FileSizePrefix.Peta, FileSizeBase.Binary, false) => "PiB",
            (_, FileSizePrefix.Exa, FileSizeBase.Binary, false) => "EiB",
            (_, FileSizePrefix.Zetta, FileSizeBase.Binary, false) => "ZiB",
            (_, FileSizePrefix.Yotta, FileSizeBase.Binary, false) => "YiB",

            (_, FileSizePrefix.Kilo, FileSizeBase.Binary, true) => "Kibit",
            (_, FileSizePrefix.Mega, FileSizeBase.Binary, true) => "Mibit",
            (_, FileSizePrefix.Giga, FileSizeBase.Binary, true) => "Gibit",
            (_, FileSizePrefix.Tera, FileSizeBase.Binary, true) => "Tibit",
            (_, FileSizePrefix.Peta, FileSizeBase.Binary, true) => "Pibit",
            (_, FileSizePrefix.Exa, FileSizeBase.Binary, true) => "Eibit",
            (_, FileSizePrefix.Zetta, FileSizeBase.Binary, true) => "Zibit",
            (_, FileSizePrefix.Yotta, FileSizeBase.Binary, true) => "Yibit",

            (_, FileSizePrefix.Kilo, FileSizeBase.Decimal, false) => "KB",
            (_, FileSizePrefix.Mega, FileSizeBase.Decimal, false) => "MB",
            (_, FileSizePrefix.Giga, FileSizeBase.Decimal, false) => "GB",
            (_, FileSizePrefix.Tera, FileSizeBase.Decimal, false) => "TB",
            (_, FileSizePrefix.Peta, FileSizeBase.Decimal, false) => "PB",
            (_, FileSizePrefix.Exa, FileSizeBase.Decimal, false) => "EB",
            (_, FileSizePrefix.Zetta, FileSizeBase.Decimal, false) => "ZB",
            (_, FileSizePrefix.Yotta, FileSizeBase.Decimal, false) => "YB",

            (_, FileSizePrefix.Kilo, FileSizeBase.Decimal, true) => "Kbit",
            (_, FileSizePrefix.Mega, FileSizeBase.Decimal, true) => "Mbit",
            (_, FileSizePrefix.Giga, FileSizeBase.Decimal, true) => "Gbit",
            (_, FileSizePrefix.Tera, FileSizeBase.Decimal, true) => "Tbit",
            (_, FileSizePrefix.Peta, FileSizeBase.Decimal, true) => "Pbit",
            (_, FileSizePrefix.Exa, FileSizeBase.Decimal, true) => "Ebit",
            (_, FileSizePrefix.Zetta, FileSizeBase.Decimal, true) => "Zbit",
            (_, FileSizePrefix.Yotta, FileSizeBase.Decimal, true) => "Ybit",

            (1, _, _, true) => "bit",
            (_, _, _, true) => "bits",
            (1, _, _, false) => "byte",
            (_, _, _, false) => "bytes",
        };
    }

    private FileSizePrefix DetectPrefix(double bytes)
    {
        if (_showBits)
        {
            bytes *= 8;
        }

        foreach (var prefix in EnumUtils.GetValues<FileSizePrefix>())
        {
            // Trying to find the largest unit, that the number of bytes can fit under. Ex. 40kb < 1mb
            if (bytes < Math.Pow((int)_prefixBase, (int)prefix + 1))
            {
                return prefix;
            }
        }

        return FileSizePrefix.None;
    }
}