using DocExampleGenerator;
using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class TextPathSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (40, 23);

        public override void Run(IAnsiConsole console)
        {
            console.Write(
                new Panel(
                    new Padder(new TextPath("C:/This/Is/A/Super/Long/Path/That/Will/Be/Truncated.txt"), new Padding(0,1)))
                .BorderStyle(new Style(foreground: Color.Grey))
                .Header("Windows path"));

            console.Write(
                new Panel(
                    new Padder(new TextPath("/This/Is/A/Super/Long/Path/That/Will/Be/Truncated.txt"), new Padding(0,1)))
                .BorderStyle(new Style(foreground: Color.Grey))
                .Header("Unix path"));

            console.Write(
                new Panel(
                    new Padder(new TextPath("/This/Is/A/Long/Path/That/Will/Be/Truncated.txt")
                        .RootColor(Color.Green)
                        .SeparatorColor(Color.Red)
                        .StemColor(Color.Yellow)
                        .LeafColor(Color.Blue), new Padding(0,1)))
                .BorderStyle(new Style(foreground: Color.Grey))
                .Header("Styling"));

            console.Write(
                new Panel(
                    new Padder(new Rows(
                        new TextPath("/This/Is/A/Long/Path/That/Will/Be/Truncated.txt").LeftJustified(),
                        new TextPath("/This/Is/A/Long/Path/That/Will/Be/Truncated.txt").Centered(),
                        new TextPath("/This/Is/A/Long/Path/That/Will/Be/Truncated.txt").RightJustified()), new Padding(0,1)))
                .BorderStyle(new Style(foreground: Color.Grey))
                .Header("Alignment"));
        }
    }
}