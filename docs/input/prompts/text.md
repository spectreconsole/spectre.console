Title: Text prompt
Order: 0
RedirectFrom: prompt
---

Sometimes you want to get some input from the user, and for this
you can use the `Prompt<TResult>`.

<?# Alert ?>
  The use of prompts
  insides status or progress displays is not supported.
<?#/ Alert ?>

## Confirmation

```csharp
if (!AnsiConsole.Confirm("Run example?"))
{
    return;
}
```

```text
Run example? [y/n] (y): _
```

## Simple

```csharp
// Ask for the user's name
string name = AnsiConsole.Ask<string>("What's your [green]name[/]?");

// Ask for the user's age
int age = AnsiConsole.Ask<int>("What's your [green]age[/]?");
```

```text
What's your name? Patrik
What's your age? 37
```

## Choices

```csharp
var fruit = AnsiConsole.Prompt(
    new TextPrompt<string>("What's your [green]favorite fruit[/]?")
        .InvalidChoiceMessage("[red]That's not a valid fruit[/]")
        .DefaultValue("Orange")
        .AddChoice("Apple")
        .AddChoice("Banana")
        .AddChoice("Orange"));
```

```text
What's your favorite fruit? [Apple/Banana/Orange] (Orange): _
```

## Validation

```csharp
var age = AnsiConsole.Prompt(
    new TextPrompt<int>("What's the secret number?")
        .Validate(age =>
        {
            return age switch
            {
                < 99 => ValidationResult.Error("[red]Too low[/]"),
                > 99 => ValidationResult.Error("[red]Too high[/]"),
                _ => ValidationResult.Success(),
            };
        }));
```

```text
What's the secret number? 32
Too low
What's the secret number? 102
Too high
What's the secret number? _
```

## Secrets

```csharp
var password = AnsiConsole.Prompt(
    new TextPrompt<string>("Enter [green]password[/]")
        .PromptStyle("red")
        .Secret());
```

```text
Enter password: ************_
```

## Optional

```csharp
var color = AnsiConsole.Prompt(
    new TextPrompt<string>("[grey][[Optional]][/] [green]Favorite color[/]?")
        .AllowEmpty());
```

```text
[Optional] Favorite color? _
```