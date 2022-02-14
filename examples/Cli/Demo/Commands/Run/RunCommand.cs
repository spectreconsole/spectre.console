using System.ComponentModel;
using Demo.Utilities;
using Spectre.Console.Cli;

namespace Demo.Commands.Run;

[Description("Build and run a .NET project output.")]
public sealed class RunCommand : Command<RunCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("-c|--configuration <CONFIGURATION>")]
        [Description("The configuration to run for. The default for most projects is '[grey]Debug[/]'.")]
        [DefaultValue("Debug")]
        public string Configuration { get; set; }

        [CommandOption("-f|--framework <FRAMEWORK>")]
        [Description("The target framework to run for. The target framework must also be specified in the project file.")]
        public string Framework { get; set; }

        [CommandOption("-r|--runtime <RUNTIMEIDENTIFIER>")]
        [Description("The target runtime to run for.")]
        public string RuntimeIdentifier { get; set; }

        [CommandOption("-p|--project <PROJECTPATH>")]
        [Description("The path to the project file to run (defaults to the current directory if there is only one project).")]
        public string ProjectPath { get; set; }

        [CommandOption("--launch-profile <LAUNCHPROFILE>")]
        [Description("The name of the launch profile (if any) to use when launching the application.")]
        public string LaunchProfile { get; set; }

        [CommandOption("--no-launch-profile")]
        [Description("Do not attempt to use [grey]launchSettings.json[/] to configure the application.")]
        public bool NoLaunchProfile { get; set; }

        [CommandOption("--no-build")]
        [Description("Do not build the project before running. Implies [grey]--no-restore[/].")]
        public bool NoBuild { get; set; }

        [CommandOption("--interactive")]
        [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
        public string Interactive { get; set; }

        [CommandOption("--no-restore")]
        [Description("Do not restore the project before building.")]
        public bool NoRestore { get; set; }

        [CommandOption("--verbosity <VERBOSITY>")]
        [Description("Set the MSBuild verbosity level. Allowed values are q[grey]uiet[/], m[grey]inimal[/], n[grey]ormal[/], d[grey]etailed[/], and diag[grey]nostic[/].")]
        [TypeConverter(typeof(VerbosityConverter))]
        [DefaultValue(Verbosity.Normal)]
        public Verbosity Verbosity { get; set; }

        [CommandOption("--no-dependencies")]
        [Description("Do not restore project-to-project references and only restore the specified project.")]
        public bool NoDependencies { get; set; }

        [CommandOption("--force")]
        [Description("Force all dependencies to be resolved even if the last restore was successful. This is equivalent to deleting [grey]project.assets.json[/].")]
        public bool Force { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        SettingsDumper.Dump(settings);
        return 0;
    }
}
