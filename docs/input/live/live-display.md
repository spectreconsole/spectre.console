Title: Live Display
Order: 0
Description: "*Spectre.Console* can update arbitrary widgets in-place."
Highlights:
    - Update tables or graphs with new updates.
    - Create a custom progress bar that extends the existing control.
Reference: 
    - T:Spectre.Console.LiveDisplay
    - M:Spectre.Console.AnsiConsole.Live(Spectre.Console.Rendering.IRenderable)
---

Spectre.Console can update arbitrary widgets in-place using the [Live Display](xref:T:Spectre.Console.LiveDisplay) widget.

<?# AsciiCast cast="live" /?>

<?# Alert ?>
  The live display is not 
  thread safe, and using it together with other interactive components such as 
  prompts, status displays or other progress displays are not supported.
<?#/ Alert ?>

```csharp
var table = new Table().Centered();

AnsiConsole.Live(table)
    .Start(ctx => 
    {
        table.AddColumn("Foo");
        ctx.Refresh();
        Thread.Sleep(1000);

        table.AddColumn("Bar");
        ctx.Refresh();
        Thread.Sleep(1000);
    });
```

## Asynchronous progress

If you prefer to use async/await, you can use `StartAsync` instead of `Start`.

```csharp
var table = new Table().Centered();

await AnsiConsole.Live(table)
    .StartAsync(async ctx => 
    {
        table.AddColumn("Foo");
        ctx.Refresh();
        await Task.Delay(1000);

        table.AddColumn("Bar");
        ctx.Refresh();
        await Task.Delay(1000);
    });
```

## Configure

```csharp
var table = new Table().Centered();

AnsiConsole.Live(table)
    .AutoClear(false)   // Do not remove when done
    .Overflow(VerticalOverflow.Ellipsis) // Show ellipsis when overflowing
    .Cropping(VerticalOverflowCropping.Top) // Crop overflow at top
    .Start(ctx =>
    {
        // Omitted
    });
```