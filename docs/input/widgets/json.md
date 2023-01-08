Title: JSON
Order: 70
Description: "Use *ImageSharp* to parse images and render them as Ascii art to the console."
Reference: T:Spectre.Console.Json.JsonText
---

To add JSON superpowers to 
your console application to render JSON text, you will need to install 
the [Spectre.Console.Json](https://www.nuget.org/packages/Spectre.Console.Json) NuGet package.

```text
> dotnet add package Spectre.Console.Json
```

## Rendering JSON

Once you've added the `Spectre.Console.Json` NuGet package, 
you can start rendering JSON to the console.

```csharp
using Spectre.Console.Json;

var json = new JsonText(
    """
    { 
        "hello": 32, 
        "world": { 
            "foo": 21, 
            "bar": 255,
            "baz": [
                0.32, 0.33e-32,
                0.42e32, 0.55e+32,
                {
                    "hello": "world",
                    "lol": null
                }
            ]
        } 
    }
    """);

AnsiConsole.Write(
    new Panel(json)
        .Header("Some JSON in a panel")
        .Collapse()
        .RoundedBorder()
        .BorderColor(Color.Yellow));
```

### Result

<?# AsciiCast cast="json" /?>

## Styling

All the different JSON parts can be customized to have unique styles.

```csharp
AnsiConsole.Write(
    new JsonText(json)
        .BracesColor(Color.Red)
        .BracketColor(Color.Green)
        .ColonColor(Color.Blue)
        .CommaColor(Color.Red)
        .StringColor(Color.Green)
        .NumberColor(Color.Blue)
        .BooleanColor(Color.Red)
        .NullColor(Color.Green));
```