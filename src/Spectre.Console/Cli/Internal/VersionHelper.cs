using System.Diagnostics;
using System.Reflection;

namespace Spectre.Console.Cli
{
    internal static class VersionHelper
    {
        private static string? TryGetAssemblyVersion(Assembly? assembly) =>
            assembly?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

        private static FileVersionInfo? TryGetMainModuleVersionInfo() =>
            Process.GetCurrentProcess().MainModule?.FileVersionInfo;

        public static string GetVersion(Assembly? assembly) =>
            TryGetAssemblyVersion(assembly) ?? "?";

        public static string GetApplicationVersion() =>
            TryGetAssemblyVersion(Assembly.GetEntryAssembly()) ??
            TryGetMainModuleVersionInfo()?.ProductVersion ??
            "?";
    }
}
