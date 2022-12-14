using System;
using System.Threading;
using Spectre.Console;

namespace Paths;

public static class Program
{
    public static void Main()
    {
        var windowsPath = @"C:\This is\A\Super Long\Windows\Path\That\Goes\On And On\And\Never\Seems\To\Stop\But\At\Some\Point\It\Must\I\Guess.txt";
        var unixPath = @"//This is/A/Super Long/Unix/Path/That/Goes/On And On/And/Never/Seems/To/Stop/But/At/Some/Point/It/Must/I/Guess.txt";

        AnsiConsole.WriteLine();
        WritePlain(windowsPath, unixPath);

        AnsiConsole.WriteLine();
        WriteColorized(windowsPath, unixPath);

        AnsiConsole.WriteLine();
        WriteAligned(windowsPath);
    }

    private static void WritePlain(string windowsPath, string unixPath)
    {
        var table = new Table().BorderColor(Color.Grey).Title("Plain").RoundedBorder();
        table.AddColumns("[grey]OS[/]", "[grey]Path[/]");
        table.AddRow(new Text("Windows"), new TextPath(windowsPath));
        table.AddRow(new Text("Unix"), new TextPath(unixPath));

        AnsiConsole.Write(table);
    }

    private static void WriteColorized(string windowsPath, string unixPath)
    {
        var table = new Table().BorderColor(Color.Grey).Title("Colorized").RoundedBorder();
        table.AddColumns("[grey]OS[/]", "[grey]Path[/]");

        table.AddRow(new Text("Windows"),
            new TextPath(windowsPath)
                .RootColor(Color.Blue)
                .SeparatorColor(Color.Yellow)
                .StemColor(Color.Red)
                .LeafColor(Color.Green));

        table.AddRow(new Text("Unix"),
            new TextPath(unixPath)
                .RootColor(Color.Blue)
                .SeparatorColor(Color.Yellow)
                .StemColor(Color.Red)
                .LeafColor(Color.Green));

        AnsiConsole.Write(table);
    }

    private static void WriteAligned(string path)
    {
        var table = new Table().BorderColor(Color.Grey).Title("Aligned").RoundedBorder();
        table.AddColumns("[grey]Alignment[/]", "[grey]Path[/]");

        table.AddRow(new Text("Left"), new TextPath(path).LeftJustified());
        table.AddRow(new Text("Center"), new TextPath(path).Centered());
        table.AddRow(new Text("Right"), new TextPath(path).RightJustified());

        AnsiConsole.Write(table);
    }
}
