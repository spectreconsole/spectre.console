using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace Prompt
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Check if we can accept key strokes
            if (!AnsiConsole.Profile.Capabilities.Interactive)
            {
                AnsiConsole.MarkupLine("[red]Environment does not support interaction.[/]");
                return;
            }

            // Confirmation
            if (!AskConfirmation())
            {
                return;
            }

            // Ask the user for some different things
            WriteDivider("Strings");
            var name = AskName();

            WriteDivider("Lists");
            var fruit = AskFruit();

            WriteDivider("Prompt Tree");
            var deepSpaceObjects = AskDeepSpaceObjects();

            WriteDivider("Choices");
            var sport = AskSport();

            WriteDivider("Integers");
            var age = AskAge();

            WriteDivider("Secrets");
            var password = AskPassword();

            WriteDivider("Mask");
            var mask = AskPasswordWithCustomMask();

            WriteDivider("Null Mask");
            var nullMask = AskPasswordWithNullMask();

            WriteDivider("Optional");
            var color = AskColor();

            // Summary
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Results[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.Write(new Table().AddColumns("[grey]Question[/]", "[grey]Answer[/]")
                .RoundedBorder()
                .BorderColor(Color.Grey)
                .AddRow("[grey]Name[/]", name)
                .AddRow("[grey]Favorite fruit[/]", fruit)
                .AddRow("[grey]Favorite DSOs[/]", string.Join(", ", deepSpaceObjects))
                .AddRow("[grey]Favorite sport[/]", sport)
                .AddRow("[grey]Age[/]", age.ToString())
                .AddRow("[grey]Password[/]", password)
                .AddRow("[grey]Mask[/]", mask)
                .AddRow("[grey]Null Mask[/]", nullMask)
                .AddRow("[grey]Favorite color[/]", string.IsNullOrEmpty(color) ? "Unknown" : color));
        }

        private static void WriteDivider(string text)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule($"[yellow]{text}[/]").RuleStyle("grey").LeftJustified());
        }

        public static bool AskConfirmation()
        {
            if (!AnsiConsole.Confirm("Run prompt example?"))
            {
                AnsiConsole.MarkupLine("Ok... :(");
                return false;
            }

            return true;
        }

        public static string AskName()
        {
            var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
            return name;
        }

        public static string AskFruit()
        {
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

            AnsiConsole.MarkupLine("You selected: [yellow]{0}[/]", fruit);
            return fruit;
        }

        public static IEnumerable<string> AskDeepSpaceObjects()
        {
            var tree =
                new SelectableItems(
                    "All",
                    new SelectableItems(
                        "Galaxies",
                        new SelectableItems(
                            "Spiral",
                            new SelectableItem("Milky Way"),
                            new SelectableItem("M31 (Andromeda)")),
                        new SelectableItems(
                            "Elliptical",
                            new SelectableItems("M87 (Virgo A)"),
                            new SelectableItems("M49 (NGC 4472)"))),
                    new SelectableItems(
                        "Globular Clusters",
                        new SelectableItem("NGC 5139 (Omega Centauri)"),
                        new SelectableItem("M13 (Hercules)"),
                        new SelectableItem("M3"),
                        new SelectableItem("M5")),
                    new SelectableItems(
                        "Open Clusters",
                        new SelectableItem("M45 (The Pleiades)"),
                        new SelectableItem("M44 (The Beehive Cluster)"),
                        new SelectableItem("M6 (the Butterfly Cluster)"),
                        new SelectableItem("M7 (Ptolemy's Cluster)")),
                    new SelectableItems(
                        "Nebulas",
                        new SelectableItems(
                            "Emission",
                            new SelectableItem("M42 (Orion Nebula)"),
                            new SelectableItem("M16 (Eagle Nebula)")),
                        new SelectableItems(
                            "Planetary",
                            new SelectableItem("M57 (Ring Nebula)"),
                            new SelectableItem("M27 (Dumbbell Nebula)"))));

            return AnsiConsole.Prompt(
                    new MultiSelectionPrompt<SelectableItem>()
                        .AddChoiceTree(
                            tree,
                            selectableItem =>
                                selectableItem is SelectableItems selectableItems
                                    ? selectableItems.Children
                                    : Enumerable.Empty<SelectableItem>())
                        .PageSize(15)
                        .Title("What are your favorite DSOs?")
                        .HighlightStyle(
                            new Style(
                                Color.Black,
                                Color.SteelBlue1))
                        .UseConverter(selectable => selectable.Name))
                .Select(selectableItem => selectableItem.Name);
        }

        public static string AskSport()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("What's your [green]favorite sport[/]?")
                    .InvalidChoiceMessage("[red]That's not a sport![/]")
                    .DefaultValue("Sport?")
                    .AddChoice("Soccer")
                    .AddChoice("Hockey")
                    .AddChoice("Basketball"));
        }

        public static int AskAge()
        {
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

        public static string AskPassword()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]password[/]?")
                    .PromptStyle("red")
                    .Secret());
        }

        public static string AskPasswordWithCustomMask()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]password[/]?")
                    .PromptStyle("red")
                    .Secret('-'));
        }

        public static string AskPasswordWithNullMask()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]password[/]?")
                    .PromptStyle("red")
                    .Secret(null));
        }

        public static string AskColor()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("[grey][[Optional]][/] What is your [green]favorite color[/]?")
                    .AllowEmpty());
        }
    }
}