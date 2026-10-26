using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.NativeAot.Tests;

internal static class NativeAot
{
    public static bool IsRunning { get; } = Probe();

    public static void EnsureRunning()
    {
        Assert.SkipUnless(
            IsRunning,
            "These tests only mean something under NativeAOT. Run them with dotnet publish.");
    }

    // GetMethod() is exactly what NativeAOT takes away, so asking for it is the most direct
    // way to tell whether the tests are running where they mean something. RuntimeFeature is
    // no help: PublishAot writes IsDynamicCodeSupported=false into runtimeconfig.json, so a
    // JIT run of this project reports the same as a published one.
    [UnconditionalSuppressMessage("Trimming", "IL2026:RequiresUnreferencedCode",
        Justification = "Calling it is the probe.")]
    private static bool Probe() => new StackFrame(0).GetMethod() == null;
}
