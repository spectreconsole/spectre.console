using DocExampleGenerator;
using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class SelectionSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (base.ConsoleSize.Cols, 14);

        public override void Run(IAnsiConsole console)
        {
            console.DisplayThenType(AskFruit, "↓↓↓¦¦¦¦ ");
        }

        private static void AskFruit(IAnsiConsole console)
        {
            // Ask for the user's favorite fruit
            var fruit = console.Prompt(
                new SelectionPrompt<string>()
                    .Title("What's your [green]favorite fruit[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(new [] {"Apple", "Apricot", "Avocado", "Banana", "Blackcurrant", "Blueberry", "Cherry", "Cloudberry", "Cocunut"}));

            // Echo the fruit back to the terminal
            console.WriteLine($"I agree. {fruit} is tasty!");
        }
    }
}