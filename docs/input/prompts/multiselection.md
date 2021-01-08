Title: Multi Selection
Order: 3
---

The `MultiSelectionPrompt` can be used when you want the user to select
one or many items from a provided list.

<img src="../assets/images/multiselection.gif" style="width: 100%;" />

# Usage

```csharp
// Ask for the user's favorite fruits
var fruits = AnsiConsole.Prompt(
    new MultiSelectionPrompt<string>()
        .Title("What are your [green]favorite fruits[/]?")
        .NotRequired() // Not required to have a favorite fruit
        .PageSize(10)
        .AddChoice("Apple")
        .AddChoices(new[] {
            "Apricot", "Avocado", 
            "Banana", "Blackcurrant", "Blueberry",
            "Cherry", "Cloudberry", "Cocunut",
        }));

// Write the selected fruits to the terminal
foreach (string fruit in fruits) 
{
    AnsiConsole.WriteLine(fruit);
}
```