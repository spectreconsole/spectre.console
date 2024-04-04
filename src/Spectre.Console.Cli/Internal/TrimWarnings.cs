namespace Spectre.Console.Cli;

internal static class TrimWarnings
{
    public const string SuppressMessage =
        "Commands and settings should be marked with DynamicallyAccessed when building the commands.";

    public const string AddCommandShouldBeExplicitAboutSettings =
        "When trimming is enabled, use the AddCommand method with two generic parameters to explicitly include the settings.";

    public const string TypeConverterWarningsCanBeIgnored = "Type converter warnings can be ignored. Intrinsic types are always included.";
}