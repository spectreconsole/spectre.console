Title: Async Extensions
Order: 11
Description: "Async Extensions provides extension methods for running tasks with an inline animations."
Highlights:
    - Extension methods for running tasks with spinner animations
    - Support for both void and generic Task types
    - Customizable spinner styles and console output
Reference:
    - T:Spectre.Console.Extensions.SpinnerExtensions
Xref: spinner-extensions
---

The Async Spinner Extension provides convenient extension methods for running tasks with an inline spinner animations in the console.

<?# AsciiCast cast="await-spinner" /?>

<?# Alert ?>
  The spinner animation is not thread safe, and using it together with other interactive 
  components such as prompts, progress displays or other status displays is not supported.
<?#/ Alert ?>

## Usage

The extension methods allow you to easily add spinner animations to any Task execution:

```csharp
// Basic usage with void Task
await someTask.Spinner();

// With generic Task<T>
var result = await someTaskWithResult.Spinner(
    Spinner.Known.Star,
    new Style(foreground: Color.Green));

// With custom console
await someTask.Spinner(
    Spinner.Known.Dots,
    style: Style.Plain,
    ansiConsole: customConsole);
```

## Features

The spinner extensions provide:

- Support for both void Tasks and Tasks with return values
- Customizable spinner animations using any Spectre.Console Spinner
- Optional styling for the spinner animation
- Ability to specify a custom IAnsiConsole instance

## Examples

Here's a more complete example showing different ways to use the spinner extensions:

```csharp
// Basic spinner with default settings
await Task.Delay(1000)
    .Spinner(Spinner.Known.Dots);

// Customized spinner with style
var result = await CalculateSomething()
    .Spinner(
        Spinner.Known.Star,
        new Style(foreground: Color.Green));

// Using with a custom console
await ProcessData()
    .Spinner(
        new Spinner(new[] { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" }, 80),
        new Style(foreground: Color.Blue),
        customConsole);
```

