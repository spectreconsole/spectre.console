Title: Spectre.Console 0.46 released!
Description: .NET 7 support, Layout Widget, JSON rendering
Published: 2023-01-10
Category: Release Notes
Excluded: false
---

Happy new year! 🎉  
Version 0.46 of Spectre.Console has been released!

A lot has happened since the last release, but the most notable additions 
and changes are support for [.NET 7](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7/), 
the new [Layout](https://spectreconsole.net/widgets/layout) widget, and 
[rendering of JSON](https://spectreconsole.net/widgets/json). There has also been a lot of long overdue work
on the command line argument parser.

## New Contributors
* [@GaryMcD](https://github.com/GaryMcD) made their first contribution in [#961](https://github.com/spectreconsole/spectre.console/pull/961)
* [@eduherminio](https://github.com/eduherminio) made their first contribution in [#964](https://github.com/spectreconsole/spectre.console/pull/964)
* [@Saalvage](https://github.com/Saalvage) made their first contribution in [#976](https://github.com/spectreconsole/spectre.console/pull/976)
* [@BenjaminMichaelis](https://github.com/BenjaminMichaelis) made their first contribution in [#1000](https://github.com/spectreconsole/spectre.console/pull/1000)
* [@nilaoda](https://github.com/nilaoda) made their first contribution in [#1012](https://github.com/spectreconsole/spectre.console/pull/1012)
* [@picture](https://github.com/picture)-vision made their first contribution in [#1013](https://github.com/spectreconsole/spectre.console/pull/1013)
* [@patrickfreilinger](https://github.com/patrickfreilinger) made their first contribution in [#1016](https://github.com/spectreconsole/spectre.console/pull/1016)
* [@sowa](https://github.com/sowa)705 made their first contribution in [#1014](https://github.com/spectreconsole/spectre.console/pull/1014)
* [@ardalis](https://github.com/ardalis) made their first contribution in [#1021](https://github.com/spectreconsole/spectre.console/pull/1021)
* [@Elisha](https://github.com/Elisha)-Aguilera made their first contribution in [#1038](https://github.com/spectreconsole/spectre.console/pull/1038)
* [@wguner](https://github.com/wguner) made their first contribution in [#1044](https://github.com/spectreconsole/spectre.console/pull/1044)
* [@bcwood](https://github.com/bcwood) made their first contribution in [#1068](https://github.com/spectreconsole/spectre.console/pull/1068)
* [@FrankRay](https://github.com/FrankRay)78 made their first contribution in [#1073](https://github.com/spectreconsole/spectre.console/pull/1073)
* [@tomkerkhove](https://github.com/tomkerkhove) made their first contribution in [#1089](https://github.com/spectreconsole/spectre.console/pull/1089)
* [@ArveSystad](https://github.com/ArveSystad) made their first contribution in [#1090](https://github.com/spectreconsole/spectre.console/pull/1090)
* [@maije](https://github.com/maije) made their first contribution in [#1096](https://github.com/spectreconsole/spectre.console/pull/1096)
* [@krisrok](https://github.com/krisrok) made their first contribution in [#953](https://github.com/spectreconsole/spectre.console/pull/953)

## What's changed?
* Add support for .NET 7.0 by [@patriksvensson](https://github.com/patriksvensson) in [#1056](https://github.com/spectreconsole/spectre.console/pull/1056)
* Add `Layout` widget by [@patriksvensson](https://github.com/patriksvensson) in [#1041](https://github.com/spectreconsole/spectre.console/pull/1041)
* Add JSON text renderer by [@patriksvensson](https://github.com/patriksvensson) in [#1086](https://github.com/spectreconsole/spectre.console/pull/1086)
* Backward direction of text prompt autocomplete by [@nkochnev](https://github.com/nkochnev) in [#921](https://github.com/spectreconsole/spectre.console/pull/921)
* Custom mask for secret by [@GaryMcD](https://github.com/GaryMcD) in [#970](https://github.com/spectreconsole/spectre.console/pull/970)
* Allow selections to wrap around by [@Saalvage](https://github.com/Saalvage) in [#976](https://github.com/spectreconsole/spectre.console/pull/976)
* Join .NET Foundation by [@patriksvensson](https://github.com/patriksvensson) in [#978](https://github.com/spectreconsole/spectre.console/pull/978)
* Adding value: a single semi-colon! by [@johanlindfors](https://github.com/johanlindfors) in [#986](https://github.com/spectreconsole/spectre.console/pull/986)
* Fix `@` being used in Figlet font by [@Saalvage](https://github.com/Saalvage) in [#972](https://github.com/spectreconsole/spectre.console/pull/972)
* Add new and transferred issues to backlog project by [@patriksvensson](https://github.com/patriksvensson) in [#995](https://github.com/spectreconsole/spectre.console/pull/995)
* Pin SDK due to a bug in .NET 6.0.401 by [@patriksvensson](https://github.com/patriksvensson) in [#1011](https://github.com/spectreconsole/spectre.console/pull/1011)
* Remove period trimming by [@BenjaminMichaelis](https://github.com/BenjaminMichaelis) in [#1008](https://github.com/spectreconsole/spectre.console/pull/1008)
* Allow `PACKET` key on MultiSelectionPrompt by [@nilaoda](https://github.com/nilaoda) in [#1012](https://github.com/spectreconsole/spectre.console/pull/1012)
* Added Suckless Simple Terminal to list of ANSI terminals by [@picture](https://github.com/picture)-vision in [#1013](https://github.com/spectreconsole/spectre.console/pull/1013)
* Add culture option to `TypeConverterHelper`, `TextPrompt` and `AnsiConsole` by [@sowa](https://github.com/sowa)705 in [#1014](https://github.com/spectreconsole/spectre.console/pull/1014)
* Minor typo fixes by [@ardalis](https://github.com/ardalis) in [#1021](https://github.com/spectreconsole/spectre.console/pull/1021)
* Alignment fixes by [@patriksvensson](https://github.com/patriksvensson) in [#1066](https://github.com/spectreconsole/spectre.console/pull/1066)
* `IndexOf` replaced by Count at Add method - Performance issue #975 fixed by [@maije](https://github.com/maije) in [#1096](https://github.com/spectreconsole/spectre.console/pull/1096)
* Modified tokenizer not to break on on `]]]` at the end of a style by [@nils](https://github.com/nils)-a in [#1027](https://github.com/spectreconsole/spectre.console/pull/1027)
* Command line argument parsing improvements by [@FrankRay](https://github.com/FrankRay)78 in [#1048](https://github.com/spectreconsole/spectre.console/pull/1048)
* Show help for default command by [@krisrok](https://github.com/krisrok) in [#953](https://github.com/spectreconsole/spectre.console/pull/953)
* Automatically display default values of options in the help page by @0xced in [#1032](https://github.com/spectreconsole/spectre.console/pull/1032)

## Documentation updates
* Add link to documentation in README by [@ardalis](https://github.com/ardalis) in [#1030](https://github.com/spectreconsole/spectre.console/pull/1030)
* Update `.NET 5` references in docs by [@eduherminio](https://github.com/eduherminio) in [#964](https://github.com/spectreconsole/spectre.console/pull/964)
* Blog date fix by [@phil](https://github.com/phil)-scott-78 in [#963](https://github.com/spectreconsole/spectre.console/pull/963)
* Update sponsors by [@tomkerkhove](https://github.com/tomkerkhove) in [#1089](https://github.com/spectreconsole/spectre.console/pull/1089)
* Inline `CommandArgument` required/optional style in template parameter docs by [@ArveSystad](https://github.com/ArveSystad) in [#1090](https://github.com/spectreconsole/spectre.console/pull/1090)
* Add documentation for `BreakdownChart` by [@BenjaminMichaelis](https://github.com/BenjaminMichaelis) in [#1000](https://github.com/spectreconsole/spectre.console/pull/1000)
* Create `Panel` documentation by [@patrickfreilinger](https://github.com/patrickfreilinger) in [#1016](https://github.com/spectreconsole/spectre.console/pull/1016)
* Added details for using links within markup. by [@GaryMcD](https://github.com/GaryMcD) in [#961](https://github.com/spectreconsole/spectre.console/pull/961)
* Added documentation for `Rows` widget by [@Elisha](https://github.com/Elisha)-Aguilera in [#1038](https://github.com/spectreconsole/spectre.console/pull/1038)
* Added documentation guide for `Grid` widget  by [@Elisha](https://github.com/Elisha)-Aguilera in [#1043](https://github.com/spectreconsole/spectre.console/pull/1043)
* Added documentation guide for the `Padder` widget by [@Elisha](https://github.com/Elisha)-Aguilera in [#1046](https://github.com/spectreconsole/spectre.console/pull/1046)
* Created a `Columns` widget documentation by [@wguner](https://github.com/wguner) in [#1044](https://github.com/spectreconsole/spectre.console/pull/1044)
* Fixed typo in `Panel` documentation [@bcwood](https://github.com/bcwood) in [#1068](https://github.com/spectreconsole/spectre.console/pull/1068)
* Clarified the license for `SixLabors.ImageSharp` by [@FrankRay](https://github.com/FrankRay)78 in [#1073](https://github.com/spectreconsole/spectre.console/pull/1073)
* Add documentation for `Layout` by [@patriksvensson](https://github.com/patriksvensson) in [#1127](https://github.com/spectreconsole/spectre.console/pull/1127)

## Dependencies
* Update dependency `Wcwidth.Sources` to `v1` by [@renovate](https://github.com/renovate) in [#969](https://github.com/spectreconsole/spectre.console/pull/969)
* Update `actions/setup-dotnet` action to `v3` by [@renovate](https://github.com/renovate) in [#982](https://github.com/spectreconsole/spectre.console/pull/982)
* Update dependency `Microsoft.NET.Test.Sdk` to `v17.3.2` by [@renovate](https://github.com/renovate) in [#977](https://github.com/spectreconsole/spectre.console/pull/977)
* Update dependency `cake.tool` to `v2.3.0` by [@renovate](https://github.com/renovate) in [#1015](https://github.com/spectreconsole/spectre.console/pull/1015)