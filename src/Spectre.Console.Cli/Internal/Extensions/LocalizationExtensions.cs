namespace Spectre.Console.Cli;

internal static class LocalizationExtensions
{
    public static string? LocalizedDescription(this MemberInfo me)
    {
        var description = me.GetCustomAttribute<DescriptionAttribute>();
        if (description is null)
        {
            return null;
        }

        var localization = me.GetCustomAttribute<LocalizationAttribute>();
        string? localizedText;
        return (localizedText = localization?.ResourceClass
            .GetProperty(description.Description)?
            .GetValue(default) as string) != null
            ? localizedText
            : description.Description;
    }
}