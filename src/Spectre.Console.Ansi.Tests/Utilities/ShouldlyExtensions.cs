using System.Diagnostics;

namespace Spectre.Console.Ansi.Tests;

public static class ShouldlyExtensions
{
    [DebuggerStepThrough]
    public static T And<T>(this T item)
    {
        return item;
    }

    [DebuggerStepThrough]
    public static T And<T>(this T item, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        action(item);
        return item;
    }
}