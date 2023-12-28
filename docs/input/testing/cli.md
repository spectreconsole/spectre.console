Title: Testing Spectre CLI Apps
Description: "Test Spectre CLI features,functionality, output, and performance."
---

The `CommandAppTester` class is used to test a Spectre CLI application.
It should be used to test a `CommandApp` `Command`s and `CommandSetting`s.

## CommandAppTester

The `CommandAppTester` class is a wrapper around the `CommandApp` object configured for testing.

### Example

```csharp
public class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(appConfig);
        var result = app.Run(args);
        return result;
    }
    
    public static void appConfig(IConfigurator config)
    {
        config.AddCommand<HelloCommand>("hello");
    }
}

public class HelloCommand : Command
{
    public override int Execute(CommandContext context)
    {
        AnsiConsole.WriteLine("Hello world");
        return 0;
    }
}

public class ProgramTests
{
    [Fact]
    public void Should_Return_Hello_World()
    {
        // Given
        var app = new CommandAppTester();
        app.Configure(Program.appConfig);

        // When
        var result = app.Run("hello");

        // Then
        Assert.Multiple(() =>
        {
            Assert.Equal(0, result.ExitCode);
            Assert.Equal("Hello world", result.Output);
        });
    }
}
```