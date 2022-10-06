Title: Bar Chart
Order: 20
Description: "Use **BarChart** to render bar charts to the console."
Highlights:
    - Custom colors
    - Labels
    - Use your own data with a converter.
Reference: T:Spectre.Console.BarChart

---

Use `BarChart` to render bar charts to the console.

<?# AsciiCast cast="bar-chart" /?>

## Usage

### Basic usage

```csharp
AnsiConsole.Write(new BarChart()
    .Width(60)
    .Label("[green bold underline]Number of fruits[/]")
    .CenterLabel()
    .AddItem("Apple", 12, Color.Yellow)
    .AddItem("Orange", 54, Color.Green)
    .AddItem("Banana", 33, Color.Red));
```

### Add items with converter

```csharp
// Create a list of fruits
var items = new List<(string Label, double Value)>
{
    ("Apple", 12),
    ("Orange", 54),
    ("Banana", 33),
};

// Render bar chart
AnsiConsole.Write(new BarChart()
    .Width(60)
    .Label("[green bold underline]Number of fruits[/]")
    .CenterLabel()
    .AddItems(items, (item) => new BarChartItem(
        item.Label, item.Value, Color.Yellow)));
```

### Add items implementing IBarChartItem

```csharp
public sealed class Fruit : IBarChartItem
{
    public string Label { get; set; }
    public double Value { get; set; }
    public Color? Color { get; set; }

    public Fruit(string label, double value, Color? color = null)
    {
        Label = label;
        Value = value;
        Color = color;
    }
}

// Create a list of fruits
var items = new List<Fruit>
{
    new Fruit("Apple", 12, Color.Yellow),
    new Fruit("Orange", 54, Color.Red),
    new Fruit("Banana", 33, Color.Green),
};

// Render bar chart
AnsiConsole.Write(new BarChart()
    .Width(60)
    .Label("[green bold underline]Number of fruits[/]")
    .CenterLabel()
    .AddItem(new Fruit("Mango", 3))
    .AddItems(items));
```
