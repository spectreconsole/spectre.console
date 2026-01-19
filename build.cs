#:sdk Cake.Sdk@6.0.0

var solution = "./src/Spectre.Console.slnx";

////////////////////////////////////////////////////////////////
// Arguments

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var noLinting = HasArgument("no-lint");

////////////////////////////////////////////////////////////////
// Tasks

Task("Clean")
    .Does(ctx =>
{
    ctx.CleanDirectory("./.artifacts");
});

Task("Lint")
    .WithCriteria(ctx => BuildSystem.IsLocalBuild || BuildSystem.IsPullRequest, "Not a local build or pull request")
    .WithCriteria(ctx => !noLinting, "Linting disabled by user")
    .Does(ctx =>
{
    ctx.DotNetFormatStyle(solution, new DotNetFormatSettings
    {
        VerifyNoChanges = true,
    });
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Lint")
    .Does(ctx =>
{
    ctx.DotNetBuild(solution, new DotNetBuildSettings
    {
        Configuration = configuration,
        Verbosity = DotNetVerbosity.Minimal,
        NoLogo = true,
        NoIncremental = ctx.HasArgument("rebuild"),
        MSBuildSettings = new DotNetMSBuildSettings()
            .TreatAllWarningsAs(MSBuildTreatAllWarningsAs.Error)
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(ctx =>
{
    ctx.DotNetTest(solution, new DotNetTestSettings
    {
        Configuration = configuration,
        Verbosity = DotNetVerbosity.Minimal,
        NoLogo = true,
        NoRestore = true,
        NoBuild = true,
    });
});

Task("Package")
    .IsDependentOn("Test")
    .Does(ctx =>
{
    ctx.DotNetPack(solution, new DotNetPackSettings
    {
        Configuration = configuration,
        Verbosity = DotNetVerbosity.Minimal,
        NoLogo = true,
        NoRestore = true,
        NoBuild = true,
        OutputDirectory = "./.artifacts",
        MSBuildSettings = new DotNetMSBuildSettings()
            .TreatAllWarningsAs(MSBuildTreatAllWarningsAs.Error)
    });
});

Task("Sign-Binaries")
    .IsDependentOn("Package")
    .WithCriteria(ctx => ctx.HasArgument("sign"), "Not signing binaries")
    .Does(ctx =>
{
    // Ensure the sign tool is installed
    ctx.StartProcess("dotnet", new ProcessSettings
    {
        Arguments = "tool install --tool-path .sign --prerelease sign"
    });

    var commandSettings = new CommandSettings
    {
        ToolExecutableNames = ["sign", "sign.exe"],
        ToolName = "sign",
        ToolPath = ResolveSignTool("sign.exe")
            ?? ResolveSignTool("sign")
            ?? throw new Exception("Failed to locate sign tool"),
    };

    var files = ctx.GetFiles("./.artifacts/*.nupkg");
    foreach (var file in files)
    {
        ctx.Information("Signing {0}...", file.FullPath);

        var arguments = new ProcessArgumentBuilder()
            .Append("code")
            .Append("azure-key-vault")
            .AppendQuoted(file.FullPath)
            .AppendSwitchQuoted("--file-list", ctx.MakeAbsolute(ctx.File("./resources/signclient.filter")).FullPath)
            .AppendSwitchQuoted("--publisher-name", "Spectre Console")
            .AppendSwitchQuoted("--description", "A .NET library that makes it easier to create beautiful console applications.")
            .AppendSwitchQuoted("--description-url", "https://spectreconsole.net")
            .AppendSwitchQuoted("--azure-credential-type", "azure-cli")
            .AppendSwitchQuotedSecret("--azure-key-vault-certificate", Argument<string>("keyvaultCertificate"))
            .AppendSwitchQuotedSecret("--azure-key-vault-url", Argument<string>("keyvaultUrl"));

        ctx.Command(commandSettings, arguments);
        ctx.Information("Done signing {0}.", file.FullPath);
    }

    FilePath? ResolveSignTool(string name)
    {
        var path = ctx.MakeAbsolute(ctx.Directory(".sign").Path.CombineWithFilePath(name));
        return ctx.FileExists(path) ? path : null;
    }
});

Task("Publish-NuGet")
    .WithCriteria(ctx => BuildSystem.IsRunningOnGitHubActions, "Not running on GitHub Actions")
    .IsDependentOn("Sign-Binaries")
    .Does(ctx =>
{
    var apiKey = Argument<string?>("nuget-key", null);
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        throw new CakeException("No NuGet API key was provided.");
    }

    foreach (var file in ctx.GetFiles("./.artifacts/*.nupkg"))
    {
        ctx.Information("Publishing {0}...", file.GetFilename().FullPath);
        DotNetNuGetPush(file.FullPath, new DotNetNuGetPushSettings
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = apiKey,
        });
    }
});

////////////////////////////////////////////////////////////////
// Targets

Task("Publish")
    .IsDependentOn("Publish-NuGet");

Task("Default")
    .IsDependentOn("Package");

////////////////////////////////////////////////////////////////
// Execution

RunTarget(target);