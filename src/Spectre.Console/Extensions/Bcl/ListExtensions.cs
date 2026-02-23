namespace Spectre.Console;

internal static class ListExtensions
{
    public static void RemoveLast<T>(this List<T> list)
    {
        ArgumentNullException.ThrowIfNull(list);

        if (list.Count > 0)
        {
            list.RemoveAt(list.Count - 1);
        }
    }

    public static void AddOrReplaceLast<T>(this List<T> list, T item)
    {
        ArgumentNullException.ThrowIfNull(list);

        if (list.Count == 0)
        {
            list.Add(item);
        }
        else
        {
            list[list.Count - 1] = item;
        }
    }
}