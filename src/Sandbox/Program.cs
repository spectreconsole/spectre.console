using Spectre.Console;

RenderFigletWithPanel(FigletLayoutMode.FullSize);
RenderFigletWithPanel(FigletLayoutMode.Fitted);
RenderFigletWithPanel(FigletLayoutMode.Smushed);

return;

static void RenderFigletWithPanel(FigletLayoutMode mode)
{
    AnsiConsole.Write(
        new Panel(
                new FigletText("Hello from Spectre.Console").LayoutMode(mode))
            .Header(mode switch
            {
                FigletLayoutMode.FullSize => "FullSize [i](default)[/]",
                FigletLayoutMode.Fitted => "Fitted",
                FigletLayoutMode.Smushed => "Smushed",
                _ => throw new InvalidOperationException(),
            })
            .BorderColor(Color.Yellow));
}