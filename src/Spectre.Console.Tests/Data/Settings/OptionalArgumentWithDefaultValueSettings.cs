using System.ComponentModel;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public sealed class OptionalArgumentWithDefaultValueSettings : CommandSettings
    {
        [CommandArgument(0, "[GREETING]")]
        [DefaultValue("Hello World")]
        public string Greeting { get; set; }
    }

    public sealed class OptionalArgumentWithDefaultValueAndTypeConverterSettings : CommandSettings
    {
        [CommandArgument(0, "[GREETING]")]
        [DefaultValue("5")]
        [TypeConverter(typeof(StringToIntegerConverter))]
        public int Greeting { get; set; }
    }

    public sealed class RequiredArgumentWithDefaultValueSettings : CommandSettings
    {
        [CommandArgument(0, "<GREETING>")]
        [DefaultValue("Hello World")]
        public string Greeting { get; set; }
    }
}
