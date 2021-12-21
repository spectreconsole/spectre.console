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

#if !NET5_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ModuleInitializerAttribute : Attribute
    {
    }
}
#endif