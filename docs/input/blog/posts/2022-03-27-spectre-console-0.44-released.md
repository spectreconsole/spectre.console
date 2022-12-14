Title: Spectre.Console 0.44 released!
Description: Alternate screen buffers, better exception rendering... and more!
Published: 2022-03-27
Category: Release Notes
Excluded: false
---

Version 0.44 of Spectre.Console has been released!

There are a lot of fixes and improvements in this release, but the most noteworthy 
additions are better exception rendering, support for alternate screen buffers, 
and a new widget called [TextPath](xref:T:Spectre.Console.TextPath), which makes 
it easy to render paths in the console.

Check out the two new examples in our GitHub repository:

* [./examples/Console/AlternateScreen](https://github.com/spectreconsole/spectre.console/blob/main/examples/Console/AlternateScreen/Program.cs)
* [./examples/Console/Paths](https://github.com/spectreconsole/spectre.console/blob/main/examples/Console/Paths/Program.cs)

## New Contributors

* [@twaalewijn](https://github.com/twaalewijn) made their first contribution in [#642](https://github.com/spectreconsole/spectre.console/pull/642)
* [@zmrfzn](https://github.com/zmrfzn) made their first contribution in [#652](https://github.com/spectreconsole/spectre.console/pull/652)
* [@jouniheikniemi](https://github.com/jouniheikniemi) made their first contribution in [#689](https://github.com/spectreconsole/spectre.console/pull/689)
* [@Sir-Baconn](https://github.com/Sir-Baconn) made their first contribution in [#682](https://github.com/spectreconsole/spectre.console/pull/682)
* [@seclerp](https://github.com/seclerp) made their first contribution in [#665](https://github.com/spectreconsole/spectre.console/pull/665)
* [@Arcodiant](https://github.com/Arcodiant) made their first contribution in [#748](https://github.com/spectreconsole/spectre.console/pull/748)
* [@ap0llo](https://github.com/ap0llo) made their first contribution in [#681](https://github.com/spectreconsole/spectre.console/pull/681)

## What's Changed

* Added the ability to hide CommandOptions. by [@twaalewijn](https://github.com/twaalewijn) in [#642](https://github.com/spectreconsole/spectre.console/pull/642)
* Add support for alternate screen buffers by [@patriksvensson](https://github.com/patriksvensson) in [#647](https://github.com/spectreconsole/spectre.console/pull/647)
* Updates asciinema update by [@phil-scott-78](https://github.com/phil-scott-78) in [#648](https://github.com/spectreconsole/spectre.console/pull/648)
* Update Roslynator.Analyzers to version 3.3 by [@phil-scott-78](https://github.com/phil-scott-78) in [#650](https://github.com/spectreconsole/spectre.console/pull/650)
* Ordered CommandArguments by position by [@nils-a](https://github.com/nils-a) in [#646](https://github.com/spectreconsole/spectre.console/pull/646)
* Table title's first letter is getting capitalized by default by [@zmrfzn](https://github.com/zmrfzn) in [#652](https://github.com/spectreconsole/spectre.console/pull/652)
* Simplify stack frame parsing by [@0xced](https://github.com/0xced) in [#637](https://github.com/spectreconsole/spectre.console/pull/637)
* Trying to rework dotnet tools for docs and playwright by [@phil-scott-78](https://github.com/phil-scott-78) in [#667](https://github.com/spectreconsole/spectre.console/pull/667)
* Add release notes for 0.43.0 by [@patriksvensson](https://github.com/patriksvensson) in [#663](https://github.com/spectreconsole/spectre.console/pull/663)
* Use file scoped namespace declarations by [@patriksvensson](https://github.com/patriksvensson) in [#666](https://github.com/spectreconsole/spectre.console/pull/666)
* Add global usings by [@patriksvensson](https://github.com/patriksvensson) in [#668](https://github.com/spectreconsole/spectre.console/pull/668)
* Changed TypeResolver in Cli/logging by [@nils-a](https://github.com/nils-a) in [#675](https://github.com/spectreconsole/spectre.console/pull/675)
* Invalid WithExample call on the CommandApp doc page by [@jouniheikniemi](https://github.com/jouniheikniemi) in [#689](https://github.com/spectreconsole/spectre.console/pull/689)
* Add overload with params array for AddChoiceGroup() by [@Sir-Baconn](https://github.com/Sir-Baconn) in [#682](https://github.com/spectreconsole/spectre.console/pull/682)
* Simplify exception formatting by [@0xced](https://github.com/0xced) in [#695](https://github.com/spectreconsole/spectre.console/pull/695)
* Improves exception rendering for async methods in .NET 6 by [@phil-scott-78](https://github.com/phil-scott-78) in [#718](https://github.com/spectreconsole/spectre.console/pull/718)
* Adding better type names for return types and parameters by [@phil-scott-78](https://github.com/phil-scott-78) in [#719](https://github.com/spectreconsole/spectre.console/pull/719)
* Docs redesign by [@phil-scott-78](https://github.com/phil-scott-78) in [#728](https://github.com/spectreconsole/spectre.console/pull/728)
* Configures deployment to netlify by [@phil-scott-78](https://github.com/phil-scott-78) in [#729](https://github.com/spectreconsole/spectre.console/pull/729)
* Tweaking font-size and line-height for smaller screens by [@phil-scott-78](https://github.com/phil-scott-78) in [#730](https://github.com/spectreconsole/spectre.console/pull/730)
* Adding a best practices guide. by [@phil-scott-78](https://github.com/phil-scott-78) in [#744](https://github.com/spectreconsole/spectre.console/pull/744)
* Adds widget for displaying (pretty) paths by [@patriksvensson](https://github.com/patriksvensson) in [#734](https://github.com/spectreconsole/spectre.console/pull/734)
* Add missing inheritance and nullable types in settings.md by [@seclerp](https://github.com/seclerp) in [#665](https://github.com/spectreconsole/spectre.console/pull/665)
* Optimizing build by [@phil-scott-78](https://github.com/phil-scott-78) in [#747](https://github.com/spectreconsole/spectre.console/pull/747)
* Update SixLabors.ImageSharp reference by [@Arcodiant](https://github.com/Arcodiant) in [#748](https://github.com/spectreconsole/spectre.console/pull/748)
* Add support for styling and aligning paths by [@patriksvensson](https://github.com/patriksvensson) in [#746](https://github.com/spectreconsole/spectre.console/pull/746)
* Add documentation for `TextPath` widget by [@patriksvensson](https://github.com/patriksvensson) in [#757](https://github.com/spectreconsole/spectre.console/pull/757)
* Fix path and description for TextPath docs by [@patriksvensson](https://github.com/patriksvensson) in [#758](https://github.com/spectreconsole/spectre.console/pull/758)
* Adding a short template file for new documentation by [@phil-scott-78](https://github.com/phil-scott-78) in [#763](https://github.com/spectreconsole/spectre.console/pull/763)
* Introduce MarkupInterpolated and MarkupLineInterpolated extensions by [@phil-scott-78](https://github.com/phil-scott-78) in [#761](https://github.com/spectreconsole/spectre.console/pull/761)
* Obsolete the AnsiConsoleFactory class by [@0xced](https://github.com/0xced) in [#585](https://github.com/spectreconsole/spectre.console/pull/585)
* Add option to configure the style of the choices list and the default value in TextPrompt by [@ap0llo](https://github.com/ap0llo) in [#681](https://github.com/spectreconsole/spectre.console/pull/681)