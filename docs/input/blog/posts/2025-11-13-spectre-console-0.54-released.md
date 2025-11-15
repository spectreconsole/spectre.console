Title: Spectre.Console 0.54.0 released!
Description: Spectre.Console.Cli has a new home!
Published: 2025-11-13
Category: Release Notes
Excluded: false
---

Version `0.54.0` of Spectre.Console has been released!

## Spectre.Console.Cli has a new home!

We've decided to move `Spectre.Console.Cli` to its own repository, where we will prepare it for a _1.0_ release. This means that the _Spectre.Console.Cli_ NuGet packages will no longer be versioned together with _Spectre.Console_. They will now have a preview version such as `1.0.0-alpha-0.x`.

There should be no issues staying on version _0.53.0_ of _Spectre.Console.Cli_ until we release a stable version if you prefer not to use a pre-release dependency.

## New unit testing package for Spectre.Console.Cli

There is now a new testing package for _Spectre.Console.Cli_ called [Spectre.Console.Cli.Testing](https://www.nuget.org/packages/Spectre.Console.Cli.Testing). This is where you will find the `CommandAppTester` from now on.

You can find more information about unit testing in the [documentation](https://spectreconsole.net/cli/unit-testing).

## What's Changed

* Normalizes paths when writing exceptions to the console for tests. by [@phil-scott-78](https://github.com/phil-scott-78) in [#1758](https://github.com/spectreconsole/spectre.console/pull/1758)
* Fixes issue with Panel not applying overflow to children by [@phil-scott-78](https://github.com/phil-scott-78) in [#1942](https://github.com/spectreconsole/spectre.console/pull/1942)
* Remove Spectre.Console.Cli from repository by [@patriksvensson](https://github.com/patriksvensson) in [#1928](https://github.com/spectreconsole/spectre.console/pull/1928)

**Full Changelog**: https://github.com/spectreconsole/spectre.console/compare/0.53.0...0.54.0