using System;
using System.Globalization;

namespace Spectre.Console.Internal
{
    internal static class DayOfWeekExtensions
    {
        public static string GetAbbreviatedDayName(this DayOfWeek day, CultureInfo culture)
        {
            culture ??= CultureInfo.InvariantCulture;
            var name = culture.DateTimeFormat.GetAbbreviatedDayName(day);

            if (name.Length > 0 && char.IsLower(name[0]))
            {
                name = string.Format(culture, "{0}{1}", char.ToUpper(name[0], culture), name.Substring(1));
            }

            return name;
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
