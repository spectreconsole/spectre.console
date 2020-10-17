Title: 絵文字
Order: 3
---


どのような絵文字が使用できるかは、使用しているOSやターミナルに依存し、どのように表示されるかは保証されません。絵文字の幅計算は正確ではないため、表、パネル、グリッドで使用する場合は表示がずれるかもしれません。

完全な互換性を確保するために、Unicode 13.0 より以前の`Emoji_Presentation`カテゴリにあるものだけを使用することを検討してください。
公式の絵文字一覧
https://www.unicode.org/Public/UCD/latest/ucd/emoji/emoji-data.txt

# 使用方法

```csharp
// Markup
AnsiConsole.MarkupLine("Hello :globe_showing_europe_africa:!");

// Constant
var hello = "Hello " + Emoji.Known.GlobeShowingEuropeAfrica;
```

# テキスト内の絵文字を置き換える

```csharp
var phrase = "Mmmm :birthday_cake:";
var rendered = Emoji.Replace(phrase);
```

# 絵文字の再マッピングや追加

既存の絵文字を別のものにしたり、完全に新しい物を追加したいことがあります。このために、`Emoji.Remap`メソッドを使用できます。
この方法は、マークアップ文字と`Emoji.Replace`の両方で動作します。

```csharp
// Remap the emoji
Emoji.Remap("globe_showing_europe_africa", "😄");

// Render markup
AnsiConsole.MarkupLine("Hello :globe_showing_europe_africa:!");

// Replace emojis in string
var phrase = "Hello :globe_showing_europe_africa:!";
var rendered = Emoji.Replace(phrase);
```

# 絵文字

_次のテーブルのイメージは互換性で示したのと同じ理由でブラウザで正しく表示されない場合があります。_

<div class="input-group mb-3">
  <div class="input-group-prepend">
    <span class="input-group-text" id="basic-addon1">
        <i class="fas fa-search" aria-hidden="true"></i>
    </span>
  </div>
  <input
    class="form-control w-100 filter"
    data-table="emoji-results"
    type="text" placeholder="Search Emojis..." autocomplete="off" 
    aria-label="Search Emojis">
</div>

<?# EmojiTable /?>

<script type="text/javascript" src="../../assets/js/table-search.js"></script>