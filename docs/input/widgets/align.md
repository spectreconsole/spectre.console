Title: Align
Description: "Use **Align** to render and position widgets in the console."
Highlights:
    - Custom colors
    - Labels
    - Use your own data with a converter.
Reference: T:Spectre.Console.Align

---

Use `Align` to render and position widgets in the console.

<?# AsciiCast cast="align" /?>

## Usage

### Basic usage

```csharp
// Render an item and align it in the bottom-left corner of the console
AnsiConsole.Write(new Align(
            new Text("Spectre!"),
            HorizontalAlignment.Left,
            VerticalAlignment.Bottom
        ));
```

### Align items from an IEnumerable

```csharp
// Create a list of items
var alignItems = new List<Text>(){
        new Text("Spectre"),
        new Text("Console"),
        new Text("Is Awesome!")
    };

// Render the items in the middle-right of the console
AnsiConsole.Write(new Align(
            alignItems,
            HorizontalAlignment.Right,
            VerticalAlignment.Middle
        ));
```

### Dynamically align with different widgets

```csharp
// Create a table 
var table = new Table()
            .AddColumn("ID")
            .AddColumn("Methods")
            .AddColumn("Purpose")
            .AddRow("1", "Center()", "Initializes a new instance that is center aligned")
            .AddRow("2", "Measure()", "Measures the renderable object")
            .AddRow("3", "Right()", "Initializes a new instance that is right aligned.");

// Create a panel
var panel = new Panel(table)
            .Header("Other Align Methods")
            .Border(BoxBorder.Double);

// Renders the panel in the top-center of the console
AnsiConsole.Write(new Align(panel, HorizontalAlignment.Center, VerticalAlignment.Top));
```

