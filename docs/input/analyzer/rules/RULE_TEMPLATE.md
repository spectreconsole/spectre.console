---
Title: SpectreXxxx
Description: Rule title
Category: Usage
Severity: Hidden, Info, Warning, or Error
Excluded: true
---

## Cause

A concise-as-possible description of when this rule is violated. If there's a lot to explain, begin with "A violation of this rule occurs when..."

## Reason for rule

Explain why the user should care about the violation.

## How to fix violations

To fix a violation of this rule, [describe how to fix a violation].

## Examples

### Violates

Example(s) of code that violates the rule.

### Does not violate

Example(s) of code that does not violate the rule.

## How to suppress violations

**If the severity of your analyzer isn't _Warning_, delete this section.**

```csharp
#pragma warning disable Spectre1000 // <Rule name>
#pragma warning restore Spectre1000 // <Rule name>
```