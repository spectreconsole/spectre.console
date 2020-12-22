Title: Figlet
Order: 4
RedirectFrom: figlet
---

Spectre.Console can render [FIGlet](http://www.figlet.org/) text by using the `FigletText` class.

# Default font

```csharp
AnsiConsole.Render(
    new FigletText("Hello")
        .LeftAligned()
        .Color(Color.Red));
```

```text
 _   _          _   _          
| | | |   ___  | | | |   ___  
| |_| |  / _ \ | | | |  / _ \ 
|  _  | |  __/ | | | | | (_) |
|_| |_|  \___| |_| |_|  \___/ 
```

# Custom font

```csharp
var font = FigletFont.Load("starwars.flf");

AnsiConsole.Render(
    new FigletText(font, "Hello")
        .LeftAligned()
        .Color(Color.Red));
```