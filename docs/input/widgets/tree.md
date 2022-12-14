Title: Tree
Order: 10
Description: "The **Tree** widget can be used to render hierarchical data."
Highlights:
    - Custom colors and styles for guidelines.
    - Include any *Spectre.Console* widgets as child nodes.
Reference: T:Spectre.Console.Tree

---

The `Tree` widget can be used to render hierarchical data.

<?# AsciiCast cast="tree" /?>

## Usage

```csharp
// Create the tree
var root = new Tree("Root");

// Add some nodes
var foo = root.AddNode("[yellow]Foo[/]");
var table = foo.AddNode(new Table()
    .RoundedBorder()
    .AddColumn("First")
    .AddColumn("Second")
    .AddRow("1", "2")
    .AddRow("3", "4")
    .AddRow("5", "6"));

table.AddNode("[blue]Baz[/]");
foo.AddNode("Qux");

var bar = root.AddNode("[yellow]Bar[/]");
bar.AddNode(new Calendar(2020, 12)
    .AddCalendarEvent(2020, 12, 12)
    .HideHeader());

// Render the tree
AnsiConsole.Write(root);
```

## Collapsing nodes

```csharp
root.AddNode("Label").Collapse();
```

## Appearance

### Style

```csharp
var root = new Tree("Root")
    .Style("white on red");
```

### Guide lines

```csharp
// ASCII guide lines
var root = new Tree("Root")
    .Guide(TreeGuide.Ascii);

// Default guide lines
var root = new Tree("Root")
    .Guide(TreeGuide.Line);

// Double guide lines
var root = new Tree("Root")
    .Guide(TreeGuide.DoubleLine);

// Bold guide lines
var root = new Tree("Root")
    .Guide(TreeGuide.BoldLine);
```
