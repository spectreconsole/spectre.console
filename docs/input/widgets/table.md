Title: Table
Order: 3
RedirectFrom: tables
---

Tables are a perfect way of displaying tabular data in a terminal.

`Spectre.Console` is super smart about rendering tables and will adjust
all columns to fit whatever is inside them. Anything that implements 
`IRenderable` can be used as a column header or column cell, even another table!

# Usage

<!------------------------->
<!--- USAGE             --->
<!------------------------->

To render a table, create a `Table` instance, add the number of
columns that you need, and then add the rows. Finish by passing the
table to a console's `Render` method.

```csharp
// Create a table
var table = new Table();

// Add some columns
table.AddColumn("Foo");
table.AddColumn(new TableColumn("Bar").Centered());

// Add some rows
table.AddRow("Baz", "[green]Qux[/]");
table.AddRow(new Markup("[blue]Corgi[/]"), new Panel("Waldo"));

// Render the table to the console
AnsiConsole.Render(table);
```

This will render the following output:

![Table](../assets/images/table.png)

# Table appearance

<!------------------------->
<!--- TABLE APPEARANCE  --->
<!------------------------->

## Borders

For a list of borders, see the [Borders](xref:borders) appendix section.

```csharp
// Sets the border
table.SetBorder(Border.None);
table.SetBorder(Border.Ascii);
table.SetBorder(Border.Square);
table.SetBorder(Border.Rounded);
```

## Expand / Collapse

```csharp
// Table will take up as much space as it can
// with respect to other things.
table.Expand();

// Table will take up minimal width
table.Collapse();
```

## Hide headers

```csharp
// Hides all column headers
table.HideHeaders();
```

## Set table width

```csharp
// Sets the table width to 50 cells
table.SetWidth(50);
```

# Column appearance

<!------------------------->
<!--- COLUMN APPEARANCE --->
<!------------------------->

## Alignment

```csharp
// Set the alignment explicitly
column.SetAlignment(Justify.Right);
```

## Padding

```csharp
// Set left and right padding
column.SetPadding(left: 3, right: 5);

// Set padding individually.
column.PadLeft(3);
column.PadRight(5);
```

## Disable column wrapping

```csharp
// Disable column wrapping
column.NoWrap();
```

## Set column width

```csharp
// Set the column width (no fluent extension method for this yet)
column.Width = 15;
```