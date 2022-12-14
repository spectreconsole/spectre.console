Title: Columns
Description: "Use **Columns** to render widgets in vertical columns to the console."
Highlights:
    - Custom colors
    - Labels
    - Use your own data with a converter.
Reference: T:Spectre.Console.Columns

---

Use `Columns` to render widgets in vertical columns to the console.

<?# AsciiCast cast="columns" /?>

## Usage

### Basic usage

```csharp
// Render two items on separate columns to Console
AnsiConsole.Write(new Columns(
            new Text("Item 1"),
            new Text("Item 2")
        ));
```

### Add items from an IEnumerable

```csharp
// Create a list of Items
var columns = new List<Text>(){
        new Text("Item 1"),
        new Text("Item 2"),
        new Text("Item 3")
    };

// Render each item in list on separate line
AnsiConsole.Write(new Columns(columns));
```

### Apply custom styles to each column

```csharp
// Create a list of Items, apply separate styles to each
var columns = new List<Text>(){
    new Text("Item 1", new Style(Color.Red, Color.Black)),
    new Text("Item 2", new Style(Color.Green, Color.Black)),
    new Text("Item 3", new Style(Color.Blue, Color.Black))
};

// Renders each item with own style
AnsiConsole.Write(new Columns(columns));
```

