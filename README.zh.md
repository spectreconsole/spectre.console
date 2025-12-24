# `Spectre.Console`

_[![Spectre.Console NuGet Version](https://img.shields.io/nuget/v/spectre.console.svg?style=flat&label=NuGet%3A%20Spectre.Console)](https://www.nuget.org/packages/spectre.console)_

`Spectre.Console`是一个 .NET 的库，可以更轻松地创建美观的跨平台控制台应用程序。

深受 [Rich](https://github.com/willmcgugan/rich) 这个Python优秀库的启发。

## 目录

1. [功能](#功能)
1. [安装](#安装)
1. [文档](#文档)
1. [例子](#例子)
1. [开源许可](#开源许可)

## 功能

* 编写时考虑到了单元测试。
* 支持 tables、grid、panel 和 [rich](https://github.com/willmcgugan/rich) 所支持的标记语言。
* 支持大部分的 SRG 参数，包括粗体、暗淡字、斜体、下划线、删除线和闪烁文本。
* 支持终端显示 3/4/8/24 位色。自动检测终端类型，自适应颜色范围。

![例子](resources/example.png)

## 安装

最快的安装方式，就是用NuGet包管理直接安装`Spectre.Console`。

```csharp
dotnet add package Spectre.Console
```

## 文档

`Spectre.Console`的文档可以在这里查看
https://spectreconsole.net/

## 例子

To see `Spectre.Console` in action, please see the 
[examples repository](https://github.com/spectreconsole/examples).

## 开源许可

版权所有 © Patrik Svensson, Phil Scott, Nils Andresen, Cédric Luthi, Frank Ray

Spectre.Console 基于 MIT 协议提供。查看 LICENSE 文件了解更多信息。

* SixLabors.ImageSharp 的协议请查看 https://github.com/SixLabors/ImageSharp/blob/master/LICENSE
