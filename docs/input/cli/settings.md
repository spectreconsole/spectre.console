Title: Specifying Settings
Order: 5
Description: "How to define command line argument settings for your *Spectre.Console.Cli* Commands"
Reference:
    - T:Spectre.Console.Cli.CommandSettings
    - T:Spectre.Console.Cli.CommandArgumentAttribute
    - T:Spectre.Console.Cli.CommandOptionAttribute
---

Settings for `Spectre.Console.Cli` commands are defined via classes that inherit from `CommandSettings`. Attributes are used to indicate how the parser interprets the command line arguments and create a runtime instance of the settings to be used.

Example:

```csharp
public sealed class MyCommandSettings : CommandSettings
{
    [CommandArgument(0, "[name]")]
    public string? Name { get; set; }

    [CommandOption("-c|--count")]
    public int? Count { get; set; }
}
```

This setting file tells `Spectre.Console.Cli` that our command has two parameters. One is marked as a `CommandArgument`, the other is a `CommandOption`.

## CommandArgument

Arguments have a position and a name. The name is not only used for generating help, but its formatting is used to determine whether or not the argument is optional. Angle brackets denote a required argument (e.g. `<name>`) whereas square brackets denote an optional argument (e.g. `[name]`). If neither are specified an exception will be thrown.

The position is used for scenarios where there could be more than one argument.

For example, we could split the above name argument into two values with an optional last name.

```csharp
[CommandArgument(0, "<firstName>")]
public string FirstName { get; set; }

[CommandArgument(1, "[lastName]")]
public string? LastName { get; set; }
```

## CommandOption

`CommandOption` is used when you have options that are passed in command line switches. The attribute has one parameter - a pipe delimited string with the list of argument names. The following rules apply:

* As many names can be specified as you wish, they just can't conflict with other arguments.
* Options with a single character must be preceded by a single dash (e.g. `-c`).
* Multi-character options must be preceded by two dashes (e.g. `--count`).

### Flags

There is a special mode for `CommandOptions` on boolean types. Typically all `CommandOptions` require a value to be included after the switch. For these only the switch needs to be specified to mark the value as true. This example would allow the user to run either `app.exe --debug`, or `app.exe --debug true`.

```csharp
[CommandOption("--debug")]
public bool? Debug { get; set; }
```

### Hidden options

`CommandOptions` can be hidden from being rendered in help by setting `IsHidden` to `true`.

```csharp
[CommandOption("--hidden-opt", IsHidden = true)]
public bool? HiddenOpt { get; set; }
```

## Description

When rendering help the [`System.ComponentModel.Description`](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.descriptionattribute?view=net-5.0) attribute is supported for specifying the text displayed to the user for both `CommandOption` and `CommandArgument`.

## DefaultValue

The [`System.ComponentModel.DefaultValue`](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.defaultvalueattribute?view=net-5.0) attribute supported to specify a default value for a command. For example, in the hello example displaying hello for a default count of zero wouldn't make sense. We can change this to a single hello:

```csharp
[CommandOption("-c|--count")]
[DefaultValue(1)]
public int Count { get; set; }
```

## TypeConverter

`System.ComponentModel.TypeConverter` is supported for more complex arguments, such as mapping log levels to an enum via a [`TypeConverter`](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.typeconverter?view=net-5.0).

## Arrays

### Argument Vector

One (exactly one) `CommandArgument` can be defined as an array, and any additional parameters will be included in the value. For example:

```csharp
[CommandArgument(0, "[name]")]
public string[] Name { get; set; }
```

Would allow the user to run `app.exe Dwayne Elizondo "Mountain Dew" Herbert Camacho`. The settings passed to the command would have a 5 element array consisting of Dwayne, Elizondo, Mountain Dew, Herbert and Camacho.

A command can have only one argument vector, and it needs to be the last argument. (I.e. there can be no `CommandArgument` whose position is higher than that of the argument vector.)

### Option Arrays

A `CommandOption` can be defined as an array like the following:

```csharp
[CommandOption("-n|--name <VALUES>")]
public string[] Names { get; set; },
```

This would allow the user to run `app.exe --name Dwayne --name Elizondo --name "Mountain Dew" --name Herbert --name Camacho` and would result in a 5 element array consisting of Dwayne, Elizondo, Mountain Dew, Herbert and Camacho.

## Constructors

`Spectre.Console.Cli` supports constructor initialization and init only initialization. For constructor initialization, the parameter name of the constructor must match the name of the property name of the settings class. Order does not matter.

```csharp
public class Settings : CommandSettings
{
    public Settings(string[] name)
    {
        Name = name;
    }

    [Description("The name to display")]
    [CommandArgument(0, "[Name]")]
    public string? Name { get; }
}
```

Also supported are init only properties.

```csharp
public class Settings : CommandSettings
{
    [Description("The name to display")]
    [CommandArgument(0, "[Name]")]
    public string? Name { get; init; }
}
```

## Validation

Simple type validation is performed automatically, but for scenarios where more complex validation is required, overriding the `Validate` method is supported. This method must return either `ValidationResult.Error` or `ValidationResult.Success`.

```csharp
public class Settings : CommandSettings
{
    [Description("The name to display")]
    [CommandArgument(0, "[Name]")]
    public string? Name { get; init; }

    public override ValidationResult Validate()
    {
        return Name.Length < 2
            ? ValidationResult.Error("Names must be at least two characters long")
            : ValidationResult.Success();
    }
}
```
