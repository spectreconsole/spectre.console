namespace Spectre.Console.Cli;

internal static class EnumerableExtensions
{
    public static IReadOnlyList<T> ToSafeReadOnlyList<T>(this IEnumerable<T> source)
    {
        return source switch
        {
            null => new List<T>(),
            IReadOnlyList<T> list => list,
            _ => source.ToList(),
        };
    }
}