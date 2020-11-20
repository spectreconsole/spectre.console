using System.IO;
using System.Runtime.CompilerServices;

namespace Spectre.Console.Tests.Unit
{
    public static class TestConfig
    {
        [ModuleInitializer]
        public static void Init()
        {
            VerifyTests.VerifierSettings.DeriveTestDirectory((_, directory) =>
            {
                var snapshots = Path.Combine(directory, "Snapshots");
                Directory.CreateDirectory(snapshots);
                return Path.Combine(snapshots);
            });
        }
    }
}