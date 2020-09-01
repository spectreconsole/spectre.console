Title: Markup
Order: 3
Hidden: False
---

In `Spectre.Console` there's a class called `Markup` that
allows you to output rich text to the console.

```csharp
AnsiConsole.Render(new Markup("[bold yellow]Hello[/] [red]World![/]"));
```

Which should output something similar to the image below. Note that the
actual appearance might vary depending on your terminal.

![](/spectre.console/assets/images/helloworld.png)


The `Markup` class implements `IRenderable` which means that you 
can use this in tables, grids, and panels. Most classes that support
rendering of `IRenderable` also have overloads for rendering rich text.

```csharp
var table = new Table();
table.AddColumn(new TableColumn(new Markup("[yellow]Foo[/]")));
table.AddColumn(new TableColumn("[blue]Bar[/]"));
```

# Convenience methods

There is also convenience methods on `AnsiConsole` that can be used
to write markup text to the console without instantiating a new `Markup`
instance.

```csharp
AnsiConsole.Markup("[underline green]Hello[/] ");
AnsiConsole.MarkupLine("[bold]World[/]");
```

# Escaping format characters

To output a `[` you use `[[`, and to output a `]` you use `]]`.

```csharp
AnsiConsole.Markup("[[Hello]] "); // [Hello]
AnsiConsole.Markup("[red][[World]][/]"); // [World]
```

# Setting background color

You can set the background color in markup by prefixing the color with
`on`.

```
[bold yellow on blue]Hello[/]
[default on blue]World[/]
```

# Colors

For a list of colors, see the [Colors](xref:colors) section.

# Styles

Note that what styles that can be used is defined by the system or your terminal software, and may not appear as they should.

<table class="table">
    <tr>
        <td><code>bold</code></td>
        <td>Bold text</td>
    </tr>
    <tr>
        <td><code>dim</code></td>
        <td>Dim or faint text</td>
    </tr>
    <tr>
        <td><code>italic</code></td>
        <td>Italic text</td>
    </tr>
    <tr>
        <td><code>underline</code></td>
        <td>Underlined text</td>
    </tr>
    <tr>
        <td><code>invert</code></td>
        <td>Swaps the foreground and background colors</td>
    </tr>
    <tr>
        <td><code>conceal</code></td>
        <td>Hides the text</td>
    </tr>
    <tr>
        <td><code>slowblink</code></td>
        <td>Makes text blink slowly</td>
    </tr>
    <tr>
        <td><code>rapidblink</code></td>
        <td>Makes text blink</td>
    </tr>
    <tr>
        <td><code>strikethrough</code></td>
        <td>Shows text with a horizontal line through the center</td>
    </tr>
</table>