Title: Unit Testing
Order: 24
Description: Instructions for unit testing a Spectre.Console application.
---

`Spectre.Console` has a separate project that contains test harnesses for unit testing your own console applications. 

The fastest way of getting started is to install the `Spectre.Console.Testing` NuGet package.

```text
> dotnet add package Spectre.Console.Testing
```

`Spectre.Console.Testing` is also the namespace containing the test classes.

## Testing console behaviour

 `TestConsole` and `TestConsoleInput` are testable implementations of `IAnsiConsole` and `IAnsiConsoleInput`, allowing you fine-grain control over testing console output and interactivity.

The following example renders some widgets before then validating the console output:

```csharp
    [TestMethod]
    public void Should_Render_Panel()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Text("Hello World")));

        // Then
        Assert.AreEqual(console.Output, """"
┌─────────────┐
│ Hello World │
└─────────────┘

"""");
    }
```

While `Assert` is fine for validating simple output, more complex output may benefit from a tool like [Verify](https://github.com/VerifyTests/Verify).

The following example prompts the user for input before then validating the expected choice was made:

```csharp
    [TestMethod]
    public void Should_Select_Orange()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("Orange");

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .AddChoice("Banana")
                .AddChoice("Orange"));

        // Then
        Assert.AreEqual(console.Output, "Favorite fruit? [Banana/Orange]: Orange\n");
    }
```