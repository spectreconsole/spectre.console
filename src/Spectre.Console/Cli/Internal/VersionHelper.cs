using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Spectre.Console.Cli.Internal
{
    internal static class VersionHelper
    {
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
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
