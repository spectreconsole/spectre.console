Title: Grid
Order: 45
Description: "Use **Grid** to render items in a grid pattern."
Highlights:
    - Custom colors
    - Labels
    - Use your own data with a converter.
Reference: T:Spectre.Console.Grid

---

Use `Grid` to render items in a grid pattern.

<?# AsciiCast cast="grid" /?>

## Usage

### Basic usage

```csharp
var grid = new Grid();
        
// Add columns 
grid.AddColumn();
grid.AddColumn();
grid.AddColumn();

// Add header row 
grid.AddRow(new string[]{"Header 1", "Header 2", "Header 3"});
grid.AddRow(new string[]{"Row 1", "Row 2", "Row 3"});

// Write to Console
AnsiConsole.Write(grid);
```

### Align and style items within cells

```csharp
var grid = new Grid();
        
// Add columns 
grid.AddColumn();
grid.AddColumn();
grid.AddColumn();

// Add header row 
grid.AddRow(new Text[]{
    new Text("Header 1", new Style(Color.Red, Color.Black)).LeftAligned(),
    new Text("Header 2", new Style(Color.Green, Color.Black)).Centered(),
    new Text("Header 3", new Style(Color.Blue, Color.Black)).RightAligned()
});

// Add content row 
grid.AddRow(new Text[]{
    new Text("Row 1").LeftAligned(),
    new Text("Row 2").Centered(),
    new Text("Row 3").RightAligned()
});

// Write centered cell grid contents to Console
AnsiConsole.Write(grid);
```

### Embed a grid within a grid

```csharp
var grid = new Grid();
        
// Add columns 
grid.AddColumn();
grid.AddColumn();
grid.AddColumn();

// Add header row 
grid.AddRow(new Text[]{
    new Text("Header 1", new Style(Color.Red, Color.Black)).LeftAligned(),
    new Text("Header 2", new Style(Color.Green, Color.Black)).Centered(),
    new Text("Header 3", new Style(Color.Blue, Color.Black)).RightAligned()
});

var embedded = new Grid();

embedded.AddColumn();
embedded.AddColumn();

embedded.AddRow(new Text("Embedded I"), new Text("Embedded II"));
embedded.AddRow(new Text("Embedded III"), new Text("Embedded IV"));

// Add content row 
grid.AddRow(
    new Text("Row 1").LeftAligned(),
    new Text("Row 2").Centered(),
    embedded
);

// Write centered cell grid contents to Console
AnsiConsole.Write(grid);
```