using System;
using System.Threading;
using Spectre.Console;

namespace Live;

public static class Program
{
    public static void Main()
    {
        var windowsPath = @"C:\This is\A\Super Long\Windows\Path\That\Goes\On And On\And\Never\Seems\To\Stop\But\At\Some\Point\It\Must\I\Guess.txt";
        var unixPath = @"//This is/A/Super Long/Unix/Path/That/Goes/On And On/And/Never/Seems/To/Stop/But/At/Some/Point/It/Must/I/Guess.txt";

        var table = new Table().BorderColor(Color.Grey);
        table.AddColumns("[grey]OS[/]", "[grey]Path[/]");

        table.AddRow(new Text("Windows"),
            new TextPath(windowsPath));

        table.AddRow(new Text("Unix"),
            new TextPath(unixPath)
                .RootColor(Color.Blue)
                .SeparatorColor(Color.Yellow)
                .StemStyle(Color.Red)
                .LeafStyle(Color.Green));

        AnsiConsole.Write(table);
    }
}
