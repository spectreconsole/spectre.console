Title: Text Path
Order: 80
Description: "The **TextPath** class is used to render a Windows or Unix path."
Highlights:
    - Automatically shrinks paths to fit.
    - Custom colors for segments of the path.
    - Specify left, center or right aligned paths.
Reference: T:Spectre.Console.TextPath

---

The `TextPath` class is used to render a Windows or Unix path.

<?# AsciiCast cast="text-path" /?>

## Usage

```csharp
var path = new TextPath("C:/This/Path/Is/Too/Long/To/Fit/In/The/Area.txt");

AnsiConsole.Write(path);
```

## Alignment

```csharp
var path = new TextPath("C:/This/Path/Is/Too/Long/To/Fit/In/The/Area.txt");
path.Alignment = Justify.Right;

AnsiConsole.Write(path);
```

You can also specify styles via extension methods:

```csharp
var path = new TextPath("C:/This/Path/Is/Too/Long/To/Fit/In/The/Area.txt")
    .RightJustified();
```

## Styling

All the segments in the path can be customized to have different styles.

```csharp
var path = new TextPath("C:/This/Path/Is/Too/Long/To/Fit/In/The/Area.txt");

path.RootStyle = new Style(foreground: Color.Red);
path.SeparatorStyle = new Style(foreground: Color.Green);
path.StemStyle = new Style(foreground: Color.Blue);
path.LeafStyle = new Style(foreground: Color.Yellow);
```

You can also specify styles via extension methods:

```csharp
var path = new TextPath("C:/This/Path/Is/Too/Long/To/Fit/In/The/Area.txt")
    .RootStyle(new Style(foreground: Color.Red))
    .SeparatorStyle(new Style(foreground: Color.Green))
    .StemStyle(new Style(foreground: Color.Blue))
    .LeafStyle(new Style(foreground: Color.Yellow));
```

Or just set the colors via extension methods:

```csharp
var path = new TextPath("C:/This/Path/Is/Too/Long/To/Fit/In/The/Area.txt")
    .RootColor(Color.Red)
    .SeparatorColor(Color.Green)
    .StemColor(Color.Blue)
    .LeafColor(Color.Yellow);
```
