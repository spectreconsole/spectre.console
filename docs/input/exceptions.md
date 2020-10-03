Title: Exceptions
Order: 3
---

Exceptions isn't always readable when viewed in the terminal.  
You can make exception a bit more readable by using the `WriteException` method.

```csharp
AnsiConsole.WriteException(ex);
```

<img src="assets/images/exception.png" style="max-width: 100%; margin-bottom: 20px">



You can also shorten specific parts of the exception to make it even
more readable, and make paths clickable hyperlinks. Whether or not
the hyperlinks are clickable is up to the terminal. 

```csharp
AnsiConsole.WriteException(ex, 
    ExceptionFormat.ShortenPaths | ExceptionFormat.ShortenTypes |
    ExceptionFormat.ShortenMethods | ExceptionFormat.ShowLinks);
```

<img src="assets/images/compact_exception.png" style="max-width: 100%;">
