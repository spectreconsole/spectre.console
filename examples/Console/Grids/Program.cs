using Spectre.Console;

namespace Grids;

public static class Program
{
    public static void Main()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Usage: [grey]dotnet [blue]run[/] [[options]] [[[[--]] <additional arguments>...]]]][/]");
        AnsiConsole.WriteLine();

        var grid = new Grid();
        grid.AddColumn(new GridColumn().NoWrap());
        grid.AddColumn(new GridColumn().PadLeft(2));
        grid.AddRow("Options:");
        grid.AddRow("  [blue]-h[/], [blue]--help[/]", "Show command line help.");
        grid.AddRow("  [blue]-c[/], [blue]--configuration[/] <CONFIGURATION>", "The configuration to run for.");
        grid.AddRow("  [blue]-v[/], [blue]--verbosity[/] <LEVEL>", "Set the [grey]MSBuild[/] verbosity level.");

        AnsiConsole.Write(grid);
    }
}
