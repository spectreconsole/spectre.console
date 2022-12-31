using DocExampleGenerator;
using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class InputSample : BaseSample
    {
        public override void Run(IAnsiConsole console)
        {
            var age = 0;
            var name = string.Empty;
            var sport = string.Empty;
            var password = string.Empty;
            var color = string.Empty;

            console.DisplayThenType(c => name = AskName(c), "Peter F↲");
            console.DisplayThenType(c => sport = AskSport(c), "football↲¦¦¦¦Hockey↲");
            console.DisplayThenType(c => age = AskAge(c), "Forty↲¦¦¦¦40↲");
            console.DisplayThenType(c => password = AskPassword(c), "hunter2↲");
            console.DisplayThenType(c => color = AskColor(c), "↲");

            AnsiConsole.Write(new Rule("[yellow]Results[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.Write(new Table().AddColumns("[grey]Question[/]", "[grey]Answer[/]")
                .RoundedBorder()
                .BorderColor(Color.Grey)
                .AddRow("[grey]Name[/]", name)
                .AddRow("[grey]Favorite sport[/]", sport)
                .AddRow("[grey]Age[/]", age.ToString())
                .AddRow("[grey]Password[/]", password)
                .AddRow("[grey]Favorite color[/]", string.IsNullOrEmpty(color) ? "Unknown" : color));
        }

        private static string AskName(IAnsiConsole console)
        {
            console.WriteLine();
            console.Write(new Rule("[yellow]Strings[/]").RuleStyle("grey").LeftJustified());
            var name = console.Ask<string>("What's your [green]name[/]?");
            return name;
        }


        private static string AskSport(IAnsiConsole console)
        {
            console.WriteLine();
            console.Write(new Rule("[yellow]Choices[/]").RuleStyle("grey").LeftJustified());

            return console.Prompt(
                new TextPrompt<string>("What's your [green]favorite sport[/]?")
                    .InvalidChoiceMessage("[red]That's not a sport![/]")
                    .DefaultValue("Sport?")
                    .AddChoice("Soccer")
                    .AddChoice("Hockey")
                    .AddChoice("Basketball"));
        }

        private static int AskAge(IAnsiConsole console)
        {
            console.WriteLine();
            console.Write(new Rule("[yellow]Integers[/]").RuleStyle("grey").LeftJustified());

            return console.Prompt(
                new TextPrompt<int>("How [green]old[/] are you?")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid age[/]")
                    .Validate(age =>
                    {
                        return age switch
                        {
                            <= 0 => ValidationResult.Error("[red]You must at least be 1 years old[/]"),
                            >= 123 => ValidationResult.Error("[red]You must be younger than the oldest person alive[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));
        }

        private static string AskPassword(IAnsiConsole console)
        {
            console.WriteLine();
            console.Write(new Rule("[yellow]Secrets[/]").RuleStyle("grey").LeftJustified());

            return console.Prompt(
                new TextPrompt<string>("Enter [green]password[/]?")
                    .PromptStyle("red")
                    .Secret());
        }

        private static string AskColor(IAnsiConsole console)
        {
            console.WriteLine();
            console.Write(new Rule("[yellow]Optional[/]").RuleStyle("grey").LeftJustified());

            return console.Prompt(
                new TextPrompt<string>("[grey][[Optional]][/] What is your [green]favorite color[/]?")
                    .AllowEmpty());
        }
    }
}