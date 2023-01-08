Title: Layout
Order: 45
Description: "Use **Layout** to layout widgets in the terminal."
Reference: T:Spectre.Console.Layout

---

Use `Layout` to layout widgets in the terminal.

<?# AsciiCast cast="layout" /?>

## Usage

```csharp
// Create the layout
var layout = new Layout("Root")
    .SplitColumns(
        new Layout("Left"),
        new Layout("Right")
            .SplitRows(
                new Layout("Top"),
                new Layout("Bottom")));

// Update the left column
layout["Left"].Update(
    new Panel(
        Align.Center(
            new Markup("Hello [blue]World![/]"),
            VerticalAlignment.Middle))
        .Expand());

// Render the layout
AnsiConsole.Write(layout);
```

## Setting minimum size

```csharp
layout["Left"].MinimumSize(10);
```

## Setting ratio

```csharp
layout["Left"].Ratio(2);
```

## Settings explicit size

```csharp
layout["Left"].Size(32);
```

## Hide layout

```csharp
layout["Left"].Invisible();
```

## Show layout

```csharp
layout["Left"].Visible();
```