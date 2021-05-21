Title: Figlet
Order: 50
RedirectFrom: figlet
---

Spectre.Console can render [FIGlet](http://www.figlet.org/) text by using the `FigletText` class.

## Default font

```csharp
AnsiConsole.Render(
    new FigletText("Hello")
        .LeftAligned()
        .Color(Color.Red));
```

<?# AsciiCast cast="figlet" /?>


## Custom font

```csharp
var font = FigletFont.Load("starwars.flf");

AnsiConsole.Render(
    new FigletText(font, "Hello")
        .LeftAligned()
        .Color(Color.Red));
```
