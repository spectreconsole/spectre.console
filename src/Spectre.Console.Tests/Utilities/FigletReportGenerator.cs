using System.Text;

namespace Spectre.Console.Tests;

public static class FigletReportGenerator
{
    private const string SampleText = "Hello World";

    public static string Generate()
    {
        var rendered = new StringBuilder();
        int loadOk = 0, fullOk = 0, fitOk = 0, smushOk = 0;
        var modes = new[]
        {
            FigletLayoutMode.FullSize, FigletLayoutMode.Fitted, FigletLayoutMode.Smushed,
        };

        var table = new Table();
        table.AddColumns("Font", "Load", "FullSize", "Fitted", "Smushed", "Note");

        var fontNames = FigletTestFontLoader.GetAllZipFontFileNames();
        foreach (var fontName in fontNames)
        {
            FigletFont font;
            try
            {
                font = FigletTestFontLoader.LoadZipFont(fontName);
                loadOk++;
            }
            catch (Exception ex)
            {
                table.AddRow(fontName, "FAIL", "-", "-", "-", ex.Message);
                continue;
            }

            var cells = new string[3];
            for (var mode = 0; mode < modes.Length; mode++)
            {
                try
                {
                    var output = RenderFont(font, (FigletLayoutMode)mode);
                    rendered.AppendLine($"===== {fontName} | {(FigletLayoutMode)mode} =====");
                    rendered.Append(output);
                    rendered.AppendLine();

                    var hasContent = output.Any(c => c != ' ' && c != '\r' && c != '\n');
                    cells[mode] = hasContent ? "ok" : "empty";
                    if (hasContent)
                    {
                        switch (mode)
                        {
                            case 0:
                                fullOk++;
                                break;
                            case 1:
                                fitOk++;
                                break;
                            case 2:
                                smushOk++;
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    cells[mode] = "FAIL: " + ex.Message;
                }
            }

            var note = font.Count == 0 ? "0 glyphs parsed" : $"{font.Count} glyphs";
            table.AddRow(fontName, "ok", cells[0], cells[1], cells[2], note);
        }

        using var console = new TestConsole().Width(256);

        console.WriteLine("Figlet Font Report");
        console.WriteLine("------------------");
        console.WriteLine();
        console.WriteLine($"- Total fonts: {fontNames.Count}");
        console.WriteLine($"- Loaded: {loadOk}");
        console.WriteLine($"- FullSize rendered content: {fullOk}");
        console.WriteLine($"- Fitted rendered content: {fitOk}");
        console.WriteLine($"- Smushed rendered content: {smushOk}");
        console.WriteLine();
        console.Write(table);
        console.WriteLine();
        console.Write(rendered.ToString());

        return console.Output;
    }

    private static string RenderFont(FigletFont font, FigletLayoutMode mode)
    {
        using var writer = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.No,
            ColorSystem = ColorSystemSupport.NoColors,
            Out = new AnsiConsoleOutput(writer),
            Interactive = InteractionSupport.No,
        });

        console.Profile.Width = 200;
        console.Write(new FigletText(font, SampleText)
        {
            LayoutMode = mode,
        });

        return writer.ToString();
    }
}