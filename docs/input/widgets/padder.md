Title: Padder
Order: 55
Description: "Use **Padder** to add padding around a Widget."
Highlights:
    - Custom colors
    - Labels
    - Use your own data with a converter.
Reference: T:Spectre.Console.Padder

---

Use `Padder` to add padding around a Widget.

<?# AsciiCast cast="padder" /?>

## Usage

### Basic usage

```csharp
// Create three text elements
var paddedText_I = new Text("Padded Text I", new Style(Color.Red, Color.Black));
var paddedText_II = new Text("Padded Text II", new Style(Color.Green, Color.Black));
var paddedText_III = new Text("Padded Text III", new Style(Color.Blue, Color.Black));

// Apply padding to the three text elements
var pad_I = new Padder(paddedText_I).PadRight(16).PadBottom(0).PadTop(4);
var pad_II = new Padder(paddedText_II).PadBottom(0).PadTop(2);
var pad_III = new Padder(paddedText_III).PadLeft(16).PadBottom(0).PadTop(0);

// Insert padded elements within single-row grid
var grid = new Grid();

grid.AddColumn();
grid.AddColumn();
grid.AddColumn();

grid.AddRow(pad_I, pad_II, pad_III);

// Write grid and it's padded contents to the Console
AnsiConsole.Write(grid);
```

### Padding element within a padded element

```csharp
// Create two text elements
var paddedText_I = new Text("Padded Text I", new Style(Color.Red, Color.Black));
var paddedText_II = new Text("Padded Text II", new Style(Color.Blue, Color.Black));

// Create, apply padding on text elements
var pad_I = new Padder(paddedText_I).PadRight(2).PadBottom(0).PadTop(0);
var pad_II = new Padder(paddedText_II).PadLeft(2).PadBottom(0).PadTop(0);

// Insert the text elements into a single row grid
var grid = new Grid();

grid.AddColumn();
grid.AddColumn();

grid.AddRow(pad_I, pad_II);

// Apply horizontal and vertical padding on the grid
var paddedGrid = new Padder(grid).Padding(4,1);

// Write the padded grid to the Console
AnsiConsole.Write(paddedGrid);
```