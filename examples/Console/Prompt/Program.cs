namespace Spectre.Console.Examples
{
    public static class Program
    {
        private class City
        {
            public string Name { get; set; }
            public string Country { get; set; }
            public float Area { get; set; }
            public int Population { get; set; }
        }

        public static void Main(string[] args)
        {
            // Check if we can accept key strokes
            if (!AnsiConsole.Profile.Capabilities.Interactive)
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

            // Ask the user for some different things
            var name = AskName();
            var fruit = AskFruit();
            var city = AskCity();
            var sport = AskSport();
            var age = AskAge();
            var password = AskPassword();
            var color = AskColor();

            // Summary
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Results[/]").RuleStyle("grey").LeftAligned());
            AnsiConsole.Write(new Table().AddColumns("[grey]Question[/]", "[grey]Answer[/]")
                .RoundedBorder()
                .BorderColor(Color.Grey)
                .AddRow("[grey]Name[/]", name)
                .AddRow("[grey]Favorite fruit[/]", fruit)
                .AddRow("[grey]Favorite city[/]", city.Name)
                .AddRow("[grey]Favorite sport[/]", sport)
                .AddRow("[grey]Age[/]", age.ToString())
                .AddRow("[grey]Password[/]", password)
                .AddRow("[grey]Favorite color[/]", string.IsNullOrEmpty(color) ? "Unknown" : color));
        }

        private static string AskName()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Strings[/]").RuleStyle("grey").LeftAligned());
            var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
            return name;
        }

        private static string AskFruit()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Lists[/]").RuleStyle("grey").LeftAligned());

            var favorites = AnsiConsole.Prompt(
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

            var fruit = favorites.Count == 1 ? favorites[0] : null;
            if (string.IsNullOrWhiteSpace(fruit))
            {
                fruit = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Ok, but if you could only choose [green]one[/]?")
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(favorites));
            }

            AnsiConsole.MarkupLine("Your selected: [yellow]{0}[/]", fruit);
            return fruit;
        }

        private static City AskCity()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Table[/]").RuleStyle("grey").LeftAligned());

            var city = AnsiConsole.Prompt(
                new TablePrompt<City>()
                    .Title("What is your [green]favorite city[/]?")
                    .AddColumns("Name", "Country", "Area", "Population")
                    .UseConverter((item, index) =>
                    {
                        return index switch
                        {
                            0 => item.Name,
                            1 => item.Country,
                            2 => $"{item.Area} km2",
                            3 => item.Population.ToString("N0"),
                            _ => string.Empty,
                        };
                    })
                    .AddChoices(
                        new City { Name = "Paris", Country = "France", Area = 105.4F, Population = 2175601 },
                        new City { Name = "London", Country = "England", Area = 1572F, Population = 8961989 },
                        new City { Name = "Los Angeles", Country = "United States", Area = 1299.01F, Population = 3898747 },
                        new City { Name = "Shanghai", Country = "China", Area = 6341F, Population = 24870895 }
                    ));

            AnsiConsole.MarkupLine("Your selected: [yellow]{0}[/]", city.Name);
            return city;
        }

        private static string AskSport()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Choices[/]").RuleStyle("grey").LeftAligned());

            return AnsiConsole.Prompt(
                new TextPrompt<string>("What's your [green]favorite sport[/]?")
                    .InvalidChoiceMessage("[red]That's not a sport![/]")
                    .DefaultValue("Sport?")
                    .AddChoice("Soccer")
                    .AddChoice("Hockey")
                    .AddChoice("Basketball"));
        }

        private static int AskAge()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Integers[/]").RuleStyle("grey").LeftAligned());

            return AnsiConsole.Prompt(
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

        private static string AskPassword()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Secrets[/]").RuleStyle("grey").LeftAligned());

            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]password[/]?")
                    .PromptStyle("red")
                    .Secret());
        }

        private static string AskColor()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Optional[/]").RuleStyle("grey").LeftAligned());

            return AnsiConsole.Prompt(
                new TextPrompt<string>("[grey][[Optional]][/] What is your [green]favorite color[/]?")
                    .AllowEmpty());
        }
    }
}
