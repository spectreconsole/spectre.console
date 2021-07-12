Title: Colors
Order: 0
Description: "*Spectre.Console* allows easy rendering of the standard ANSI colors of your terminal, and also supports rendering up to 24-bit colors depending on the capabilities of your terminal."
Highlights:
    - Predefined common colors
    - Easy syntax for inline styling
---

The following is a list of the standard 8-bit colors supported in terminals.

Note that the first 16 colors are generally defined by the system or your terminal software, and may not display exactly as rendered here.

## Usage

You can either use the colors in code, such as `new Style(foreground: Color.Maroon)` or
in markup text such as `AnsiConsole.Markup("[maroon on blue]Hello[/]")`.

## Standard colors

<input
    id="colorSearch"
    type="search"
    oninput="search(this, 'color-results', 2)"
    placeholder="Search for colors.."
    title="Type in a color" />

<?# ColorTable /?>
