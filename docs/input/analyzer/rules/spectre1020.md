---
Title: Spectre1020
Description: Avoid calling other live renderables while a current renderable is running.
Category: Usage
Severity: Warning
---

## Cause

A violation of this rule occurs when a child LiveRenderable i.e. `Progress`, `Status` and `Live` are called within the context of an executing renderable. Concurrent LiveRenderable are not supported and will cause issues when running simultaneously.

## Reason for rule

When LiveRenderable such as `Progress`, `Status` or `Live` are running they expect to be running exclusively. They rely on ANSI sequences to draw and keep the console experience consistent. With simultaneous calls both renderables compete with the console causing concurrent writes corrupting the output.

## How to fix violations

Redesign logic to allow one LiveRenderable to complete before starting a second renderable. 

## Examples

### Violates

```csharp
AnsiConsole.Progress().Start(ctx => {
    AnsiConsole.Status().Start("Running status too...", statusCtx => {});
});
```

### Does not violate

```csharp
AnsiConsole.Progress().Start(ctx => {
    // run progress and complete tasks
});

AnsiConsole.Status().Start("Running status afterwards...", statusCtx => {});
```

## How to suppress violations

```csharp
#pragma warning disable Spectre1020 // <Rule name>

#pragma warning restore Spectre1020 // <Rule name>
```
