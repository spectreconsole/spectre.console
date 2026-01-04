namespace Spectre.Console;

internal static class DayOfWeekExtensions
{
    extension(DayOfWeek day)
    {
        public string GetAbbreviatedDayName(CultureInfo culture)
        {
            culture ??= CultureInfo.InvariantCulture;
            return culture.DateTimeFormat
                .GetAbbreviatedDayName(day)
                .CapitalizeFirstLetter(culture);
        }

        public DayOfWeek GetNextWeekDay()
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