Title: Exceptions
Order: 3
---

Exceptions isn't always readable when viewed in the terminal.  
You can make exception a bit more readable by using the `WriteException` method.

```csharp
AnsiConsole.WriteException(ex);
```

<img src="assets/images/exception.png" style="max-width: 100%;">

## Shortening parts

You can also shorten specific parts of the exception to make it even
more readable, and make paths clickable hyperlinks. Whether or not
the hyperlinks are clickable is up to the terminal. 

```csharp
AnsiConsole.WriteException(ex, 
    ExceptionFormat.ShortenPaths | ExceptionFormat.ShortenTypes |
    ExceptionFormat.ShortenMethods | ExceptionFormat.ShowLinks);
```

<img src="assets/images/compact_exception.png" style="max-width: 100%;">

## Customizing exception output

In addition to shorten specific part of the exception, you can 
also override the default styling.

```csharp
AnsiConsole.WriteException(ex, new ExceptionSettings
{
    Format = ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks,
    Style = new ExceptionStyle
    {
        Exception = Style.WithForeground(Color.Grey),
        Message = Style.WithForeground(Color.White),
        NonEmphasized = Style.WithForeground(Color.Cornsilk1),
        Parenthesis = Style.WithForeground(Color.Cornsilk1),
        Method = Style.WithForeground(Color.Red),
        ParameterName = Style.WithForeground(Color.Cornsilk1),
        ParameterType = Style.WithForeground(Color.Red),
        Path = Style.WithForeground(Color.Red),
        LineNumber = Style.WithForeground(Color.Cornsilk1),
    }
});
```

<img src="assets/images/custom_exception.png" style="max-width: 100%;">
