using System.Runtime.CompilerServices;
using Spectre.Verify.Extensions;
using VerifyTests;

namespace Spectre.Console.Tests
{
    public static class VerifyConfiguration
    {
        [ModuleInitializer]
        public static void Init()
        {
            VerifierSettings.DerivePathInfo(Expectations.Initialize);
        }
    }
}