Title: Canvas
Order: 60
Description: "**Canvas** is a widget that allows you to render arbitrary pixels to the console."
Reference: T:Spectre.Console.Canvas
---

`Canvas` is a widget that allows you to render arbitrary "pixels" 
(or _coxels_, as [Simon Cropp](https://twitter.com/SimonCropp/status/1331554791726534657?s=20) 
suggested we should call them).

## Drawing primitives

```csharp
// Create a canvas
var canvas = new Canvas(16, 16);

// Draw some shapes
for(var i = 0; i < canvas.Width; i++)
{
    // Cross
    canvas.SetPixel(i, i, Color.White);
    canvas.SetPixel(canvas.Width - i - 1, i, Color.White);

    // Border
    canvas.SetPixel(i, 0, Color.Red);
    canvas.SetPixel(0, i, Color.Green);
    canvas.SetPixel(i, canvas.Height - 1, Color.Blue);
    canvas.SetPixel(canvas.Width - 1, i, Color.Yellow);
}

// Render the canvas
AnsiConsole.Write(canvas);
```

## Result

<?# AsciiCast cast="canvas" /?>
