Title: Canvas Image
Order: 70
Description: "Use *ImageSharp* to parse images and render them as Ascii art to the console."
Reference: T:Spectre.Console.CanvasImage
---

To add [ImageSharp](https://github.com/SixLabors/ImageSharp) superpowers to 
your console application to draw images, you will need to install 
the [Spectre.Console.ImageSharp](https://www.nuget.org/packages/Spectre.Console.ImageSharp) NuGet package.

```text
> dotnet add package Spectre.Console.ImageSharp
```

## Loading images

Once you've added the `Spectre.Console.ImageSharp` NuGet package, 
you can create a new instance of `CanvasImage` to draw images to the console.

```csharp
// Load an image
var image = new CanvasImage("cake.png");

// Set the max width of the image.
// If no max width is set, the image will take
// up as much space as there is available.
image.MaxWidth(16);

// Render the image to the console
AnsiConsole.Write(image);
```

## Result

<?# AsciiCast cast="canvas-image" /?>

## Manipulating images

You can take full advantage of [ImageSharp](https://github.com/SixLabors/ImageSharp)
and manipulate images directly via its [Processing API](https://docs.sixlabors.com/api/ImageSharp/SixLabors.ImageSharp.Processing.html).

```csharp
// Load an image
var image = new CanvasImage("cake.png");
image.MaxWidth(32);

// Set a sampler that will be used when scaling the image.
image.BilinearResampler();

// Mutate the image using ImageSharp
image.Mutate(ctx => ctx.Grayscale().Rotate(-45).EntropyCrop());

// Render the image to the console
AnsiConsole.Write(image);
```

## Result

<?# AsciiCast cast="canvas-image-manipulation" /?>
