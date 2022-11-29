Title: Panel
Order: 0
RedirectFrom: panels
Description: "The **Panel** widget can be used to organize text into a rendered box."
Reference: T:Spectre.Console.Panel

---

The `Panel` widget can be used to organize text into a rendered box.

<?# AsciiCast cast="panel" /?>

## Usage

To render a panel, create a `Panel` instance, passing a string to its constructor to assign the contents.

```csharp
var panel = new Panel("Hello World");
```

## Appearance

# Headers

```csharp
// Sets the header
panel.Header = new PanelHeader("Some text");
```

# Borders

For a list of borders, see the [Borders](xref:borders) appendix section.

```csharp
// Sets the border
panel.Border = BoxBorder.Ascii;
panel.Border = BoxBorder.Square;
panel.Border = BoxBorder.Rounded;
panel.Border = BoxBorder.Heavy;
panel.Border = BoxBorder.Double;
panel.Border = BoxBorder.None;
```

# Padding

```csharp
// Sets the padding
panel.Padding = new Padding(2, 2, 2, 2);
```

# Expand

Enabling the Expand property will cause the Panel to be as wide as the console. 
Otherwise, the Panel width will be automatically calculated based on its content.
Note that this auto-calculation is not based on the Panel Header, so a Header that
is long in length may get truncated with certain content.

```csharp
// Sets the expand property
panel.Expand = true;
```
