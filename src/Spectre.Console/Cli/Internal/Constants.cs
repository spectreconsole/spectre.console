namespace Spectre.Console.Cli
{
    internal static class CliConstants
    {
        public const string DefaultCommandName = "__default_command";
        public const string True = "true";
        public const string False = "false";

        public static string[] AcceptedBooleanValues { get; } = new string[]
        {
            True,
            False,
        };

        public static class Commands
        {
            public const string Branch = "cli";
            public const string Version = "version";
            public const string XmlDoc = "xmldoc";
            public const string Explain = "explain";
        }
    }
}
