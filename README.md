# `Spectre.Console`

_[![Spectre.IO NuGet Version](https://img.shields.io/nuget/v/spectre.io.svg?style=flat&label=NuGet%3A%20Spectre.Console)](https://www.nuget.org/packages/spectre.console)_

A .NET Standard 2.0 library that makes it easier to create beautiful console applications.  
It is heavily inspired by the excellent [Rich library](https://github.com/willmcgugan/rich) 
for Python.

## Table of Contents

1. [Features](#features)
2. [Example](#example)
3. [Usage](#usage)  
   3.1. [Using the static API](#using-the-static-api)  
   3.2. [Creating a console](#creating-a-console)
4. [Available styles](#available-styles)
5. [Predefiend colors](#predefined-colors)

## Features

* Written with unit testing in mind.
* Supports the most common SRG parameters when it comes to text 
  styling such as bold, dim, italic, underline, strikethrough, 
  and blinking text.
* Supports 3/4/8/24-bit colors in the terminal.  
  The library will detect the capabilities of the current terminal 
  and downgrade colors as needed.

## Example

![Example](https://spectresystems.se/assets/open-source/spectre-console/example.png)

## Usage

The `Spectre.Console` API is stateful and is not thread-safe.
If you need to write to the console from different threads, make sure that 
you take appropriate precautions, just like when you use the 
regular `System.Console` API.

If the current terminal does not support ANSI escape sequences, 
`Spectre.Console` will fallback to using the `System.Console` API.

_NOTE: This library is currently under development and API's 
might change or get removed at any point up until a 1.0 release._

### Using the static API

The static API is perfect when you just want to output text
like you usually do with the `System.Console` API, but prettier.

```csharp
AnsiConsole.Foreground = Color.CornflowerBlue;
AnsiConsole.Style = Styles.Underline | Styles.Bold;
AnsiConsole.WriteLine("Hello World!");

AnsiConsole.Reset();
AnsiConsole.MarkupLine("[yellow]{0}[/] [underline]world[/]!", "Goodbye");
```

If you want to get a reference to the default `IAnsiConsole`, 
you can access it via `AnsiConsole.Console`.

### Creating a console

Sometimes it's useful to explicitly create a console with specific 
capabilities, such as during unit testing when you want control 
over the environment your code runs in.

```csharp
IAnsiConsole console = AnsiConsole.Create(
    new AnsiConsoleSettings()
    {
        Ansi = AnsiSupport.Yes,
        ColorSystem = ColorSystemSupport.TrueColor,
        Out = new StringWriter(),
    });
```

_NOTE: Even if you can specify a specific color system to use 
when manually creating a console, remember that the user's terminal 
might not be able to use it, so unless you're creating an IAnsiConsole 
for testing, always use `ColorSystemSupport.Detect` and `AnsiSupport.Detect`._

## Available styles

_NOTE: Not all styles are supported in every terminal._

Name | Description
--- | ---
`bold` | Bold text
`dim` | Dim or faint text
`italic` | Italic text
`underline` | Underlined text
`invert` | Swaps the foreground and background colors
`conceal` | Hides the text
`slowblink` | Makes text blink slowly
`rapidblink` | Makes text blink
`strikethrough` | Shows text with a horizontal line through the center

## Predefined colors

Number | Name | RGB | Hex | System.ConsoleColor
--- | --- | --- | --- | ---
`0` | `black` | `0,0,0` | `#000000` | `Black`
`1` | `maroon` | `128,0,0` | `#800000` | `DarkRed`
`2` | `green` | `0,128,0` | `#008000` | `DarkGreen`
`3` | `olive` | `128,128,0` | `#808000` | `DarkYellow`
`4` | `navy` | `0,0,128` | `#000080` | `DarkBlue`
`5` | `purple` | `128,0,128` | `#800080` | `DarkMagenta`
`6` | `teal` | `0,128,128` | `#008080` | `DarkCyan`
`7` | `silver` | `192,192,192` | `#c0c0c0` | `Gray`
`8` | `grey` | `128,128,128` | `#808080` | `DarkGray`
`9` | `red` | `255,0,0` | `#ff0000` | `Red`
`10` | `lime` | `0,255,0` | `#00ff00` | `Green`
`11` | `yellow` | `255,255,0` | `#ffff00` | `Yellow`
`12` | `blue` | `0,0,255` | `#0000ff` | `Blue`
`13` | `fuchsia` | `255,0,255` | `#ff00ff` | `Magenta`
`14` | `aqua` | `0,255,255` | `#00ffff` | `Cyan`
`15` | `white` | `255,255,255` | `#ffffff` | `White`
`16` | `grey0` | `0,0,0` | `#000000`
`17` | `navyblue` | `0,0,95` | `#00005f`
`18` | `darkblue` | `0,0,135` | `#000087`
`19` | `blue3` | `0,0,175` | `#0000af`
`20` | `blue3_1` | `0,0,215` | `#0000d7`
`21` | `blue1` | `0,0,255` | `#0000ff`
`22` | `darkgreen` | `0,95,0` | `#005f00`
`23` | `deepskyblue4` | `0,95,95` | `#005f5f`
`24` | `deepskyblue4_1` | `0,95,135` | `#005f87`
`25` | `deepskyblue4_2` | `0,95,175` | `#005faf`
`26` | `dodgerblue3` | `0,95,215` | `#005fd7`
`27` | `dodgerblue2` | `0,95,255` | `#005fff`
`28` | `green4` | `0,135,0` | `#008700`
`29` | `springgreen4` | `0,135,95` | `#00875f`
`30` | `turquoise4` | `0,135,135` | `#008787`
`31` | `deepskyblue3` | `0,135,175` | `#0087af`
`32` | `deepskyblue3_1` | `0,135,215` | `#0087d7`
`33` | `dodgerblue1` | `0,135,255` | `#0087ff`
`34` | `green3` | `0,175,0` | `#00af00`
`35` | `springgreen3` | `0,175,95` | `#00af5f`
`36` | `darkcyan` | `0,175,135` | `#00af87`
`37` | `lightseagreen` | `0,175,175` | `#00afaf`
`38` | `deepskyblue2` | `0,175,215` | `#00afd7`
`39` | `deepskyblue1` | `0,175,255` | `#00afff`
`40` | `green3_1` | `0,215,0` | `#00d700`
`41` | `springgreen3_1` | `0,215,95` | `#00d75f`
`42` | `springgreen2` | `0,215,135` | `#00d787`
`43` | `cyan3` | `0,215,175` | `#00d7af`
`44` | `darkturquoise` | `0,215,215` | `#00d7d7`
`45` | `turquoise2` | `0,215,255` | `#00d7ff`
`46` | `green1` | `0,255,0` | `#00ff00`
`47` | `springgreen2_1` | `0,255,95` | `#00ff5f`
`48` | `springgreen1` | `0,255,135` | `#00ff87`
`49` | `mediumspringgreen` | `0,255,175` | `#00ffaf`
`50` | `cyan2` | `0,255,215` | `#00ffd7`
`51` | `cyan1` | `0,255,255` | `#00ffff`
`52` | `darkred` | `95,0,0` | `#5f0000`
`53` | `deeppink4` | `95,0,95` | `#5f005f`
`54` | `purple4` | `95,0,135` | `#5f0087`
`55` | `purple4_1` | `95,0,175` | `#5f00af`
`56` | `purple3` | `95,0,215` | `#5f00d7`
`57` | `blueviolet` | `95,0,255` | `#5f00ff`
`58` | `orange4` | `95,95,0` | `#5f5f00`
`59` | `grey37` | `95,95,95` | `#5f5f5f`
`60` | `mediumpurple4` | `95,95,135` | `#5f5f87`
`61` | `slateblue3` | `95,95,175` | `#5f5faf`
`62` | `slateblue3_1` | `95,95,215` | `#5f5fd7`
`63` | `royalblue1` | `95,95,255` | `#5f5fff`
`64` | `chartreuse4` | `95,135,0` | `#5f8700`
`65` | `darkseagreen4` | `95,135,95` | `#5f875f`
`66` | `paleturquoise4` | `95,135,135` | `#5f8787`
`67` | `steelblue` | `95,135,175` | `#5f87af`
`68` | `steelblue3` | `95,135,215` | `#5f87d7`
`69` | `cornflowerblue` | `95,135,255` | `#5f87ff`
`70` | `chartreuse3` | `95,175,0` | `#5faf00`
`71` | `darkseagreen4_1` | `95,175,95` | `#5faf5f`
`72` | `cadetblue` | `95,175,135` | `#5faf87`
`73` | `cadetblue_1` | `95,175,175` | `#5fafaf`
`74` | `skyblue3` | `95,175,215` | `#5fafd7`
`75` | `steelblue1` | `95,175,255` | `#5fafff`
`76` | `chartreuse3_1` | `95,215,0` | `#5fd700`
`77` | `palegreen3` | `95,215,95` | `#5fd75f`
`78` | `seagreen3` | `95,215,135` | `#5fd787`
`79` | `aquamarine3` | `95,215,175` | `#5fd7af`
`80` | `mediumturquoise` | `95,215,215` | `#5fd7d7`
`81` | `steelblue1_1` | `95,215,255` | `#5fd7ff`
`82` | `chartreuse2` | `95,255,0` | `#5fff00`
`83` | `seagreen2` | `95,255,95` | `#5fff5f`
`84` | `seagreen1` | `95,255,135` | `#5fff87`
`85` | `seagreen1_1` | `95,255,175` | `#5fffaf`
`86` | `aquamarine1` | `95,255,215` | `#5fffd7`
`87` | `darkslategray2` | `95,255,255` | `#5fffff`
`88` | `darkred_1` | `135,0,0` | `#870000`
`89` | `deeppink4_1` | `135,0,95` | `#87005f`
`90` | `darkmagenta` | `135,0,135` | `#870087`
`91` | `darkmagenta_1` | `135,0,175` | `#8700af`
`92` | `darkviolet` | `135,0,215` | `#8700d7`
`93` | `purple_1` | `135,0,255` | `#8700ff`
`94` | `orange4_1` | `135,95,0` | `#875f00`
`95` | `lightpink4` | `135,95,95` | `#875f5f`
`96` | `plum4` | `135,95,135` | `#875f87`
`97` | `mediumpurple3` | `135,95,175` | `#875faf`
`98` | `mediumpurple3_1` | `135,95,215` | `#875fd7`
`99` | `slateblue1` | `135,95,255` | `#875fff`
`100` | `yellow4` | `135,135,0` | `#878700`
`101` | `wheat4` | `135,135,95` | `#87875f`
`102` | `grey53` | `135,135,135` | `#878787`
`103` | `lightslategrey` | `135,135,175` | `#8787af`
`104` | `mediumpurple` | `135,135,215` | `#8787d7`
`105` | `lightslateblue` | `135,135,255` | `#8787ff`
`106` | `yellow4_1` | `135,175,0` | `#87af00`
`107` | `darkolivegreen3` | `135,175,95` | `#87af5f`
`108` | `darkseagreen` | `135,175,135` | `#87af87`
`109` | `lightskyblue3` | `135,175,175` | `#87afaf`
`110` | `lightskyblue3_1` | `135,175,215` | `#87afd7`
`111` | `skyblue2` | `135,175,255` | `#87afff`
`112` | `chartreuse2_1` | `135,215,0` | `#87d700`
`113` | `darkolivegreen3_1` | `135,215,95` | `#87d75f`
`114` | `palegreen3_1` | `135,215,135` | `#87d787`
`115` | `darkseagreen3` | `135,215,175` | `#87d7af`
`116` | `darkslategray3` | `135,215,215` | `#87d7d7`
`117` | `skyblue1` | `135,215,255` | `#87d7ff`
`118` | `chartreuse1` | `135,255,0` | `#87ff00`
`119` | `lightgreen` | `135,255,95` | `#87ff5f`
`120` | `lightgreen_1` | `135,255,135` | `#87ff87`
`121` | `palegreen1` | `135,255,175` | `#87ffaf`
`122` | `aquamarine1_1` | `135,255,215` | `#87ffd7`
`123` | `darkslategray1` | `135,255,255` | `#87ffff`
`124` | `red3` | `175,0,0` | `#af0000`
`125` | `deeppink4_2` | `175,0,95` | `#af005f`
`126` | `mediumvioletred` | `175,0,135` | `#af0087`
`127` | `magenta3` | `175,0,175` | `#af00af`
`128` | `darkviolet_1` | `175,0,215` | `#af00d7`
`129` | `purple_2` | `175,0,255` | `#af00ff`
`130` | `darkorange3` | `175,95,0` | `#af5f00`
`131` | `indianred` | `175,95,95` | `#af5f5f`
`132` | `hotpink3` | `175,95,135` | `#af5f87`
`133` | `mediumorchid3` | `175,95,175` | `#af5faf`
`134` | `mediumorchid` | `175,95,215` | `#af5fd7`
`135` | `mediumpurple2` | `175,95,255` | `#af5fff`
`136` | `darkgoldenrod` | `175,135,0` | `#af8700`
`137` | `lightsalmon3` | `175,135,95` | `#af875f`
`138` | `rosybrown` | `175,135,135` | `#af8787`
`139` | `grey63` | `175,135,175` | `#af87af`
`140` | `mediumpurple2_1` | `175,135,215` | `#af87d7`
`141` | `mediumpurple1` | `175,135,255` | `#af87ff`
`142` | `gold3` | `175,175,0` | `#afaf00`
`143` | `darkkhaki` | `175,175,95` | `#afaf5f`
`144` | `navajowhite3` | `175,175,135` | `#afaf87`
`145` | `grey69` | `175,175,175` | `#afafaf`
`146` | `lightsteelblue3` | `175,175,215` | `#afafd7`
`147` | `lightsteelblue` | `175,175,255` | `#afafff`
`148` | `yellow3` | `175,215,0` | `#afd700`
`149` | `darkolivegreen3_2` | `175,215,95` | `#afd75f`
`150` | `darkseagreen3_1` | `175,215,135` | `#afd787`
`151` | `darkseagreen2` | `175,215,175` | `#afd7af`
`152` | `lightcyan3` | `175,215,215` | `#afd7d7`
`153` | `lightskyblue1` | `175,215,255` | `#afd7ff`
`154` | `greenyellow` | `175,255,0` | `#afff00`
`155` | `darkolivegreen2` | `175,255,95` | `#afff5f`
`156` | `palegreen1_1` | `175,255,135` | `#afff87`
`157` | `darkseagreen2_1` | `175,255,175` | `#afffaf`
`158` | `darkseagreen1` | `175,255,215` | `#afffd7`
`159` | `paleturquoise1` | `175,255,255` | `#afffff`
`160` | `red3_1` | `215,0,0` | `#d70000`
`161` | `deeppink3` | `215,0,95` | `#d7005f`
`162` | `deeppink3_1` | `215,0,135` | `#d70087`
`163` | `magenta3_1` | `215,0,175` | `#d700af`
`164` | `magenta3_2` | `215,0,215` | `#d700d7`
`165` | `magenta2` | `215,0,255` | `#d700ff`
`166` | `darkorange3_1` | `215,95,0` | `#d75f00`
`167` | `indianred_1` | `215,95,95` | `#d75f5f`
`168` | `hotpink3_1` | `215,95,135` | `#d75f87`
`169` | `hotpink2` | `215,95,175` | `#d75faf`
`170` | `orchid` | `215,95,215` | `#d75fd7`
`171` | `mediumorchid1` | `215,95,255` | `#d75fff`
`172` | `orange3` | `215,135,0` | `#d78700`
`173` | `lightsalmon3_1` | `215,135,95` | `#d7875f`
`174` | `lightpink3` | `215,135,135` | `#d78787`
`175` | `pink3` | `215,135,175` | `#d787af`
`176` | `plum3` | `215,135,215` | `#d787d7`
`177` | `violet` | `215,135,255` | `#d787ff`
`178` | `gold3_1` | `215,175,0` | `#d7af00`
`179` | `lightgoldenrod3` | `215,175,95` | `#d7af5f`
`180` | `tan` | `215,175,135` | `#d7af87`
`181` | `mistyrose3` | `215,175,175` | `#d7afaf`
`182` | `thistle3` | `215,175,215` | `#d7afd7`
`183` | `plum2` | `215,175,255` | `#d7afff`
`184` | `yellow3_1` | `215,215,0` | `#d7d700`
`185` | `khaki3` | `215,215,95` | `#d7d75f`
`186` | `lightgoldenrod2` | `215,215,135` | `#d7d787`
`187` | `lightyellow3` | `215,215,175` | `#d7d7af`
`188` | `grey84` | `215,215,215` | `#d7d7d7`
`189` | `lightsteelblue1` | `215,215,255` | `#d7d7ff`
`190` | `yellow2` | `215,255,0` | `#d7ff00`
`191` | `darkolivegreen1` | `215,255,95` | `#d7ff5f`
`192` | `darkolivegreen1_1` | `215,255,135` | `#d7ff87`
`193` | `darkseagreen1_1` | `215,255,175` | `#d7ffaf`
`194` | `honeydew2` | `215,255,215` | `#d7ffd7`
`195` | `lightcyan1` | `215,255,255` | `#d7ffff`
`196` | `red1` | `255,0,0` | `#ff0000`
`197` | `deeppink2` | `255,0,95` | `#ff005f`
`198` | `deeppink1` | `255,0,135` | `#ff0087`
`199` | `deeppink1_1` | `255,0,175` | `#ff00af`
`200` | `magenta2_1` | `255,0,215` | `#ff00d7`
`201` | `magenta1` | `255,0,255` | `#ff00ff`
`202` | `orangered1` | `255,95,0` | `#ff5f00`
`203` | `indianred1` | `255,95,95` | `#ff5f5f`
`204` | `indianred1_1` | `255,95,135` | `#ff5f87`
`205` | `hotpink` | `255,95,175` | `#ff5faf`
`206` | `hotpink_1` | `255,95,215` | `#ff5fd7`
`207` | `mediumorchid1_1` | `255,95,255` | `#ff5fff`
`208` | `darkorange` | `255,135,0` | `#ff8700`
`209` | `salmon1` | `255,135,95` | `#ff875f`
`210` | `lightcoral` | `255,135,135` | `#ff8787`
`211` | `palevioletred1` | `255,135,175` | `#ff87af`
`212` | `orchid2` | `255,135,215` | `#ff87d7`
`213` | `orchid1` | `255,135,255` | `#ff87ff`
`214` | `orange1` | `255,175,0` | `#ffaf00`
`215` | `sandybrown` | `255,175,95` | `#ffaf5f`
`216` | `lightsalmon1` | `255,175,135` | `#ffaf87`
`217` | `lightpink1` | `255,175,175` | `#ffafaf`
`218` | `pink1` | `255,175,215` | `#ffafd7`
`219` | `plum1` | `255,175,255` | `#ffafff`
`220` | `gold1` | `255,215,0` | `#ffd700`
`221` | `lightgoldenrod2_1` | `255,215,95` | `#ffd75f`
`222` | `lightgoldenrod2_2` | `255,215,135` | `#ffd787`
`223` | `navajowhite1` | `255,215,175` | `#ffd7af`
`224` | `mistyrose1` | `255,215,215` | `#ffd7d7`
`225` | `thistle1` | `255,215,255` | `#ffd7ff`
`226` | `yellow1` | `255,255,0` | `#ffff00`
`227` | `lightgoldenrod1` | `255,255,95` | `#ffff5f`
`228` | `khaki1` | `255,255,135` | `#ffff87`
`229` | `wheat1` | `255,255,175` | `#ffffaf`
`230` | `cornsilk1` | `255,255,215` | `#ffffd7`
`231` | `grey100` | `255,255,255` | `#ffffff`
`232` | `grey3` | `8,8,8` | `#080808`
`233` | `grey7` | `18,18,18` | `#121212`
`234` | `grey11` | `28,28,28` | `#1c1c1c`
`235` | `grey15` | `38,38,38` | `#262626`
`236` | `grey19` | `48,48,48` | `#303030`
`237` | `grey23` | `58,58,58` | `#3a3a3a`
`238` | `grey27` | `68,68,68` | `#444444`
`239` | `grey30` | `78,78,78` | `#4e4e4e`
`240` | `grey35` | `88,88,88` | `#585858`
`241` | `grey39` | `98,98,98` | `#626262`
`242` | `grey42` | `108,108,108` | `#6c6c6c`
`243` | `grey46` | `118,118,118` | `#767676`
`244` | `grey50` | `128,128,128` | `#808080`
`245` | `grey54` | `138,138,138` | `#8a8a8a`
`246` | `grey58` | `148,148,148` | `#949494`
`247` | `grey62` | `158,158,158` | `#9e9e9e`
`248` | `grey66` | `168,168,168` | `#a8a8a8`
`249` | `grey70` | `178,178,178` | `#b2b2b2`
`250` | `grey74` | `188,188,188` | `#bcbcbc`
`251` | `grey78` | `198,198,198` | `#c6c6c6`
`252` | `grey82` | `208,208,208` | `#d0d0d0`
`253` | `grey85` | `218,218,218` | `#dadada`
`254` | `grey89` | `228,228,228` | `#e4e4e4`
`255` | `grey93` | `238,238,238` | `#eeeeee`
