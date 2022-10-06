Title: Breakdown Chart
Order: 25
Description: "Use **BreakdownChart** to render breakdown charts to the console."
Highlights:
    - Custom colors
    - Labels
    - Use your own data with a converter.
Reference: T:Spectre.Console.BreakdownChart

---

Use `BreakdownChart` to render breakdown charts to the console.

<?# AsciiCast cast="breakdown-chart" /?>

## Usage

### Basic usage

```csharp
AnsiConsole.Write(new BreakdownChart()
    .Width(60)
    // Add item is in the order of label, value, then color.
    .AddItem("SCSS", 80, Color.Red)
    .AddItem("HTML", 28.3, Color.Blue)
    .AddItem("C#", 22.6, Color.Green)
    .AddItem("JavaScript", 6, Color.Yellow)
    .AddItem("Ruby", 6, Color.LightGreen)
    .AddItem("Shell", 0.1, Color.Aqua));
```

### Additional Styling

```csharp
// Render chart at full width of console.
AnsiConsole.Write(new BreakdownChart()
    .FullSize()
    .AddItem("SCSS", 80, Color.Red)
    .AddItem("HTML", 28.3, Color.Blue)
    .AddItem("C#", 22.6, Color.Green)
    .AddItem("JavaScript", 6, Color.Yellow)
    .AddItem("Ruby", 6, Color.LightGreen)
    .AddItem("Shell", 0.1, Color.Aqua));
```

```csharp
// Show percentage signs after the values in the chart.
AnsiConsole.Write(new BreakdownChart()
    .ShowPercentage()
    .AddItem("SCSS", 80, Color.Red)
    .AddItem("HTML", 28.3, Color.Blue)
    .AddItem("C#", 22.6, Color.Green)
    .AddItem("JavaScript", 6, Color.Yellow)
    .AddItem("Ruby", 6, Color.LightGreen)
    .AddItem("Shell", 0.1, Color.Aqua));
```

```csharp
// Hide tags displaying in the chart altogether.
AnsiConsole.Write(new BreakdownChart()
    .HideTag()
    .AddItem("SCSS", 80, Color.Red)
    .AddItem("HTML", 28.3, Color.Blue)
    .AddItem("C#", 22.6, Color.Green)
    .AddItem("JavaScript", 6, Color.Yellow)
    .AddItem("Ruby", 6, Color.LightGreen)
    .AddItem("Shell", 0.1, Color.Aqua));
```

```csharp
// Hide the values next to the tag from displaying in the chart.
AnsiConsole.Write(new BreakdownChart()
    .HideTagValues()
    .AddItem("SCSS", 80, Color.Red)
    .AddItem("HTML", 28.3, Color.Blue)
    .AddItem("C#", 22.6, Color.Green)
    .AddItem("JavaScript", 6, Color.Yellow)
    .AddItem("Ruby", 6, Color.LightGreen)
    .AddItem("Shell", 0.1, Color.Aqua));
```

### Additional Functionality

#### Add items with converter

```csharp
// Create a list of fruits with their colors
var items = new List<(string Label, double Value, Color color)>
{
    ("Apple", 12, Color.Green),
    ("Orange", 54, Color.Orange1),
    ("Banana", 33, Color.Yellow),
};

// Render the chart
AnsiConsole.Write(new BreakdownChart()
    .FullSize()
    .ShowPercentage()
    .AddItems(items, (item) => new BreakdownChartItem(
        item.Label, item.Value, item.color)));
```

#### Add items implementing IBreakdownChartItem

```csharp
// Declare Fruit that implements IBreakdownChartItem
public sealed class Fruit : IBreakdownChartItem
{
    public string Label { get; set; }
    public double Value { get; set; }
    public Color Color { get; set; }

    public Fruit(string label, double value, Color color)
    {
        Label = label;
        Value = value;
        Color = color;
    }
}

// Create a list of fruits
var items = new List<Fruit>
{
    new Fruit("Apple", 12, Color.Green),
    new Fruit("Orange", 54, Color.Orange1),
    new Fruit("Banana", 33, Color.Yellow),
}

// Render chart
AnsiConsole.Write(new BreakdownChart()
.Width(60)
.AddItem(new Fruit("Mango", 3, Color.Orange4))
.AddItems(items));
```
