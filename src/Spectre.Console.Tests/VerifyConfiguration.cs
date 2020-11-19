using System.IO;
using System.Runtime.CompilerServices;

namespace Spectre.Console.Tests
{
    public static class VerifyConfiguration
    {
        [ModuleInitializer]
        public static void Init()
        {
            VerifyTests.VerifierSettings.DeriveTestDirectory((_, directory) =>
            {
                var expectations = Path.Combine(directory, "Expectations");
                if (!Directory.Exists(expectations))
                {
                    Directory.CreateDirectory(expectations);
                }

                return expectations;
            });
        }
    }
}