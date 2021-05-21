using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class RuleSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (82, 10);

        public override void Run(IAnsiConsole console)
        {
            console.Write(new Rule());
            console.WriteLine();
            console.Write(new Rule("[blue]Left aligned[/]").LeftAligned().RuleStyle("red"));
            console.WriteLine();
            console.Write(new Rule("[green]Centered[/]").Centered().RuleStyle("green"));
            console.WriteLine();
            console.Write(new Rule("[red]Right aligned[/]").RightAligned().RuleStyle("blue"));
            console.WriteLine();
        }
    }
}