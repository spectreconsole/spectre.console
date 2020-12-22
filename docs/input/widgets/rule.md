Title: Rule
Order: 2
RedirectFrom: rule
---

The `Rule` class is used to render a horizontal rule (line) to the terminal.

<img src="../assets/images/rule.png" style="width: 100%;" />

# Usage

To render a rule without a title:

```csharp
var rule = new Rule();
AnsiConsole.Render(rule);
```

## Title

You can set the rule title markup text.

```csharp
var rule = new Rule("[red]Hello[/]");
AnsiConsole.Render(rule);
```

```text
───────────────────────────────── Hello ─────────────────────────────────
```

### Title alignment

You can set the rule's title alignment.

```csharp
var rule = new Rule("[red]Hello[/]");
rule.Alignment = Justify.Left;
AnsiConsole.Render(rule);
```

```text
── Hello ────────────────────────────────────────────────────────────────
```

You can also specify it via an extension method:

```csharp
var rule = new Rule("[red]Hello[/]");
rule.LeftAligned();
AnsiConsole.Render(rule);
```

```text
── Hello ────────────────────────────────────────────────────────────────
```


## Styling

```csharp
var rule = new Rule("[red]Hello[/]");
rule.Style = Style.Parse("red dim");
AnsiConsole.Render(rule);
```
You can also specify it via an extension method

```csharp
var rule = new Rule("[red]Hello[/]");
rule.RuleStyle("red dim");
AnsiConsole.Render(rule);
```
