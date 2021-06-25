---
Title: Spectre1010
Description: Favor the use of the instance of AnsiConsole over the static helper
Category: Usage
Severity: Info
---

## Cause

A violation of this rule occurs when the static helper `AnsiConsole` is used when a field or method parameter of type
`IAnsiConsole` is available.

## Reason for rule

Use of `IAnsiConsole` improves testability of the code, and also allows upstream callers the ability to customize the console
capabilities and features. When a field variable or parameter is available it should be used to ensure the code takes advantage
of that configuration.

## How to fix violations

To fix a violation of this rule, change from `AnsiConsole` to the name of the local instance.

## Examples

### Violates

```csharp
class Example
{
    private IAnsiConsole _ansiConsole;

    public Example(IAnsiConsole ansiConsole) 
    {
        _ansiConsole = ansiConsole;
    }

    public Run()
    {
        AnsiConsole.WriteLine("Running...");
    }

}
```

### Does not violate

```csharp
class Example
{
    private IAnsiConsole _ansiConsole;

    public Example(IAnsiConsole ansiConsole) 
    {
        _ansiConsole = ansiConsole;
    }

    public Run()
    {
        _ansiConsole.WriteLine("Running...");
    }

}
```
