using System;
using System.Threading;
using Spectre.Console;

namespace Live;

public static class Program
{
    public static void Main()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new TextPath(@"C:\Users\Patrik\Source\github\patriksvensson-forks\spectre.console\examples\Console\Paths"));
        AnsiConsole.WriteLine();

        var table = new Table().BorderColor(Color.Grey);
        table.AddColumns("[grey]Index[/]", "[yellow]Path[/]");
        table.AddRow(new Text("1"), new TextPath(@"C:\Users\Patrik\Source\github\patriksvensson-forks\spectre.console\examples\Console\Paths"));
        AnsiConsole.Write(table);
    }
}
