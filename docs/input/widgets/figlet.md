Title: Figlet
Order: 50
RedirectFrom: figlet
Description: "*Spectre.Console* can render FIGlet text by using the **FigletText** class."
XmlDocsType: T:Spectre.Console.FigletText
---

Spectre.Console can render [FIGlet](http://www.figlet.org/) text by using the `FigletText` class.

## Default font

```csharp
AnsiConsole.Write(
    new FigletText("Hello")
        .LeftAligned()
        .Color(Color.Red));
```

<?# AsciiCast cast="figlet" /?>


## Custom font

```csharp
var font = FigletFont.Load("starwars.flf");

AnsiConsole.Write(
    new FigletText(font, "Hello")
        .LeftAligned()
        .Color(Color.Red));
```
