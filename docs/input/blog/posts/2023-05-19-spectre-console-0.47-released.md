Title: Spectre.Console 0.47 released!
Description: Alacritty terminal support, command line improvements
Published: 2023-05-19
Category: Release Notes
Excluded: false
---

Version 0.47 of Spectre.Console has been released!

There are a lot of fixes and improvements in this release, the most noteworthy changes being support for the [Alacritty](https://github.com/alacritty/alacritty) terminal and continued improvements to the command line parsing.

Thank you to all contributers.

## New Contributors
* @wbaldoumas made their first contribution in https://github.com/spectreconsole/spectre.console/pull/1143
* @MartinZikmund made their first contribution in https://github.com/spectreconsole/spectre.console/pull/1151
* @ilyahryapko made their first contribution in https://github.com/spectreconsole/spectre.console/pull/1131
* @meziantou made their first contribution in https://github.com/spectreconsole/spectre.console/pull/1174
* @MaxAtoms made their first contribution in https://github.com/spectreconsole/spectre.console/pull/1211
* @phillip-haydon made their first contribution in https://github.com/spectreconsole/spectre.console/pull/1218

## What's Changed
* Add Alacritty to the supported terminals in AnsiDetector by @MaxAtoms in https://github.com/spectreconsole/spectre.console/pull/1211
* Add an implicit operator to convert from Color to Style by @0xced in https://github.com/spectreconsole/spectre.console/pull/1160
* Allow case-insensitive confirmation prompt by @MartinZikmund in https://github.com/spectreconsole/spectre.console/pull/1151
* Allow configuration of confirmation prompt comparison via `StringComparer` by @MartinZikmund in https://github.com/spectreconsole/spectre.console/pull/1161
* Do not register analyzer if SpectreConsole is not available in the current compilation by @meziantou in https://github.com/spectreconsole/spectre.console/pull/1172
* Ensure correct comparer is used for `TextPrompt` by @MartinZikmund in https://github.com/spectreconsole/spectre.console/pull/1152
* Forward CancellationToken to GetOperation by @meziantou in https://github.com/spectreconsole/spectre.console/pull/1173
* Fix minor typo in Prompt example by @Frassle in https://github.com/spectreconsole/spectre.console/pull/1183
* Fix coconut spelling by @phillip-haydon in https://github.com/spectreconsole/spectre.console/pull/1218
* Improve conversion error messages by @0xced in https://github.com/spectreconsole/spectre.console/pull/1141
* Make the code fix more robust and detect more symbols of type IAnsiConsole by @meziantou in https://github.com/spectreconsole/spectre.console/pull/1169
* Minor Refactorings by @Elisha-Aguilera in https://github.com/spectreconsole/spectre.console/pull/1081
* Simplify access to the SemanticModel in analyzers by @meziantou in https://github.com/spectreconsole/spectre.console/pull/1167
* Use SymbolEqualityComparer.Default when possible by @meziantou in https://github.com/spectreconsole/spectre.console/pull/1171
* Use StringComparison.Ordinal instead of culture-sensitive comparisons by @meziantou in https://github.com/spectreconsole/spectre.console/pull/1174

## Command line updates
* Add possibility to set description and/or data for the default command by @0xced in https://github.com/spectreconsole/spectre.console/pull/1091
* Add support for converting command parameters into FileInfo and DirectoryInfo by @0xced in https://github.com/spectreconsole/spectre.console/pull/1145
* Add support for arrays in [DefaultValue] attributes by @0xced in https://github.com/spectreconsole/spectre.console/pull/1164
* Add ability to pass example args using `params` syntax by @seclerp in https://github.com/spectreconsole/spectre.console/pull/1166
* Alias for branches by @ilyahryapko in https://github.com/spectreconsole/spectre.console/pull/1131
* Command line improvements by @FrankRay78 in https://github.com/spectreconsole/spectre.console/pull/1103

## Documentation updates
* Alignment => Justification Docs Fixes by @wbaldoumas in https://github.com/spectreconsole/spectre.console/pull/1143