Title: Markup
Order: 30
Description: The Markup class allows you to output rich text to the console.
Highlights:
 - Easily add *color*.
 - Add hyperlinks to for supported terminals.
 - Emoji 🚀 parsing.
---

The `Markup` class allows you to output rich text to the console.

## Syntax

Console markup uses a syntax inspired by bbcode. If you write the style (see [Styles](xref:styles)) 
in square brackets, e.g. `[bold red]`, that style will apply until it is closed with a `[/]`.

```csharp
AnsiConsole.Render(new Markup("[bold yellow]Hello[/] [red]World![/]"));
```

The `Markup` class implements `IRenderable` which means that you 
can use this in tables, grids, and panels. Most classes that support
rendering of `IRenderable` also have overloads for rendering rich text.

```csharp
var table = new Table();
table.AddColumn(new TableColumn(new Markup("[yellow]Foo[/]")));
table.AddColumn(new TableColumn("[blue]Bar[/]"));
AnsiConsole.Render(table);
```

## Convenience methods

There is also convenience methods on `AnsiConsole` that can be used
to write markup text to the console without instantiating a new `Markup`
instance.

```csharp
AnsiConsole.Markup("[underline green]Hello[/] ");
AnsiConsole.MarkupLine("[bold]World[/]");
```

## Escaping format characters

To output a `[` you use `[[`, and to output a `]` you use `]]`.

```csharp
AnsiConsole.Markup("[[Hello]] "); // [Hello]
AnsiConsole.Markup("[red][[World]][/]"); // [World]
```

You can also use the `EscapeMarkup` extension method.

```csharp
AnsiConsole.Markup("[red]{0}[/]", "Hello [World]".EscapeMarkup());
```
You can also use the `Markup.Escape` method.

```csharp
AnsiConsole.Markup("[red]{0}[/]", Markup.Escape("Hello [World]"));
```
## Setting background color

You can set the background color in markup by prefixing the color with
`on`.

```csharp
AnsiConsole.Markup("[bold yellow on blue]Hello[/]");
AnsiConsole.Markup("[default on blue]World[/]");
```

## Rendering emojis

To output an emoji as part of markup, you can use emoji shortcodes.

```csharp
AnsiConsole.Markup("Hello :globe_showing_europe_africa:!");
```

For a list of emoji, see the [Emojis](xref:emojis) appendix section.

## Colors

In the examples above, all colors was referenced by their name,
but you can also use the hex or rgb representation for colors in markdown.

```csharp
AnsiConsole.Markup("[red]Foo[/] ");
AnsiConsole.Markup("[#ff0000]Bar[/] ");
AnsiConsole.Markup("[rgb(255,0,0)]Baz[/] ");
```

For a list of colors, see the [Colors](xref:colors) appendix section.

## Styles

For a list of styles, see the [Styles](xref:styles) appendix section.