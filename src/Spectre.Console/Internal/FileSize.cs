using System;
using System.Globalization;

namespace Spectre.Console
{
    internal struct FileSize
    {
        public double Bytes { get; }
        public FileSizeUnit Unit { get; }
        public string Suffix => GetSuffix();

        public FileSize(double bytes)
        {
            Bytes = bytes;
            Unit = Detect(bytes);
        }

        public FileSize(double bytes, FileSizeUnit unit)
        {
            Bytes = bytes;
            Unit = unit;
        }

        public string Format(CultureInfo? culture = null)
        {
            var @base = GetBase(Unit);
            if (@base == 0)
            {
                @base = 1;
            }

            var bytes = Bytes / @base;

            return Unit == FileSizeUnit.Byte
                ? ((int)bytes).ToString(culture ?? CultureInfo.InvariantCulture)
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
            return (Bytes, Unit) switch
            {
                (_, FileSizeUnit.KiloByte) => "KB",
                (_, FileSizeUnit.MegaByte) => "MB",
                (_, FileSizeUnit.GigaByte) => "GB",
                (_, FileSizeUnit.TeraByte) => "TB",
                (_, FileSizeUnit.PetaByte) => "PB",
                (_, FileSizeUnit.ExaByte) => "EB",
                (_, FileSizeUnit.ZettaByte) => "ZB",
                (_, FileSizeUnit.YottaByte) => "YB",
                (1, _) => "byte",
                (_, _) => "bytes",
            };
        }

        private static FileSizeUnit Detect(double bytes)
        {
            foreach (var unit in (FileSizeUnit[])Enum.GetValues(typeof(FileSizeUnit)))
            {
                if (bytes < (GetBase(unit) * 1024))
                {
                    return unit;
                }
            }

            return FileSizeUnit.Byte;
        }

        private static double GetBase(FileSizeUnit unit)
        {
            return Math.Pow(1024, (int)unit);
        }
    }
}
