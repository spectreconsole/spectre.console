# `Spectre.Console`

A .NET Standard 2.0 library that makes it easier to create beautiful console applications.  
It is heavily inspired by the excellent [Rich library](https://github.com/willmcgugan/rich) 
for Python.

## Features

* Written with unit testing in mind.
* Supports the most common SRG parameters when it comes to text 
  styling such as bold, dim, italic, underline, strikethrough, 
  and blinking text.
* Supports 3/4/8/24-bit colors in the terminal.  
  The library will detect the capabilities of the current terminal 
  and downgrade colors as needed.

## Example

![Example](https://spectresystems.se/assets/open-source/spectre-console/example.png)

## Usage

The `Spectre.Console` API is stateful and is not thread-safe.
If you need to write to the console from different threads, make sure that 
you take appropriate precautions, just like when you use the 
regular `System.Console` API.

If the current terminal does not support ANSI escape sequences, 
`Spectre.Console` will fallback to using the `System.Console` API.

_NOTE: This library is currently under development and API's 
might change or get removed at any point up until a 1.0 release._

### Using the static API

The static API is perfect when you just want to output text
like you usually do with the `System.Console` API, but prettier.

```csharp
AnsiConsole.Foreground = Color.CornflowerBlue;
AnsiConsole.Style = Styles.Underline | Styles.Bold;
AnsiConsole.WriteLine("Hello World!");

AnsiConsole.Reset();
AnsiConsole.WriteLine("Good bye!");
AnsiConsole.WriteLine();
```

If you want to get a reference to the default `IAnsiConsole`, 
you can access it via `AnsiConsole.Console`.

### Creating a console

Sometimes it's useful to explicitly create a console with specific 
capabilities, such as during unit testing when you want control 
over the environment your code runs in.

```csharp
IAnsiConsole console = AnsiConsole.Create(
    new AnsiConsoleSettings()
    {
        Ansi = AnsiSupport.Yes,
        ColorSystem = ColorSystemSupport.TrueColor,
        Out = new StringWriter(),
    });
```

_NOTE: Even if you can specify a specific color system to use 
when manually creating a console, remember that the user's terminal 
might not be able to use it, so unless you're creating an IAnsiConsole 
for testing, always use `ColorSystemSupport.Detect` and `AnsiSupport.Detect`._
