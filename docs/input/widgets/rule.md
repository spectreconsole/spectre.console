Title: Rule
Order: 30
RedirectFrom: rule
Description: "The **Rule** class is used to render a horizontal rule (line) to the terminal."
XmlDocsType: T:Spectre.Console.Rule
Highlights:
    - Custom colors for line and title.
    - Specify left, center or right aligned title.

---

The `Rule` class is used to render a horizontal rule (line) to the terminal.

<?# AsciiCast cast="rule" /?>

## Usage

To render a rule without a title:

```csharp
var rule = new Rule();
AnsiConsole.Write(rule);
```

## Title

You can set the rule title markup text.

```csharp
var rule = new Rule("[red]Hello[/]");
AnsiConsole.Write(rule);
```

```text
───────────────────────────────── Hello ─────────────────────────────────
```

## Title alignment

You can set the rule's title alignment.

```csharp
var rule = new Rule("[red]Hello[/]");
rule.Alignment = Justify.Left;
AnsiConsole.Write(rule);
```

```text
── Hello ────────────────────────────────────────────────────────────────
```

You can also specify it via an extension method:

```csharp
var rule = new Rule("[red]Hello[/]");
rule.LeftAligned();
AnsiConsole.Write(rule);
```

```text
── Hello ────────────────────────────────────────────────────────────────
```


## Styling

```csharp
var rule = new Rule("[red]Hello[/]");
rule.Style = Style.Parse("red dim");
AnsiConsole.Write(rule);
```
You can also specify it via an extension method

```csharp
var rule = new Rule("[red]Hello[/]");
rule.RuleStyle("red dim");
AnsiConsole.Write(rule);
```
