using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class PanelSample : BaseSample
    {
        public override void Run(IAnsiConsole console)
        {
            var panel = new Panel("[red]Spaghetti\nLinguini\nFettucine\nTortellini\nCapellini\nLasagna[/]");
            panel.Header = new PanelHeader("[underline]Pasta Menu[/]", Justify.Center);
            panel.Border = BoxBorder.Double;
            panel.Padding = new Padding(2, 2, 2, 2);
            console.Write(panel);
        }
    }
}