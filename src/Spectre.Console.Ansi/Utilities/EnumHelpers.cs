namespace Spectre.Console;

internal static class EnumUtils
{
    public static T[] GetValues<T>()
        where T : struct, Enum
    {
        return
#if NET6_0_OR_GREATER
            Enum.GetValues<T>();
#else
            (T[])Enum.GetValues(typeof(T));
#endif
    }
}