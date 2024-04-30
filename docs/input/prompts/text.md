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

## Simple

<?# Example symbol="M:Prompt.Program.AskName" project="Prompt" /?>

```text
What's your name? Patrik
What's your age? 37
```

## Choices

<?# Example symbol="M:Prompt.Program.AskFruit" project="Prompt" /?>

```text
What's your favorite fruit? [Apple/Banana/Orange] (Orange): _
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

## Secrets

<?# Example symbol="M:Prompt.Program.AskPassword" project="Prompt" /?>


```text
Enter password: ************_
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

## Optional

<?# Example symbol="M:Prompt.Program.AskColor" project="Prompt" /?>

```text
[Optional] Favorite color? _
```