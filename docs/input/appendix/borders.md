Title: Borders
Order: 2
---

There is different built-in borders you can use for tables and panels.

## Table borders

![Examples of table borders](../assets/images/borders/table.png)

### Example

To set a table border to `SimpleHeavy`:

```csharp
var table = new Table();
table.Border = TableBorder.SimpleHeavy;
```

---

## Panel borders

![Examples of panel borders](../assets/images/borders/panel.png)

### Example

To set a panel border to `Rounded`:

```csharp
var panel = new Panel("Hello World");
panel.Border = BoxBorder.Rounded;
```
