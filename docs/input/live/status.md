Title: Status
Order: 10
RedirectFrom: status
Description: "*Spectre.Console* can display information about long running tasks in the console with the Status control."
Highlights: 
    - Custom spinner control for running tasks.
    - Fallback for non-interactive consoles such as CI runners.
Reference:
    - T:Spectre.Console.Status
    - M:Spectre.Console.AnsiConsole.Status
---

Spectre.Console can display information about long running tasks in the console. 

<?# AsciiCast cast="status" /?>

<?# Alert ?>
  The status display is not 
  thread safe, and using it together with other interactive components such as 
  prompts, progress displays or other status displays are not supported.
<?#/ Alert ?>

If the current terminal isn't considered "interactive", such as when running 
in a continuous integration system, or the terminal can't display 
ANSI control sequence, any progress will be displayed in a simpler way.

## Usage

```csharp
// Synchronous
AnsiConsole.Status()
    .Start("Thinking...", ctx => 
    {
        // Simulate some work
        AnsiConsole.MarkupLine("Doing some work...");
        Thread.Sleep(1000);
        
        // Update the status and spinner
        ctx.Status("Thinking some more");
        ctx.Spinner(Spinner.Known.Star);
        ctx.SpinnerStyle(Style.Parse("green"));

        // Simulate some work
        AnsiConsole.MarkupLine("Doing some more work...");
        Thread.Sleep(2000);
    });
```

## Asynchronous status

If you prefer to use async/await, you can use `StartAsync` instead of `Start`.

```csharp
// Asynchronous
await AnsiConsole.Status()
    .StartAsync("Thinking...", async ctx => 
    {
        // Omitted
    });
```

## Configure

```csharp
AnsiConsole.Status()
    .AutoRefresh(false)
    .Spinner(Spinner.Known.Star)
    .SpinnerStyle(Style.Parse("green bold"))
    .Start("Thinking...", ctx => 
    {
        // Omitted
        ctx.Refresh();
    });
```
