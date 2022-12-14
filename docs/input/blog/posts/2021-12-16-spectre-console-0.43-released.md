Title: Spectre.Console 0.43 released!
Description: Now with .NET 6 support... and more!
Published: 2021-12-16
Category: Release Notes
Excluded: false
---

We forgot (😅) to publish the release notes for `0.42`, so we've included them as well.
Noteworthy in this release is that `Spectre.Console` now ships with [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) support.

## New Contributors
* [@mgnslndh](https://github.com/mgnslndh) made their first contribution in [#584](https://github.com/spectreconsole/spectre.console/pull/584)
* [@LiamSho](https://github.com/LiamSho) made their first contribution in [#509](https://github.com/spectreconsole/spectre.console/pull/509)
* [@kzu](https://github.com/kzu) made their first contribution in [#514](https://github.com/spectreconsole/spectre.console/pull/514)
* [@GitHubPang](https://github.com/GitHubPang) made their first contribution in [#535](https://github.com/spectreconsole/spectre.console/pull/535)
* [@RedEyedDog](https://github.com/RedEyedDog) made their first contribution in [#546](https://github.com/spectreconsole/spectre.console/pull/546)
* [@rifatx](https://github.com/rifatx) made their first contribution in [#545](https://github.com/spectreconsole/spectre.console/pull/545)

## What's Changed

* Update sponsors.md by [@mgnslndh](https://github.com/mgnslndh) in [#584](https://github.com/spectreconsole/spectre.console/pull/584)
* Upgrades Statiq and re-enables docs publishing by [@phil-scott-78](https://github.com/phil-scott-78) in [#591](https://github.com/spectreconsole/spectre.console/pull/591)
* Internalizes the `WcWidth` package by [@patriksvensson](https://github.com/patriksvensson) in [#593](https://github.com/spectreconsole/spectre.console/pull/593)
* Small typos in the `Getting Started` page by [@antoniovalentini](https://github.com/antoniovalentini) in [#597](https://github.com/spectreconsole/spectre.console/pull/597)
* Add net6.0 TFM by [@patriksvensson](https://github.com/patriksvensson) in [#603](https://github.com/spectreconsole/spectre.console/pull/603)
* Fix a typo in code comment by [@GitHubPang](https://github.com/GitHubPang) in [#579](https://github.com/spectreconsole/spectre.console/pull/579)
* Added `GetParent` and `GetParents` to `MultiSelectionPrompt` by [@nils-a](https://github.com/nils-a) in [#590](https://github.com/spectreconsole/spectre.console/pull/590)
* Added ExceptionHandler to `ICommandAppSettings` by [@nils-a](https://github.com/nils-a) in [#607](https://github.com/spectreconsole/spectre.console/pull/607)
* Fixed documentation for selection and multiselection (#499) by [@nils-a](https://github.com/nils-a) in [#589](https://github.com/spectreconsole/spectre.console/pull/589)
* Escape any Markup when displaying selected prompt items by [@nils-a](https://github.com/nils-a) in [#610](https://github.com/spectreconsole/spectre.console/pull/610)
* Upgrade .NET SDK to 6.0.100 by [@patriksvensson](https://github.com/patriksvensson) in [#616](https://github.com/spectreconsole/spectre.console/pull/616)
* Allow color numbers in markup expressions by [@patriksvensson](https://github.com/patriksvensson) in [#615](https://github.com/spectreconsole/spectre.console/pull/615)
* Remove `Render` from docs in favor of `Write` by [@patriksvensson](https://github.com/patriksvensson) in [#613](https://github.com/spectreconsole/spectre.console/pull/613)
* Clarify `ITypeResolver` returns `null` by [@nils-a](https://github.com/nils-a) in [#620](https://github.com/spectreconsole/spectre.console/pull/620)
* Fix typo in `PairDeconstructor` class name by [@0xced](https://github.com/0xced) in [#619](https://github.com/spectreconsole/spectre.console/pull/619)
* Fix type conversion in the default pair deconstructor implementation by [@0xced](https://github.com/0xced) in [#618](https://github.com/spectreconsole/spectre.console/pull/618)
* Update examples to net6.0 by [@patriksvensson](https://github.com/patriksvensson) in [#625](https://github.com/spectreconsole/spectre.console/pull/625)
* Fix exception formatting for generic methods by [@0xced](https://github.com/0xced) in [#639](https://github.com/spectreconsole/spectre.console/pull/639)
* Use browser context for social cards by [@phil-scott-78](https://github.com/phil-scott-78) in [#490](https://github.com/spectreconsole/spectre.console/pull/490)
* Adds additional check that analyzer is within a method by [@phil-scott-78](https://github.com/phil-scott-78) in [#488](https://github.com/spectreconsole/spectre.console/pull/488)
* Add support for manipulating individual table rows by [@patriksvensson](https://github.com/patriksvensson) in [#503](https://github.com/spectreconsole/spectre.console/pull/503)
* Do not share semaphore between consoles by [@patriksvensson](https://github.com/patriksvensson) in [#507](https://github.com/spectreconsole/spectre.console/pull/507)
* Fix `ArgumentOutOfRangeExceptio` when rendering a table by [@LiamSho](https://github.com/LiamSho) in [#507](https://github.com/spectreconsole/spectre.console/pull/509)
* Make building more flexible by allowing feature bands by [@kzu](https://github.com/kzu) in [#514](https://github.com/spectreconsole/spectre.console/pull/514)
* Fix parsing of exceptions on .NET Framework by [@0xced](https://github.com/0xced) in [#513](https://github.com/spectreconsole/spectre.console/pull/513)
* Fix the style parameter nullable annotation on `AnsiConsoleExtensions` by [@0xced](https://github.com/0xced) in [#527](https://github.com/spectreconsole/spectre.console/pull/527)
* Fix 404 in documentation by [@nils-a](https://github.com/nils-a) in [#530](https://github.com/spectreconsole/spectre.console/pull/530)
* Fix typos in code comments by [@GitHubPang](https://github.com/GitHubPang) in [#535](https://github.com/spectreconsole/spectre.console/pull/535)
* Remove additional registration of `ICommand` by [@nils-a](https://github.com/nils-a) in [#533](https://github.com/spectreconsole/spectre.console/pull/533)
* Add extension methods to get cell width of `char` and `string` by [@patriksvensson](https://github.com/patriksvensson) in [#523](https://github.com/spectreconsole/spectre.console/pull/523)
* Disable GH Action workflow for docs by [@patriksvensson](https://github.com/patriksvensson) in [#547](https://github.com/spectreconsole/spectre.console/pull/547)
* Fix typos in docs by [@GitHubPang](https://github.com/GitHubPang) in [#542](https://github.com/spectreconsole/spectre.console/pull/542)
* Add a segment builder for merging multiple segments by [@phil-scott-78](https://github.com/phil-scott-78) in [#552](https://github.com/spectreconsole/spectre.console/pull/552)
* Allow user to update table cell by [@RedEyedDog](https://github.com/RedEyedDog) in [#546](https://github.com/spectreconsole/spectre.console/pull/546)
* Fix a minor typo for `Spectre1021` by [@GitHubPang](https://github.com/GitHubPang) in [#557](https://github.com/spectreconsole/spectre.console/pull/557)
* Future-proof conditional compilation by [@0xced](https://github.com/0xced) in [#563](https://github.com/spectreconsole/spectre.console/pull/563)
* Add support custom max value for barcharts by [@rifatx](https://github.com/rifatx) in [#545](https://github.com/spectreconsole/spectre.console/pull/545)
* Improve the error message when acquiring the interactive semaphore fails by [@0xced](https://github.com/0xced) in [#569](https://github.com/spectreconsole/spectre.console/pull/569)
* Add `AnsiConsole.Write` method by [@patriksvensson](https://github.com/patriksvensson) in [#577](https://github.com/spectreconsole/spectre.console/pull/577)