using System.Diagnostics;
using System.Reflection;

namespace Spectre.Console.Cli
{
    internal static class VersionHelper
    {
        public static string GetVersion(Assembly? assembly)
        {
            if (assembly == null)
            {
                return "?";
            }

            try
            {
                var info = FileVersionInfo.GetVersionInfo(assembly.Location);
                return info.ProductVersion ?? assembly?.GetName()?.Version?.ToString() ?? "?";
            }
            catch
            {
                return "?";
            }
        }
    }
}
