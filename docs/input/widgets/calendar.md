Title: Calendar
Order: 40
RedirectFrom: calendar
Description: "The **Calendar** is used to render a calendar to the terminal."
Highlights:
    - Include highlighted events.
    - Culture aware.
    - Custom headers.
Reference: T:Spectre.Console.Calendar

---

The `Calendar` is used to render a calendar to the terminal.

## Usage

To render a calendar, create a `Calendar` instance with a target date.

```csharp
var calendar = new Calendar(2020,10);
AnsiConsole.Write(calendar);
```

<?# AsciiCast cast="calendar" /?>

## Culture

You can set the calendar's culture to show localized weekdays.

```csharp
var calendar = new Calendar(2020,10);
calendar.Culture("sv-SE");
AnsiConsole.Write(calendar);
```

<?# AsciiCast cast="calendar-culture" /?>

## Header

You can hide the calendar header.

```csharp
var calendar = new Calendar(2020,10);
calendar.HideHeader();
AnsiConsole.Write(calendar);
```

You can set the header style of the calendar.

```csharp
var calendar = new Calendar(2020, 10);
calendar.HeaderStyle(Style.Parse("blue bold"));
AnsiConsole.Write(calendar);
```

<?# AsciiCast cast="calendar-header" /?>

## Calendar Events

You can add an event to the calendar.
If a date has an event associated with it, the date gets highlighted in the calendar.

```csharp
var calendar = new Calendar(2020,10);
calendar.AddCalendarEvent(2020, 10, 11);
AnsiConsole.Write(calendar);
```

You can set the highlight style for a calendar event via `SetHighlightStyle`.

```csharp
var calendar = new Calendar(2020, 10);
calendar.AddCalendarEvent(2020, 10, 11);
calendar.HighlightStyle(Style.Parse("yellow bold"));
AnsiConsole.Write(calendar);
```

<?# AsciiCast cast="calendar-highlight" /?>
