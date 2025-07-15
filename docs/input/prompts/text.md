Title: Text prompt
Order: 0
RedirectFrom: prompt
Description: "*Spectre.Console* has multiple input functions for helping receive strongly typed input from a user."
Highlights:
    - Confirmation.
    - Strongly typed input.
    - Input restricted to specific items.
    - Secrets such as passwords or keys.
---

Sometimes you want to get some input from the user, and for this
you can use the `Prompt<TResult>`.

<?# AsciiCast cast="input" /?>

<?# Alert ?>
  The use of prompts
  insides status or progress displays is not supported.
<?#/ Alert ?>

## Confirmation

<?# Example symbol="M:Prompt.Program.AskConfirmation" project="Prompt" /?>

```text
Run prompt example? [y/n] (y): _
```

### Usage

```csharp
// Ask the user to confirm
var confirmation = AnsiConsole.Prompt(
    new TextPrompt<bool>("Run prompt example?")
        .AddChoice(true)
        .AddChoice(false)
        .DefaultValue(true)
        .WithConverter(choice => choice ? "y" : "n"));

// Echo the confirmation back to the terminal
Console.WriteLine(confirmation ? "Confirmed" : "Declined");
```

Otherwise it is possible to use the `ConfirmationPrompt`

```csharp
// Ask the user to confirm
var confirmation = AnsiConsole.Prompt(
    new ConfirmationPrompt("Run prompt example?"));

// Echo the confirmation back to the terminal
Console.WriteLine(confirmation ? "Confirmed" : "Declined");
```

## Simple

<?# Example symbol="M:Prompt.Program.AskName" project="Prompt" /?>

```text
What's your name? Patrik
What's your age? 37
```

### Usage

```csharp
// Ask the user a couple of simple questions
var name = AnsiConsole.Prompt(
    new TextPrompt<string>("What's your name?"));
var age = AnsiConsole.Prompt(
    new TextPrompt<int>("What's your age?"));

// Echo the name and age back to the terminal
AnsiConsole.WriteLine($"So you're {name} and you're {age} years old");
```

Otherwise it is possible to use the `Ask` method

```csharp
// Ask the user a couple of simple questions
var name = AnsiConsole.Ask<string>("What's your name?");
var age = AnsiConsole.Ask<int>("What's your age?");

// Echo the name and age back to the terminal
AnsiConsole.WriteLine($"So you're {name} and you're {age} years old");
```

## Choices

<?# Example symbol="M:Prompt.Program.AskFruit" project="Prompt" /?>

```text
What's your favorite fruit? [Apple/Banana/Orange] (Orange): _
```

### Usage

```csharp
// Ask for the user's favorite fruit
var fruit = AnsiConsole.Prompt(
    new TextPrompt<string>("What's your favorite fruit?")
      .AddChoices(["Apple", "Banana", "Orange"])
      .DefaultValue("Orange"));

// Echo the fruit back to the terminal
Console.WriteLine($"I agree. {fruit} is tasty!");
```

## Validation

<?# Example symbol="M:Prompt.Program.AskAge" project="Prompt" /?>

```text
What's the secret number? 32
Too low
What's the secret number? 102
Too high
What's the secret number? _
```

### Usage

```csharp
// Ask the user to guess the secret number
var number = AnsiConsole.Prompt(
    new TextPrompt<int>("What's the secret number?")
      .Validate((n) => n switch
      {
          < 50 => ValidationResult.Error("Too low"),
          50 => ValidationResult.Success(),
          > 50 => ValidationResult.Error("Too high"),
      }));

// Echo the user's success back to the terminal
Console.WriteLine($"Correct! The secret number is {number}.");
```

## Secrets

<?# Example symbol="M:Prompt.Program.AskPassword" project="Prompt" /?>


```text
Enter password: ************_
```

### Usage

```csharp
// Ask the user to enter the password
var password = AnsiConsole.Prompt(
    new TextPrompt<string>("Enter password:")
        .Secret());

// Echo the password back to the terminal
Console.WriteLine($"Your password is {password}");
```

## Masks

<?# Example symbol="M:Prompt.Program.AskPasswordWithCustomMask" project="Prompt" /?>


```text
Enter password: ------------_
```

You can utilize a null character to completely hide input.

<?# Example symbol="M:Prompt.Program.AskPasswordWithNullMask" project="Prompt" /?>

```text
Enter password: _
```

### Usage

```csharp
// Ask the user to enter the password
var password = AnsiConsole.Prompt(
    new TextPrompt<string>("Enter password:")
        .Secret('-'));

// Echo the password back to the terminal
Console.WriteLine($"Your password is {password}");
```

## Optional

<?# Example symbol="M:Prompt.Program.AskColor" project="Prompt" /?>

```text
[Optional] Favorite color? _
```

### Usage

```csharp
// Ask for the user's favorite color (optional)
var color = AnsiConsole.Prompt(
    new TextPrompt<string>("[[Optional]] Favorite color?")
        .AllowEmpty());

// Echo the color back to the terminal
Console.WriteLine(string.IsNullOrWhiteSpace(color)
    ? "You're right, all colors are beautiful"
    : $"I agree. {color} is a very beautiful color");
```