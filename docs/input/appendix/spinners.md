Title: Spinners
Order: 4
Description: "*Spectre.Console* supports over 80 different types of spinners, and allows new ones to be defined. They are used automatically with the controls"
Highlights:
    - Status display
    - Progress display
---

For all available spinners, see https://jsfiddle.net/sindresorhus/2eLtsbey/embedded/result/

## Usage

Spinners can be used with [Progress](xref:progress) and [Status](xref:status).

```csharp
AnsiConsole.Status()
    .Spinner(Spinner.Known.Star)
    .Start("Thinking...", ctx => {
        // Omitted
    });
```

## Implementing a spinner

To implement your own spinner, all you have to do is 
inherit from the `Spinner` base class.

In the example below, the spinner will alternate between
the characters `A`, `B` and `C` every 100 ms.

```csharp
public sealed class MySpinner : Spinner
{
    // The interval for each frame
    public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
    
    // Whether or not the spinner contains unicode characters
    public override bool IsUnicode => false;

    // The individual frames of the spinner
    public override IReadOnlyList<string> Frames => 
        new List<string>
        {
            "A", "B", "C",
        };
}
```