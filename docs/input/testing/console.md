Title: Testing Spectre Console
Description: "Test Spectre Console features,functionality, output, and performance."
---

The `TestConsole` class is used to test the output of `AnsiConsole`.

## TestConsole

The `TestConsole` class is a wrapper around the `AnsiConsole` object configured for testing, with no (current) support for input.

## Example

```csharp
public class ProgramTests
{   
    [Fact]
    public void Should_Return_Hello_World()
    {
        // Given
        var console = new TestConsole();
        // When
        console.WriteLine("Hello world");
        // Then
        Assert.Equal("Hello world" + Environment.NewLine, console.Output);
    }
}
```