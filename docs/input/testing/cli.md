Title: Testing Spectre CLI
Description: "Test Spectre CLI features,functionality, output, and performance."
---

## Introduction

This document describes how to test Spectre CLI using the `CommandAppTester` class.

## CommandAppTester

The `CommandAppTester` class is a wrapper around the `CommandApp` object configured for testing.

## Example

```csharp
public class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.AddCommand<HelloCommand>("hello");
        });

        var result = app.Run(args);
        return result;
    }
}

public class HelloCommand : Command
{
    public override int Execute(CommandContext context, string[] args)
    {
        context.Console.WriteLine("Hello world");
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
        app.Configure(config =>
        {
            config.AddCommand<HelloCommand>("hello");
        });

        // When
        var result = app.Run("hello");

        // Then
        result.ExitCode.ShouldBe(0);
        result.Output.ShouldBe("Hello world");
    }
}
```