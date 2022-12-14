Title: Spectre.Console 0.41 released!
Description: In this release we (mostly Phil) have been focusing on getting the new fancy Roslyn Analyzers out the door...
Published: 2021-07-19
Category: Release Notes
Excluded: false
---

In this release, we (mostly [Phil](https://twitter.com/philco78)) have been focusing on getting the new fancy Roslyn Analyzers out the door.
If you want to try them out, add a reference to [Spectre.Console.Analyzer](https://www.nuget.org/packages/spectre.console.analyzer) in your project, and you should get some best practice tips in your favorite IDE!

It's summer in the northern hemisphere, so it will probably be a couple of weeks until the next release.

## Features

* [#417 - Support cancellation in prompts](https://github.com/spectreconsole/spectre.console/issues/417)
* [#324 - Remove AsciiTreeGuide as default tree guide](https://github.com/spectreconsole/spectre.console/issues/324)
* [#413 - Support custom characters at the end of a TextPrompt](https://github.com/spectreconsole/spectre.console/issues/413)
* [#447 - Alternative to the obsolete 'Select' function for selecting default items in SelectionPrompt](https://github.com/spectreconsole/spectre.console/issues/447)
* [#460 - Default values for Ask()](https://github.com/spectreconsole/spectre.console/issues/460)

## Bugs

* [#480 - IAnsiConsole.Confirm extension is missing default value parameter](https://github.com/spectreconsole/spectre.console/issues/480)
* [#442 - Allow dynamic Figlet hardblank](https://github.com/spectreconsole/spectre.console/pull/442)