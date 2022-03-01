// Write a markup line to the console
MarkupLine("[yellow]Hello[/], [blue]World[/]!");

// Write text to the console
WriteLine("Hello, World!");

// Write a table to the console
Write(new Table()
    .RoundedBorder()
    .AddColumns("[red]Greeting[/]", "[red]Subject[/]")
    .AddRow("[yellow]Hello[/]", "World")
    .AddRow("[green]Oh hi[/]", "[blue u]Mark[/]"));