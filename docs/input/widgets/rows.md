Title: Rows
Order: 20
Description: "Use **Rows** to render widgets in horiztonal rows to the console."
Highlights:
    - Custom colors
    - Labels
    - Use your own data with a converter.
Reference: T:Spectre.Console.Rows

---

Use `Rows` to render widgets in horizontal rows to the console.

<?# AsciiCast cast="rows" /?>

## Usage

### Basic usage

```csharp
// Render two items on separate rows to Console
AnsiConsole.Write(new Rows(
            new Text("Item 1"),
            new Text("Item 2")
        ));
```

### Add items from an IEnumerable

```csharp
// Create a list of Items
var rows = new List<Text>(){
        new Text("Item 1"),
        new Text("Item 2"),
        new Text("Item 3")
    };

// Render each item in list on separate line
AnsiConsole.Write(new Rows(rows));
```

### Apply custom styles to each row

```csharp
// Create a list of Items, apply separate styles to each
var rows = new List<Text>(){
    new Text("Item 1", new Style(Color.Red, Color.Black)),
    new Text("Item 2", new Style(Color.Green, Color.Black)),
    new Text("Item 3", new Style(Color.Blue, Color.Black))
};

// Renders each item with own style
AnsiConsole.Write(new Rows(rows));
```
