Title: 例外
Order: 3
---

例外はターミナルで見たときに読みやすいとは限りません。
`WriteException`メソッドを使用することで、例外をもう少し読みやすくすることができます。

```csharp
AnsiConsole.WriteException(ex);
```

<img src="../assets/images/exception.png" style="max-width: 100%;">

## 省略表示

例外の特定部分を短くして、さらに読みやすくしたり、パスをクリック可能なハイパーリンクにすることもできます。
ハイパーリンクがクリックできるかはターミナル次第です。


```csharp
AnsiConsole.WriteException(ex, 
    ExceptionFormat.ShortenPaths | ExceptionFormat.ShortenTypes |
    ExceptionFormat.ShortenMethods | ExceptionFormat.ShowLinks);
```

<img src="../assets/images/compact_exception.png" style="max-width: 100%;">

## 例外出力のカスタマイズ

例外の特定部分を短縮するだけでなく、デフォルトのスタイルを上書きすることもできます。

```csharp
AnsiConsole.WriteException(ex, new ExceptionSettings
{
    Format = ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks,
    Style = new ExceptionStyle
    {
        Exception = Style.WithForeground(Color.Grey),
        Message = Style.WithForeground(Color.White),
        NonEmphasized = Style.WithForeground(Color.Cornsilk1),
        Parenthesis = Style.WithForeground(Color.Cornsilk1),
        Method = Style.WithForeground(Color.Red),
        ParameterName = Style.WithForeground(Color.Cornsilk1),
        ParameterType = Style.WithForeground(Color.Red),
        Path = Style.WithForeground(Color.Red),
        LineNumber = Style.WithForeground(Color.Cornsilk1),
    }
});
```

<img src="../assets/images/custom_exception.png" style="max-width: 100%;">
