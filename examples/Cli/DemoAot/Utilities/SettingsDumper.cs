using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DemoAot.Utilities;

public static class SettingsDumper
{
    public static void Dump<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(T settings)
        where T : CommandSettings
    {
        var table = new Table().RoundedBorder();
        table.AddColumn("[grey]Name[/]");
        table.AddColumn("[grey]Value[/]");

        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(settings)
                ?.ToString()
                ?.EscapeMarkup();

            table.AddRow(
                property.Name,
                value ?? "[grey]null[/]");
        }

        AnsiConsole.Write(table);
    }
}
