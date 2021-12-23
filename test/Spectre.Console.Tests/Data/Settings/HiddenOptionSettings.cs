namespace Spectre.Console.Tests.Data;

public sealed class HiddenOptionSettings : CommandSettings
{
    [CommandArgument(0, "<FOO>")]
    [Description("Dummy argument FOO")]
    public int Foo { get; set; }

    [CommandOption("--bar", IsHidden = true)]
    [Description("You should not be able to read this unless you used the 'cli explain' command with the '--hidden' option")]
    public int Bar { get; set; }

    [CommandOption("--baz")]
    [Description("Dummy option BAZ")]
    public int Baz { get; set; }
}
