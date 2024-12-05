using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace Spectre.Console.TrimTest.Utilities;

public static class SettingsDumper
{
    public static void Dump<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(CommandSettings settings)
    {
        var table = new Table().RoundedBorder();
        table.AddColumn("[grey]Name[/]");
        table.AddColumn("[grey]Value[/]");

        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var o = property.GetValue(settings);
            var value = o?.ToString().EscapeMarkup();

            table.AddRow(
                property.Name,
                value ?? "[grey]null[/]");
        }

        AnsiConsole.Write(table);
    }
}
