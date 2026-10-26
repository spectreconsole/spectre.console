using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Spectre.Console.NativeAot.Tests;

/// <summary>
/// A deliberately awkward call chain: async, iterator, generic, local function,
/// tuple/params/out parameters and a [StackTraceHidden] frame. Every shape here is
/// something <c>ExceptionRenderableBuilder</c> treats specially, so the resulting
/// stack trace is a good stress test for what NativeAOT does and doesn't preserve.
/// </summary>
internal static class Boom
{
    public static async Task ThrowAsync()
    {
        await Task.Yield();

        try
        {
            Level1([1, 2, 3]);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Outer boom, thrown from an async method.", ex);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Level1(int[] values)
    {
        foreach (var value in Enumerate(values))
        {
            Level2<string>(value, out _, "alpha", "beta");
        }
    }

    private static IEnumerable<int> Enumerate(int[] values)
    {
        foreach (var value in values)
        {
            yield return value * 2;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Level2<T>(int value, out T? result, params string[] extra)
    {
        result = default;
        Level3((Name: string.Join('-', extra), Count: value));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Level3((string Name, int Count) tuple)
    {
        ThrowHidden(tuple.Name);

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ThrowHidden(string name) => Guard(name);
    }

    // Frames marked with this attribute are meant to be filtered out of the rendered trace.
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Guard(string name)
        => throw new ArgumentOutOfRangeException(nameof(name), name, "Inner boom, thrown from a hidden frame.");
}
