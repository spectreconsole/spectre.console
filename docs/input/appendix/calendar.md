Title: Calendar
Order: 4
---

The `Calendar` API is easy to render the calendar on your terminal.

# Usage

To render a calender, create a `Calendar` instance which specify target date.

```csharp
var calendar = new Calendar(2020,10);
AnsiConsole.Render(calendar);

┌─────┬─────┬─────┬─────┬─────┬─────┬─────┐
│ Sun │ Mon │ Tue │ Wed │ Thu │ Fri │ Sat │
├─────┼─────┼─────┼─────┼─────┼─────┼─────┤
│     │     │     │     │ 1   │ 2   │ 3   │
│ 4   │ 5   │ 6   │ 7   │ 8   │ 9   │ 10  │
│ 11  │ 12  │ 13  │ 14  │ 15  │ 16  │ 17  │
│ 18  │ 19  │ 20  │ 21  │ 22  │ 23  │ 24  │
│ 25  │ 26  │ 27  │ 28  │ 29  │ 30  │ 31  │
│     │     │     │     │     │     │     │
└─────┴─────┴─────┴─────┴─────┴─────┴─────┘

```

## Culture

You can set the culture for day of week.

```csharp
var calendar = new Calendar(2020,10);
calendar.SetCulture("ja-JP");
AnsiConsole.Render(calendar);

┌────┬────┬────┬────┬────┬────┬────┐
│ 日 │ 月 │ 火 │ 水 │ 木 │ 金 │ 土 │
├────┼────┼────┼────┼────┼────┼────┤
│    │    │    │    │ 1  │ 2  │ 3  │
│ 4  │ 5  │ 6  │ 7  │ 8  │ 9  │ 10 │
│ 11 │ 12 │ 13 │ 14 │ 15 │ 16 │ 17 │
│ 18 │ 19 │ 20 │ 21 │ 22 │ 23 │ 24 │
│ 25 │ 26 │ 27 │ 28 │ 29 │ 30 │ 31 │
│    │    │    │    │    │    │    │
└────┴────┴────┴────┴────┴────┴────┘

```

## Calendar Event

You can add the event to the calender. If the date has the event, the date is highlighted.

```csharp
var calendar = new Calendar(2020,10);
calendar.AddCalendarEvent(2020, 10, 11);
AnsiConsole.Render(calendar);

┌─────┬─────┬─────┬─────┬─────┬─────┬─────┐
│ Sun │ Mon │ Tue │ Wed │ Thu │ Fri │ Sat │
├─────┼─────┼─────┼─────┼─────┼─────┼─────┤
│     │     │     │     │ 1   │ 2   │ 3   │
│ 4   │ 5   │ 6   │ 7   │ 8   │ 9   │ 10  │
│ 11* │ 12  │ 13  │ 14  │ 15  │ 16  │ 17  │
│ 18  │ 19  │ 20  │ 21  │ 22  │ 23  │ 24  │
│ 25  │ 26  │ 27  │ 28  │ 29  │ 30  │ 31  │
│     │     │     │     │     │     │     │
└─────┴─────┴─────┴─────┴─────┴─────┴─────┘

```

### Highlight style

You can set the highlight style for event via `SetHighlightStyle`.

```csharp
var calendar = new Calendar(2020,10);
calendar.AddCalendarEvent(2020, 10, 11);
calendar.SetHighlightStyle(Style.WithDecoration(Decoration.Bold));
AnsiConsole.Render(calendar);

```