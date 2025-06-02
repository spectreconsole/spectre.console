Title: Spectre.Console 0.48 released!
Description: .NET 8, custom help providers, and more!
Published: 2023-11-22
Category: Release Notes
Excluded: false
---

Version 0.48 of Spectre.Console has been released!

Several rendering issues have been addressed, such as fixing problems related to rendering inside status causing corrupt output, avoiding exceptions on Rows with no children, as well as addressing rendering bugs in TextPath.

New features have been added, such as the ability to show separators between table rows. Other notable additions include progress bar header and footer support, customizable (and localizable) help providers, and the option to style text and confirmation prompts.

# New Contributors

* [@icalvo](https://github.com/icalvo) made their first contribution in [#1215](https://github.com/spectreconsole/spectre.console/pull/1215)
* [@fredrikbentzen](https://github.com/fredrikbentzen) made their first contribution in [#1132](https://github.com/spectreconsole/spectre.console/pull/1132)
* [@jeppevammenkristensen](https://github.com/jeppevammenkristensen) made their first contribution in [#1241](https://github.com/spectreconsole/spectre.console/pull/1241)
* [@tomaszprasolek](https://github.com/tomaszprasolek) made their first contribution in [#1257](https://github.com/spectreconsole/spectre.console/pull/1257)
* [@olabacker](https://github.com/olabacker) made their first contribution in [#1302](https://github.com/spectreconsole/spectre.console/pull/1302)
* [@AndrewRathbun](https://github.com/AndrewRathbun) made their first contribution in [#1315](https://github.com/spectreconsole/spectre.console/pull/1315)


# What's Changed

## Rendering

* Add .NET 8 support by [@patriksvensson](https://github.com/patriksvensson) in [#1367](https://github.com/spectreconsole/spectre.console/pull/1367)
* Fixed render issue where writeline inside status caused corrupt output #415 #694 by [@fredrikbentzen](https://github.com/fredrikbentzen) in [#1132](https://github.com/spectreconsole/spectre.console/pull/1132)
* Relax the SDK requirements by rolling forward to the latest feature by [@0xced](https://github.com/0xced) in [#1237](https://github.com/spectreconsole/spectre.console/pull/1237)
* Add fix to avoid exception on rows with no children by [@jeppevammenkristensen](https://github.com/jeppevammenkristensen) in [#1241](https://github.com/spectreconsole/spectre.console/pull/1241)
* Set `end_of_line` to `LF` instead of `CRLF` by [@0xced](https://github.com/0xced) in [#1256](https://github.com/spectreconsole/spectre.console/pull/1256)
* Fix `Rule` widget docs by [@tomaszprasolek](https://github.com/tomaszprasolek) in [#1257](https://github.com/spectreconsole/spectre.console/pull/1257)
* Added the missing columns-cast by [@nils](https://github.com/nils)-a in [#1294](https://github.com/spectreconsole/spectre.console/pull/1294)
* Render tables with zero-width columns by [@Frassle](https://github.com/Frassle) in [#1197](https://github.com/spectreconsole/spectre.console/pull/1197)
* Fix figlet centering possibly throwing due to negative size by [@olabacker](https://github.com/olabacker) in [#1302](https://github.com/spectreconsole/spectre.console/pull/1302)
* Add option to show separator between table rows  by [@patriksvensson](https://github.com/patriksvensson) in [#1304](https://github.com/spectreconsole/spectre.console/pull/1304)
* Enable setting the color of the values in a `BreakdownChart` by [@nils](https://github.com/nils)-a in [#1303](https://github.com/spectreconsole/spectre.console/pull/1303)
* Progress bar header and footer by [@phil](https://github.com/phil)-scott-78 in [#1262](https://github.com/spectreconsole/spectre.console/pull/1262)
* Add an example showing the decorations off by [@Frassle](https://github.com/Frassle) in [#1191](https://github.com/spectreconsole/spectre.console/pull/1191)
* Fixes `TextPath` rendering bugs by [@patriksvensson](https://github.com/patriksvensson) in [#1308](https://github.com/spectreconsole/spectre.console/pull/1308)
* Fix greedy row measure by [@nils](https://github.com/nils)-a in [#1338](https://github.com/spectreconsole/spectre.console/pull/1338)
* Fix `AnsiConsoleOutput` safe height by [@0xced](https://github.com/0xced) in [#1358](https://github.com/spectreconsole/spectre.console/pull/1358)
* Allow passing a nullable style in `DefaultValueStyle()` and `ChoicesStyle()` by [@0xced](https://github.com/0xced) in [#1359](https://github.com/spectreconsole/spectre.console/pull/1359)
* Allow `ConfirmationPrompt` Styling by [@wbaldoumas](https://github.com/wbaldoumas) in [#1210](https://github.com/spectreconsole/spectre.console/pull/1210)

## CLI
* Add async command unit tests by [@FrankRay78](https://github.com/FrankRay78) in [#1228](https://github.com/spectreconsole/spectre.console/pull/1228)
* Add support for async delegate by [@icalvo](https://github.com/icalvo) in [#1215](https://github.com/spectreconsole/spectre.console/pull/1215)
* Remove unnecessary `[NotNull]` attributes by [@0xced](https://github.com/0xced) in [#1255](https://github.com/spectreconsole/spectre.console/pull/1255)
* Allow custom help providers by [@FrankRay78](https://github.com/FrankRay78) in [#1259](https://github.com/spectreconsole/spectre.console/pull/1259)
* Specified details for settings for the argument vector by [@nils](https://github.com/nils)-a in [#1301](https://github.com/spectreconsole/spectre.console/pull/1301)
* Add support for localisation in help provider by [@FrankRay78](https://github.com/FrankRay78) in [#1349](https://github.com/spectreconsole/spectre.console/pull/1349)
* Fix DefaultValue for `FileInfo` and `DirectoryInfo` by [@0xced](https://github.com/0xced) in [#1238](https://github.com/spectreconsole/spectre.console/pull/1238)

## Documentation & Samples
* Added a minimal PR template by [@nils](https://github.com/nils)-a in [#1318](https://github.com/spectreconsole/spectre.console/pull/1318)
* Fix typo in `showcase` sample by [@AndrewRathbun](https://github.com/AndrewRathbun) in [#1315](https://github.com/spectreconsole/spectre.console/pull/1315)
* Update `columns` sample to showcase nicer data by [@nils](https://github.com/nils)-a in [#1295](https://github.com/spectreconsole/spectre.console/pull/1295)
* Change all `SetErrorHandler` to `SetExceptionHandler` by [@nils](https://github.com/nils)-a in [#1298](https://github.com/spectreconsole/spectre.console/pull/1298)

## Other stuff
* Ensure the `Generator` project compiles by [@patriksvensson](https://github.com/patriksvensson) in [#1371](https://github.com/spectreconsole/spectre.console/pull/1371)