Title: Multi Selection
Order: 3
Description: "The **MultiSelectionPrompt** can be used when you want the user to select one or many items from a provided list."
Highlights:
    - Display multiple items for a user to scroll and choose from.
    - Custom page sizes.
    - Provide groups of selectable items.
---

The `MultiSelectionPrompt` can be used when you want the user to select
one or many items from a provided list.

<?# AsciiCast cast="multi-selection" /?>

<?# Alert ?> The use of prompts inside status or progress displays is not supported.
<?#/ Alert ?>

## Usage

```csharp
// Ask for the user's favorite fruits
var fruits = AnsiConsole.Prompt(
    new MultiSelectionPrompt<string>()
        .Title("What are your [green]favorite fruits[/]?")
        .NotRequired() // Not required to have a favorite fruit
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
        .InstructionsText(
            "[grey](Press [blue]<space>[/] to toggle a fruit, " + 
            "[green]<enter>[/] to accept)[/]")
        .AddChoices(new[] {
            "Apple", "Apricot", "Avocado",
            "Banana", "Blackcurrant", "Blueberry",
            "Cherry", "Cloudberry", "Coconut",
        }));

// Write the selected fruits to the terminal
foreach (string fruit in fruits)
{
    AnsiConsole.WriteLine(fruit);
}
```
