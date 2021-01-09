Title: Selection
Order: 1
---

The `SelectionPrompt` can be used when you want the user to select
a single item from a provided list.

<img src="../assets/images/selection.gif" style="width: 100%;" />

# Usage

```csharp
// Ask for the user's favorite fruit
var fruit = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("What's your [green]favorite fruit[/]?")
        .PageSize(10)
        .AddChoice("Apple")
        .AddChoices(new[] {
            "Apricot", "Avocado", 
            "Banana", "Blackcurrant", "Blueberry",
            "Cherry", "Cloudberry", "Cocunut",
        }));

// Echo the fruit back to the terminal
AnsiConsole.WriteLine($"I agree. {fruit} is tasty!");
```