# `Spectre.Console`

_[![Spectre.Console NuGet Version](https://img.shields.io/nuget/v/spectre.console.svg?style=flat&label=NuGet%3A%20Spectre.Console)](https://www.nuget.org/packages/spectre.console)_

`Spectre.Console`是一个 .NET 5/.NET Standard 2.0 的库，能让您在终端里更方便地生成精美的界面。

深受 [Rich](https://github.com/willmcgugan/rich) 这个优秀库的启发。

## 目录

1. [功能](#features)
2. [安装](#installing)
3. [文档](#documentation)
4. [例子](#examples)
5. [License](#license)

## 功能

* 编写时考虑到了单元测试。
* 支持 tables、grid、panel 和 [rich](https://github.com/willmcgugan/rich) 所支持的标记语言。
* 支持大部分的 SRG 参数，包括粗体、暗淡字、斜体、下划线、删除线和闪烁文本。
* 支持终端显示 3/4/8/24 位色。自动检测终端类型，自适应颜色范围。

![例子](resources/gfx/screenshots/example.png)

## 安装

最快的安装方式，就是用NuGet包管理直接安装Spectre.Console。

```csharp
dotnet add package Spectre.Console
```

## 文档

`Spectre.Console`的文档可以在这里查看
https://spectreconsole.github.io/spectre.console/

## 例子

如果想直接运行`Spectre.Console`的例子，则需要安装[dotnet-example](https://github.com/patriksvensson/dotnet-example)工具。

```
> dotnet tool restore
```

然后你可以列出仓库里的所有例子：

```
> dotnet example
```

跑一个看看效果：

```
> dotnet example tables
```

## License

版权所有 © Patrik Svensson, Phil Scott

Spectre.Console 基于 MIT 协议提供。查看 LICENSE 文件了解更多信息。

* SixLabors.ImageSharp 的协议请查看 https://github.com/SixLabors/ImageSharp/blob/master/LICENSE
