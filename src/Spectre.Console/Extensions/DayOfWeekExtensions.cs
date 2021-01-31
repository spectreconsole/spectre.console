using System;
using System.Globalization;

namespace Spectre.Console
{
    internal static class DayOfWeekExtensions
    {
        public static string GetAbbreviatedDayName(this DayOfWeek day, CultureInfo culture)
        {
            culture ??= CultureInfo.InvariantCulture;
            return culture.DateTimeFormat
                .GetAbbreviatedDayName(day)
                .CapitalizeFirstLetter(culture);
        }

        public static DayOfWeek GetNextWeekDay(this DayOfWeek day)
        {
            var next = (int)day + 1;
            if (next > (int)DayOfWeek.Saturday)
            {
                return DayOfWeek.Sunday;
            }

            return (DayOfWeek)next;
        }
    }
}
