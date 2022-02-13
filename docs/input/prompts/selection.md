Title: Selection
Order: 1
Description: "The **SelectionPrompt** can be used when you want the user to select a single item from a provided list."
Reference: 
    - T:Spectre.Console.SelectionPrompt`1
    - M:Spectre.Console.AnsiConsole.Prompt``1(Spectre.Console.IPrompt{``0})
---

The `SelectionPrompt` can be used when you want the user to select
a single item from a provided list.

<?# AsciiCast cast="selection" /?>

<?# Alert ?>
 Using prompts inside
  status or progress displays, are not supported.
<?#/ Alert ?>

## Usage

```csharp
// Ask for the user's favorite fruit
var fruit = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("What's your [green]favorite fruit[/]?")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
        .AddChoices(new[] {
            "Apple", "Apricot", "Avocado", 
            "Banana", "Blackcurrant", "Blueberry",
            "Cherry", "Cloudberry", "Cocunut",
        }));

// Echo the fruit back to the terminal
AnsiConsole.WriteLine($"I agree. {fruit} is tasty!");
```