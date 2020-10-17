Title: カラー
Order: 0
---

下の一覧は、ターミナルで利用できる8ビット色です。

最初の16色は通常システムか使用しているターミナルで定義されていて、ここで描画された通りに表示されないことがあります。

# 使用方法

`new Style(foreground: Color.Maroon)`のようなコード、または、`AnsiConsole.Markup("[maroon on blue]Hello[/]")`のようなマークアップテキストで色を使用できます。

# 標準的な色

<div class="input-group mb-3">
  <div class="input-group-prepend">
    <span class="input-group-text" id="basic-addon1">
        <i class="fas fa-search" aria-hidden="true"></i>
    </span>
  </div>
  <input
    class="form-control w-100 filter"
    data-table="color-results"
    type="text" placeholder="Search Colors..." autocomplete="off" 
    aria-label="Search Colors">
</div>

<?# ColorTable /?>

<script type="text/javascript" src="../../assets/js/table-search.js"></script>