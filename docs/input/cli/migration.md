Title: Migrate from Spectre.Cli
Order: 10
Description: "Migrating from *Spectre.Cli* to *Spectre.Console.Cli*"
---

The functionality in `Spectre.Cli` has been moved into the `Spectre.Console`
library. If you're using `Spectre.Cli`, you will need to migrate to ensure
that you get updates or fixes.

## 1. Remove Spectre.Cli NuGet package

Start with removing the `Spectre.Cli` package reference from your project(s).

```text
> dotnet remove package Spectre.Cli
```

## 2. Add Spectre.Console NuGet package

Add the [Spectre.Console](https://www.nuget.org/packages/spectre.console) NuGet package to your project(s).

```text
> dotnet add package Spectre.Console
```

## 3. Change using statements

Change all using statements from `Spectre.Cli` 
to `Spectre.Console.Cli`.

```diff
- using Spectre.Cli;
+ using Spectre.Console.Cli;
```

## Breaking Changes

In the process of moving `Spectre.Cli`, there have been some minor breaking changes.

### Spectre.Cli.Exceptions namespace moved

All exceptions have been moved from the `Spectre.Cli.Exceptions` namespace to
  the `Spectre.Console.Cli` namespace.

```diff
- using Spectre.Cli.Exceptions;
+ using Spectre.Console.Cli;
```