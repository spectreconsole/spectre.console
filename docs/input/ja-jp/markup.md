Title: マークアップ
Order: 2
---

`Markup`クラスは、コンソールにリッチなテキストを出力することができます。

# 文法

コンソールマークアップはbbcodeに影響を受けた文法を利用します。角括弧でスタイルを書いたら（スタイルを参照）、例えば、`[bold red]`
は、`[/]`で閉じるまでスタイルが適用されます。

```csharp
AnsiConsole.Render(new Markup("[bold yellow]Hello[/] [red]World![/]"));
```

`Markup` クラスは`IRenderable`を実装しており、table、grid、Panelを使用できることを意味します。
`IRenderable`のレンダリングに対応している多くのクラスは、リッチテキストの描画を上書きます。

```csharp
var table = new Table();
table.AddColumn(new TableColumn(new Markup("[yellow]Foo[/]")));
table.AddColumn(new TableColumn("[blue]Bar[/]"));
```

# 便利なメソッド

`AnsiConsole`には、新しい`Markup`インスタンスをインスタンス化することなく、コンソールにマークアップテキストを書き込める便利なメソッドがあります。

```csharp
AnsiConsole.Markup("[underline green]Hello[/] ");
AnsiConsole.MarkupLine("[bold]World[/]");
```

# エスケープ文字列

`[`を出力するために、 `[[`を利用し、`]`を出力するために`]]`を利用します。

```csharp
AnsiConsole.Markup("[[Hello]] "); // [Hello]
AnsiConsole.Markup("[red][[World]][/]"); // [World]
```

`SafeMarkup`拡張メソッドを使用することもできます。

```csharp
AnsiConsole.Markup("[red]{0}[/]", "Hello [World]".SafeMarkup());
```

# 背景色の設定

カラー指定の際に、`on`を付けることで、マークアップで背景色を設定できます。

```
[bold yellow on blue]Hello[/]
[default on blue]World[/]
```

# 絵文字の描画

マークアップの一部として絵文字を出力するために、emojiショートコードが使用できます。

```csharp
AnsiConsole.MarkupLine("Hello :globe_showing_europe_africa:!");
```

emojiのスタイルについては、付録の[Emoji](./appendix/emojis) を参照してください。

# カラー

上の例では、全ての色は名前で参照されています。
しかし、16進数やRGB表現をマークダウンで色指定に使用できます。

```csharp
AnsiConsole.Markup("[red]Foo[/] ");
AnsiConsole.Markup("[#ff0000]Bar[/] ");
AnsiConsole.Markup("[rgb(255,0,0)]Baz[/] ");
```

カラーのスタイルについては、付録の[Color](./appendix/colors) を参照してください。

# スタイル

リストのスタイルについては、付録の[Style](./appendix/styles) を参照してください。