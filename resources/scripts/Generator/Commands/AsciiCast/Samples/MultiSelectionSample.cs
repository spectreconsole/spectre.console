using DocExampleGenerator;
using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class MultiSelectionSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (base.ConsoleSize.Cols, 14);

        public override void Run(IAnsiConsole console)
        {
            console.DisplayThenType(AskFruit, "↓↓ ¦¦↑↑ ¦¦ ¦¦↓ ↓↓↓↓↓ ↓↓↓↓ ¦¦↲");
        }

        private static void AskFruit(IAnsiConsole console)
        {
            var favorites = console.Prompt(
                new MultiSelectionPrompt<string>()
                    .PageSize(10)
                    .Title("What are your [green]favorite fruits[/]?")
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .InstructionsText("[grey](Press [blue]<space>[/] to toggle a fruit, [green]<enter>[/] to accept)[/]")
                    .AddChoiceGroup("Berries", new[]
                    {
                        "Blackcurrant", "Blueberry", "Cloudberry",
                        "Elderberry", "Honeyberry", "Mulberry"
                    })
                    .AddChoices(new[]
                    {
                        "Apple", "Apricot", "Avocado", "Banana",
                        "Cherry", "Cocunut", "Date", "Dragonfruit", "Durian",
                        "Egg plant",  "Fig", "Grape", "Guava",
                        "Jackfruit", "Jambul", "Kiwano", "Kiwifruit", "Lime", "Lylo",
                        "Lychee", "Melon", "Nectarine", "Orange", "Olive"
                    }));

            console.MarkupLine("Your selected: [yellow]{0}[/]", string.Join(',', favorites));
        }
    }
}