Title: 罫線
Order: 2
---


テーブルやパネルに使用することができる幾つかの組み込み罫線があります。

# 表罫線

<img src="../../assets/images/borders/table.png" style="max-width: 100%;">

## 例

テーブル罫線を`SimpleHeavy`に設定する:

```csharp
var table = new Table();
table.Border = TableBorder.SimpleHeavy;
```

---

# パネル罫線

<img src="../../assets/images/borders/panel.png" style="max-width: 100%;">

## 例

パネル罫線を`Rounded`に設定する:

```csharp
var panel = new Panel("Hello World");
panel.Border = BoxBorder.Rounded;
```