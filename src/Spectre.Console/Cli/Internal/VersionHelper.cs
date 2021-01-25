using System.Reflection;

namespace Spectre.Console.Cli
{
    internal static class VersionHelper
    {
        public static string GetVersion(Assembly? assembly)
        {
            return assembly?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion ?? "?";
        }
    }
}
