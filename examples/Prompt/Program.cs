using Spectre.Console;

namespace Cursor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Check if we can accept key strokes
            if (!AnsiConsole.Capabilities.SupportsInteraction)
            {
                AnsiConsole.MarkupLine("[red]Environment does not support interaction.[/]");
                return;
            }

            // Confirmation
            if (!AnsiConsole.Confirm("Run prompt example?"))
            {
                AnsiConsole.MarkupLine("Ok... :(");
                return;
            }

            // String
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Rule("[yellow]Strings[/]").RuleStyle("grey").LeftAligned());
            var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");

            // String with choices
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Rule("[yellow]Choices[/]").RuleStyle("grey").LeftAligned());
            var fruit = AnsiConsole.Prompt(
                new TextPrompt<string>("What's your [green]favorite fruit[/]?")
                    .InvalidChoiceMessage("[red]That's not a valid fruit[/]")
                    .DefaultValue("Orange")
                    .AddChoice("Apple")
                    .AddChoice("Banana")
                    .AddChoice("Orange"));

            // Integer
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Rule("[yellow]Integers[/]").RuleStyle("grey").LeftAligned());
            var age = AnsiConsole.Prompt(
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

            // Secret
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Rule("[yellow]Secrets[/]").RuleStyle("grey").LeftAligned());
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]password[/]?")
                    .PromptStyle("red")
                    .Secret());

            // Optional
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Rule("[yellow]Optional[/]").RuleStyle("grey").LeftAligned());
            var color = AnsiConsole.Prompt(
                new TextPrompt<string>("[grey][[Optional]][/] What is your [green]favorite color[/]?")
                    .AllowEmpty());

            // Summary
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Rule("[yellow]Results[/]").RuleStyle("grey").LeftAligned());
            AnsiConsole.Render(new Table().AddColumns("[grey]Question[/]", "[grey]Answer[/]")
                .RoundedBorder()
                .BorderColor(Color.Grey)
                .AddRow("[grey]Name[/]", name)
                .AddRow("[grey]Favorite fruit[/]", fruit)
                .AddRow("[grey]Age[/]", age.ToString())
                .AddRow("[grey]Password[/]", password)
                .AddRow("[grey]Favorite color[/]", string.IsNullOrEmpty(color) ? "Unknown" : color));
        }
    }
}
