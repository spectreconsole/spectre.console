Title: クイックスタート
Order: 1
---

Spectre.Consoleの利用を開始する最初の方法は、Nugetパッケージをインストールすることです。

```shell
> dotnet add package Spectre.Console
```

その後、`Spectre.Console`名前空間を参照する必要があります。一度参照したら、提供されている全ての機能を使用できます。

```csharp
using Spectre.Console

public static class Program
{
    public static void Main(string[] args)
    {
        AnsiConsole.Markup("[underline red]Hello[/] World!");
    }
}
```
