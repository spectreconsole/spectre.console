Title: Spectre.Console 0.45 released!
Description: .NET 5 dropped, Spectre.Console.Cli moved to separate NuGet package
Published: 2022-09-10
Category: Release Notes
Excluded: false
---

Version 0.45 of Spectre.Console has been released!

There are some big changes with this release, which you can 
read all about below.

## Spectre.Console.Cli Moved

All CLI parsing-related functionality has been moved to its own NuGet 
package: [Spectre.Console.Cli](https://www.nuget.org/packages/spectre.console.cli).

The reasoning for this is that we want to play better with other CLI parsing libraries 
without the confusion of having two libraries for CLI handling intermixed.

We're sorry for breaking things like this, but we're sure it's the best
decision moving forward.

## .NET5 Support Dropped

This version has dropped `.NET5` support, which has reached EOL.  

## New Contributors

* [@drewnoakes](https://github.com/drewnoakes) made their first contribution in [#781](https://github.com/spectreconsole/spectre.console/pull/781)
* [@renovate](https://github.com/renovate) made their first contribution in [#785](https://github.com/spectreconsole/spectre.console/pull/785)
* [@leo](https://github.com/leo)-costa made their first contribution in [#782](https://github.com/spectreconsole/spectre.console/pull/782)
* [@wanglong126](https://github.com/wanglong126) made their first contribution in [#819](https://github.com/spectreconsole/spectre.console/pull/819)
* [@ivml](https://github.com/ivml) made their first contribution in [#834](https://github.com/spectreconsole/spectre.console/pull/834)
* [@dependabot](https://github.com/dependabot) made their first contribution in [#849](https://github.com/spectreconsole/spectre.console/pull/849)
* [@danielchalmers](https://github.com/danielchalmers) made their first contribution in [#850](https://github.com/spectreconsole/spectre.console/pull/850)
* [@nkochnev](https://github.com/nkochnev) made their first contribution in [#920](https://github.com/spectreconsole/spectre.console/pull/920)
* [@lonix1](https://github.com/lonix1) made their first contribution in [#938](https://github.com/spectreconsole/spectre.console/pull/938)

## What's Changed

* Move `Spectre.Console.Cli` to its own package by [@patriksvensson](https://github.com/patriksvensson) in [#827](https://github.com/spectreconsole/spectre.console/pull/827)
* Remove the 'net50' TFM by [@patriksvensson](https://github.com/patriksvensson) in [#877](https://github.com/spectreconsole/spectre.console/pull/877)
* Corrected section heading in `Status` by [@drewnoakes](https://github.com/drewnoakes) in [#781](https://github.com/spectreconsole/spectre.console/pull/781)
* Upgrade dotnet example tool to `1.6.0` by [@leo](https://github.com/leo)-costa in [#782](https://github.com/spectreconsole/spectre.console/pull/782)
* Fix documentation workflow by [@patriksvensson](https://github.com/patriksvensson) in [#799](https://github.com/spectreconsole/spectre.console/pull/799)
* Fix missing API reference pages for `Spectre.Console.Cli` by [@ivml](https://github.com/ivml) in [#834](https://github.com/spectreconsole/spectre.console/pull/834)
* Fix the "Escaping Interpolated Strings" documentation by [@0xced](https://github.com/0xced) in [#837](https://github.com/spectreconsole/spectre.console/pull/837)
* Check for null in `TextPrompt` by [@danielchalmers](https://github.com/danielchalmers) in [#850](https://github.com/spectreconsole/spectre.console/pull/850)
* Fix `ArgumentNullException` on .NET Framework by [@nils](https://github.com/nils)-a in [#923](https://github.com/spectreconsole/spectre.console/pull/923)
* Add command description to command help message by [@nkochnev](https://github.com/nkochnev) in [#920](https://github.com/spectreconsole/spectre.console/pull/920)
* Fix missing call to `Validate` when using `CommandConstructorBinde` by [@nils](https://github.com/nils)-a in [#924](https://github.com/spectreconsole/spectre.console/pull/924)
* Modified `MarkupTokenizer`, so that escaped markup inside markup is valid by [@nils](https://github.com/nils)-a in [#911](https://github.com/spectreconsole/spectre.console/pull/911)
* Detect non-interactive console via `Console.IsInputRedirected` instead of `Environment.UserInteractive` by [@bastianeicher](https://github.com/bastianeicher) in [#824](https://github.com/spectreconsole/spectre.console/pull/824)
* Set the `DevelopmentDependency` flag on the `Spectre.Console.Analyzer` project by [@0xced](https://github.com/0xced) in [#950](https://github.com/spectreconsole/spectre.console/pull/950)