# `Spectre.Console`

_[![Spectre.Console NuGet Version](https://img.shields.io/nuget/v/spectre.console.svg?style=flat&label=NuGet%3A%20Spectre.Console)](https://www.nuget.org/packages/spectre.console)_

`Spectre.Console`是一个.NET Standard 2.0 的库，能让您在终端里更方便地生成精美的界面。

受 [Rich](https://github.com/willmcgugan/rich) 这个优秀库的启发。

## 目录

1. [功能](#features)
2. [例子](#example)
3. [安装](#installing)
4. [使用](#usage)  
   4.1. [Using the static API](#using-the-static-api)  
   4.2. [Creating a console](#creating-a-console)
5. [运行例程](#running-examples)

## 功能

* 编写时考虑到了单元测试。
* 支持 tables、grid、panel 和 [rich](https://github.com/willmcgugan/rich) 所支持的标记语言。
* 支持大部分的 SRG 参数，包括粗体、暗淡字、斜体、下划线、删除线和闪烁文本。
* 支持终端显示 3/4/8/24 位色。

  库会自动检测终端类型，自适应颜色范围。

## 例子

![Example](resources/gfx/screenshots/example.png)

## 安装

最快的安装方式，就是用NuGet包管理直接安装Spectre.Console。

```csharp
dotnet add package Spectre.Console
```

## 使用

`Spectre.Console` API 是有状态并且是非线程安全的。
如果你需要在不同的线程中显示数据，确保你采取了适当的防护措施，就像你平常使用`System.Console` API 时一样。

如果当前终端不支持ASCII转义字符，`Spectre.Console`会使用`System.Console` API 作为替代。

_备注: 这个库仍在开发中，在 1.0 版本发布前，现有接口可能会被更改或删除。_

### 使用 static API

当你想像`System.Console` API 一样输出文本时，使用 static API 是最完美的，显示效果也更美观。

```csharp
AnsiConsole.Foreground = Color.CornflowerBlue;
AnsiConsole.Decoration = Decoration.Underline | Decoration.Bold;
AnsiConsole.WriteLine("Hello World!");

AnsiConsole.Reset();
AnsiConsole.MarkupLine("[bold yellow on red]{0}[/] [underline]world[/]!", "Goodbye");
```

如果你想取得`IAnsiConsole`的默认引用，可以通过`AnsiConsole.Console`获得。

### 创建控制台

在某些情况下，显性的创建一个控制台对象是有用处的，比如你想在单元测试时，明确代码的运行环境。

不建议在单元测试时在代码中使用`AnsiConsole`。

```csharp
IAnsiConsole console = AnsiConsole.Create(
    new AnsiConsoleSettings()
    {
        Ansi = AnsiSupport.Yes,
        ColorSystem = ColorSystemSupport.TrueColor,
        Out = new StringWriter(),
    });
```

_备注: 即使你可以在创建命令行对象时手动指定颜色系统，用户的环境也不一定支持这些颜色。
所以除了你使用 IAnsiConsole 用于测试之外，你应当始终使用 `ColorSystemSupport.Detect` 和 `AnsiSupport.Detect`。_

## 运行例程

为了运行 Spectre.Console，你需要安装
[dotnet-example](https://github.com/patriksvensson/dotnet-example)
全局工具。

```
> dotnet tool restore
```

现在可以列出这个仓库里可用的例子：

```
> dotnet example

╭────────────┬───────────────────────────────────────┬──────────────────────────────────────────────────────╮
│ Name       │ Path                                  │ Description                                          │
├────────────┼───────────────────────────────────────┼──────────────────────────────────────────────────────┤
│ Borders    │ examples/Borders/Borders.csproj       │ Demonstrates the different kind of borders.          │
│ Calendars  │ examples/Calendars/Calendars.csproj   │ Demonstrates how to render calendars.                │
│ Colors     │ examples/Colors/Colors.csproj         │ Demonstrates how to use colors in the console.       │
│ Columns    │ examples/Columns/Columns.csproj       │ Demonstrates how to render data into columns.        │
│ Emojis     │ examples/Emojis/Emojis.csproj         │ Demonstrates how to render emojis.                   │
│ Exceptions │ examples/Exceptions/Exceptions.csproj │ Demonstrates how to render formatted exceptions.     │
│ Grids      │ examples/Grids/Grids.csproj           │ Demonstrates how to render grids in a console.       │
│ Info       │ examples/Info/Info.csproj             │ Displays the capabilities of the current console.    │
│ Links      │ examples/Links/Links.csproj           │ Demonstrates how to render links in a console.       │
│ Panels     │ examples/Panels/Panels.csproj         │ Demonstrates how to render items in panels.          │
│ Rules      │ examples/Rules/Rules.csproj           │ Demonstrates how to render horizontal rules (lines). │
│ Tables     │ examples/Tables/Tables.csproj         │ Demonstrates how to render tables in a console.      │
╰────────────┴───────────────────────────────────────┴──────────────────────────────────────────────────────╯
```

然后运行一个例子：

```
> dotnet example tables
┌──────────┬──────────┬────────┐
│ Foo      │ Bar      │ Baz    │
├──────────┼──────────┼────────┤
│ Hello    │ World!   │        │
│ Bonjour  │ le       │ monde! │
│ Hej      │ Världen! │        │
└──────────┴──────────┴────────┘
```
