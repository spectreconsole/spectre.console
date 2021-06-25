---
Title: Spectre1000
Description: Use AnsiConsole instead of System.Console
Category: Usage
Severity: Warning
---

## Cause

A violation of this rule occurs when `System.Console` is used for common methods exposed by Spectre.Console.

## Reason for rule

Methods implemented in Spectre.Console should be used over direct access to `System.Console` to allow for enhancements and
features to be enabled.

## How to fix violations

To fix a violation of this rule, change from `System.Console` to `Spectre.Console.AnsiConsole`.

## How to suppress violations

```csharp
#pragma warning disable Spectre1000 // Use AnsiConsole instead of System.Console

#pragma warning restore Spectre1000 // Use AnsiConsole instead of System.Console
```
