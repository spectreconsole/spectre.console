Title: Calendar
Order: 40
RedirectFrom: calendar
---

The `Calendar` is used to render a calendar to the terminal.

## Usage

To render a calendar, create a `Calendar` instance with a target date.

```csharp
var calendar = new Calendar(2020,10);
AnsiConsole.Render(calendar);
```

```text
               2020 October
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

You can set the calendar's culture to show localized weekdays.

```csharp
var calendar = new Calendar(2020,10);
calendar.Culture("ja-JP");
AnsiConsole.Render(calendar);
```

```text
               Oktober 2020
┌─────┬─────┬─────┬─────┬─────┬─────┬─────┐
│ Mån │ Tis │ Ons │ Tor │ Fre │ Lör │ Sön │
├─────┼─────┼─────┼─────┼─────┼─────┼─────┤
│     │     │     │ 1   │ 2   │ 3   │ 4   │
│ 5   │ 6   │ 7   │ 8   │ 9   │ 10  │ 11* │
│ 12  │ 13  │ 14  │ 15  │ 16  │ 17  │ 18  │
│ 19  │ 20  │ 21  │ 22  │ 23  │ 24  │ 25  │
│ 26  │ 27  │ 28  │ 29  │ 30  │ 31  │     │
│     │     │     │     │     │     │     │
└─────┴─────┴─────┴─────┴─────┴─────┴─────┘
```

## Header

You can hide the calendar header.

```csharp
var calendar = new Calendar(2020,10);
calendar.HideHeader();
AnsiConsole.Render(calendar);
```

```text
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

You can set the header style of the calendar.

```csharp
var calendar = new Calendar(2020, 10);
calendar.HeaderStyle(Style.Parse("blue bold"));
AnsiConsole.Render(calendar);
```


## Calendar Events

You can add an event to the calendar.
If a date has an event associated with it, the date gets highlighted in the calendar.

```csharp
var calendar = new Calendar(2020,10);
calendar.AddCalendarEvent(2020, 10, 11);
AnsiConsole.Render(calendar);
```

```text
               2020 October
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

## Highlight style

You can set the highlight style for a calendar event via `SetHighlightStyle`.

```csharp
var calendar = new Calendar(2020, 10);
calendar.AddCalendarEvent(2020, 10, 11);
calendar.HighlightStyle(Style.Parse("yellow bold"));
AnsiConsole.Render(calendar);
```
