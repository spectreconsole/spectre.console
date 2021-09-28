---
Title: Spectre1021
Description: Avoid prompting for input while a current renderable is running.
Category: Usage
Severity: Warning
---

## Cause

A violation of this rule occurs when an AnsiConsole prompt is called within the context of an executing renderable e.g. `Progress`, `Status` and `Live`. Concurrent LiveRenderable are not supported and will cause issues when running simultaneously.

## Reason for rule

When LiveRenderable such as `Progress`, `Status` or `Live` are running they expect to be running exclusively. They rely on ANSI sequences to draw and keep the console experience consistent. Prompts also rely on ANSI sequences for their drawing. Simultaneous running can result in corrupt output.

## How to fix violations

Redesign logic to allow one LiveRenderable to complete before using a prompt or prompt before starting the operation. 

## Examples

### Violates

```csharp
AnsiConsole.Progress().Start(ctx =>
{
    // code to update progress bar
    var answer = AnsiConsole.Confirm("Continue?");
});
```

### Does not violate

```csharp
AnsiConsole.Progress().Start(ctx =>
{
    // code to update progress bar

    // persist state to restart progress after asking question   
});

var answer = AnsiConsole.Confirm("Continue?");

AnsiConsole.Progress().Start(ctx =>
{
    // apply persisted state
    // code to update progress bar
```

## How to suppress violations

```csharp
#pragma warning disable Spectre1021 // <Rule name>

#pragma warning restore Spectre1021 // <Rule name>
```
