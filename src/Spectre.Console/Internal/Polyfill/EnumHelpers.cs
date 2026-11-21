namespace Spectre.Console;

internal static class EnumUtils
{
    public static IReadOnlyList<T> GetValues<T>()
        where T : struct, Enum
    {
        return Cache<T>.Values;
    }

    private static class Cache<T>
        where T : struct, Enum
    {
        // Enum.GetValues(Type) creates an array of the enum type at runtime,
        // which fails under NativeAOT for the netstandard2.0 build. Parsing the
        // names works on every target. Enum.GetNames returns them in value order.
        public static readonly T[] Values = Enum.GetNames(typeof(T))
            .Select(name => (T)Enum.Parse(typeof(T), name))
            .ToArray();
    }
}